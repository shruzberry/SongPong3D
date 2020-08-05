using System.Collections;
using UnityEngine;
using Types;

public class BounceBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;
    public Vector2 fallAxisBounds;

    //________MOVEMENT______________
    [Header("Movement")]
    protected Vector3 velocity;
    public float speed;
    public float gravity;
    public float baseHeight = 1;
    public float bounceHeight;

    //________COMPONENTS____________
    [Header("Components")]
    private Rigidbody rb;
    private Animator animator;
    public GameObject ring1;
    public GameObject ring2;
    public GameObject ring3;

    //________APPEARANCE____________
    [Header("Appearance")]
    [ColorUsage(true, true)]
    public Color dissolveColor;

    //________MOVEMENT______________
    private float deltaH;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void InitializeBall(Game game, BallData data, BallDropper dropper)
    {
        base.InitializeBall(game, data, dropper);

        // downcast data to specific type
        BounceBallData bounceData = (data as BounceBallData);

        // COMPONENTS
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // ATTRIBUTES
        radius = GetComponent<SphereCollider>().bounds.size.y / 2;
        InitializeRings();

        // MOVEMENT
        fallAxisBounds = dropper.fallAxisBounds;
        speed = dropper.startSpeed;
        gravity = dropper.gravity;
        velocity = speed * axisVector * negative;

        // OPTIONS
        bounceHeight = bounceData.GetOption("Bounce Height");
    }

    private void InitializeRings()
    {
        ring1.SetActive(true);
        ring2.SetActive(true);
        ring3.SetActive(true);

        if(numNotes < 4)
        {
            ring3.SetActive(false);
        }
        if(numNotes < 3)
        {
            ring2.SetActive(false);
        }
        if(numNotes < 2)
        {
            ring1.SetActive(false);
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE TIME
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Returns the needed to hit on the next notes' time.
     */
    private float CalcBounceTime()
    {
        // Calculate time to hit the next note (this is returned)
        float deltaT = notes[currentNote].hitBeat - song.GetSongTimeBeats();

        // Convert to time
        deltaT = song.ToTime(deltaT);

        // Check if notes are out of order
        if(deltaT < 0){Debug.LogError("NOTES ARE OUT OF ORDER ON " + type + " BALL " + id);}

        return deltaT;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     *  Updates the balls' velocity and time for its next hit
     */
    public override void ResetMove()
    {
        // How many beats until next hit
        float moveTime = CalcBounceTime();
        // Time to reach peak (with constant motion)
        float halfTime = moveTime / 2;

        // Get the distance to travel (world units)
        float deltaD = baseHeight + bounceHeight - radius;
        float otherDeltaD = Vector3.Dot(GetNotePosition(currentNote) - GetNotePosition(currentNote - 1), axisVector); // distance on other axis

        // Calculate the initial velocity
        // u = (2s/t) - v
        // (v = 0 because this is to the peak)
        float initialVelocity = ((2 * deltaD) / halfTime) - 0;

        // Calculate the gravity
        // a = (v - u) / t
        gravity = -initialVelocity / halfTime;
        gravity = Mathf.Abs(gravity);

        // Move along game axis
        velocity = Vector3.zero;
        velocity += axisVector * -initialVelocity * negative;

        // Move along the other axis
        velocity += otherAxisVector * (otherDeltaD / moveTime);
        Debug.Log(velocity);
    }

    private Vector2 Abs(Vector2 in_vec)
    {
        Vector2 new_vec = in_vec;
        new_vec.x = Mathf.Abs(in_vec.x);
        new_vec.y = Mathf.Abs(in_vec.y);
        return new_vec;
    }

    public override void MoveActions()
    {
        // UPDATE VELOCITY
        Vector3 velocityStep = axisVector * (gravity * Time.deltaTime) * negative;
        velocity += velocityStep;

        // UPDATE POSITION
        Vector3 newPos = new Vector3(velocity.x * Time.deltaTime, 0.0f, velocity.z * Time.deltaTime);

        rb.MovePosition(transform.position + newPos);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MISS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override bool CheckMiss()
    {
        float positionOnAxis = Vector3.Dot(transform.position, axisVector);
        float minValueOnAxis = fallAxisBounds.x;
        if(positionOnAxis < minValueOnAxis)
        {
            missed = true;
        }
        return missed;
    }

    public override void MissActions(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            caught = true;
            catchTimesBeats[currentNote] = song.GetSongTimeBeats();
        }
    }

    public override void OnCatchActions()
    {
        base.OnCatchActions();
        if(numNotes - currentNote < 4)
        {
            ring3.GetComponent<Animator>().SetTrigger("fadeOut");
        }
        if(numNotes - currentNote < 3)
        {
            ring2.GetComponent<Animator>().SetTrigger("fadeOut");
        }
        if(numNotes - currentNote < 2)
        {
            ring1.GetComponent<Animator>().SetTrigger("fadeOut");
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void OnExitActions()
    {
        animator.SetTrigger("isFinished");

        velocity = BounceVelocity(velocity);
    }

    public override void ExitActions()
    {
        MoveActions();
    }

    public void OnAnimationFinish()
    {
        exit = true;
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * VELOCITY
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Flips the direction of the velocity on the gravity axis
     */
    public Vector3 BounceVelocity(Vector2 velocity)
    {
        Vector3 newVel = Vector3.zero;
        newVel -= axisVector * Vector3.Dot(axisVector, velocity);
        newVel += otherAxisVector * Vector3.Dot(otherAxisVector, velocity);
        return newVel;
    }
}
