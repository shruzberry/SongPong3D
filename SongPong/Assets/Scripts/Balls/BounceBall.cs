using System.Collections;
using UnityEngine;
using Types;

public class BounceBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;

    //________MOVEMENT______________
    protected Vector2 velocity;
    public float speed = 0.0f;
    public float gravity = 3.0f;

    //________COMPONENTS____________
    Vector3 screenBounds;
    public Rigidbody2D rb;

    //________MOVEMENT______________
    private float deltaH;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void InitializeBallSpecific()
    {
        // ATTRIBUTES
        size = GetComponent<Collider2D>().bounds.size.y;
        radius = size / 2;

        // COMPONENTS
        rb = GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // MOVEMENT
        velocity = speed * axisVector;
    }

    protected override bool CheckForInvalid()
    {
        bool error = false;

        if(numNotes < 2) error = true;

        // same note more than once

        // notes have different directionss

        return error;
    }
    
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE TIME
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Returns the needed to hit on the next notes' time.
     * TODO change to song time?
     */
    private float CalcBounceTime()
    {
        // Calculate time to hit the next note (this is returned)
        float deltaT = notes[currentNote].hitTime - song.GetSongTime();

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

        float moveTime = CalcBounceTime();

        // Get distance between the current column and the next
        Vector2 deltaD = GetNotePosition(currentNote) - GetNotePosition(currentNote - 1);

        // Calculate the velocity needed to hit at new deltaT.
        // Comes from the kinematic equation v = v0 + at solved for v0
        // v0 = -at
        // we calculate only time to reach the peak, so (t/2)
        velocity = Vector2.zero;
        velocity += axisVector * -gravity * (moveTime / 2);
        velocity += otherAxisVector * (deltaD / moveTime);
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
            catchTimes[currentNote] = song.GetSongTime();
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void OnExitActions()
    {
        StartCoroutine(WaitThenDestroy());
    }

    public override void ExitActions()
    {
        
    }

    IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        exit = true;
    }
}
