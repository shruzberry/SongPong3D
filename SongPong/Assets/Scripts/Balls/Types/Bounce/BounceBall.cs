using System.Collections;
using UnityEngine;
using Types;

public class BounceBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;

    [ColorUsage(true, true)]
    public Color dissolveColor;

    //________MOVEMENT______________
    protected Vector2 velocity;
    public float speed = 0.0f;
    public float gravity = 3.0f;

    public float bounceHeight;

    //________COMPONENTS____________
    Vector3 screenBounds;
    public Rigidbody2D rb;
    public Animator animator;

    //________MOVEMENT______________
    private float deltaH;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void InitializeBallSpecific(BallData data)
    {
        // downcast data to specific type
        BounceBallData d = (data as BounceBallData);

        // COMPONENTS
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // ATTRIBUTES
        radius = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        ball_renderer.material.SetColor("_Color", dissolveColor);

        // MOVEMENT
        velocity = speed * axisVector;

        // OPTIONS
        bounceHeight = d.GetOption("Bounce Height");
    }

    protected override bool CheckForInvalid()
    {
        bool error = false;

        if(numNotes < 2) error = true;

        // same note more than once

        // notes have different directions

        return error;
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
        SetAxisVectors();

        // How many beats until next hit
        float moveTime = CalcBounceTime();
        // Time to reach peak (with constant motion)
        float halfTime = moveTime / 2;

        // Get the distance to travel (world units)
        float deltaD = bounceHeight - radius;
        Vector2 otherDeltaD = GetNotePosition(currentNote) - GetNotePosition(currentNote - 1); // distance on other axis

        // Calculate the initial velocity
        // u = (2s/t) - v
        // (v = 0 because this is to the peak)
        float initialVelocity = ((2 * deltaD) / halfTime) - 0;

        // Calculate the gravity
        // a = (v - u) / t
        gravity = -initialVelocity / halfTime;
        gravity = Mathf.Abs(gravity);

        // Move along game axis
        velocity = Vector2.zero;
        velocity += axisVector * -initialVelocity;

        // Move along the other axis
        velocity += otherAxisVector * (otherDeltaD / moveTime);
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
        // UPDATE VELOCITY along main axis
        Vector2 velocityStep = axisVector * (gravity * Time.deltaTime);
        velocity += velocityStep;

        // UPDATE POSITION
        Vector3 newPos = new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0.0f);
        rb.MovePosition(transform.position + newPos);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MISS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override bool CheckMiss()
    {
        if(!ball_renderer.isVisible) missed = true;
        return missed;
    }

    public override void MissActions(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            caught = true;
            catchTimesBeats[currentNote] = song.GetSongTimeBeats();
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
    public Vector2 BounceVelocity(Vector2 velocity)
    {
        Vector2 newVel = Vector2.zero;
        newVel -= axisVector * Vector2.Dot(axisVector, velocity);
        newVel += otherAxisVector * Vector2.Dot(otherAxisVector, velocity);
        return newVel;
    }
}
