using System.Collections;
using UnityEngine;
using Types;

public class BounceBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;
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

        SetDirectionSettings();

        // CALC DROP TIME
        moveTime = CalcMoveTime();
    }

    public void SetDirectionSettings()
    {
        if(axis == Axis.y && direction == Direction.positive) {dirVector = new Vector2(0,1);}
        else if(axis == Axis.y && direction == Direction.negative) {dirVector = new Vector2(0,-1);}
        else if(axis == Axis.x && direction == Direction.positive) {dirVector = new Vector2(1,0);}
        else if(axis == Axis.x && direction == Direction.negative) {dirVector = new Vector2(-1,0);}

        velocity = speed * dirVector;
        acceleration = gravity * dirVector;

        speed = (direction == Direction.negative) ? -speed : speed;
        gravity = (direction == Direction.negative) ? -gravity : gravity;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DROP TIME
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override float CalcMoveTime()
    {
        if(currentNote == 0)
            return CalcDropTime();
        else
            return CalcBounceTime();
    }

    /**
     * Calculates the DropTime for the BounceBall when it first falls from spawn
     */
    private float CalcDropTime()
    {
        // Check if ball is negative or positive
        float negative = (direction == Direction.negative) ? -1.0f : 1.0f;

        // Get Paddle Info
        // returns abs value of paddle axis, makes it negative if ball is negative direction
        float paddleAxis = paddle.getPaddleAxis();
        float paddleHeightHalf = paddle.getPaddleHeight() / 2;
        
        // Determine Delta H (Height)
        float spawn = (axis == Axis.y) ? spawnLoc.y : spawnLoc.x; // decide which coordinate to use depending on the axis
        deltaH = negative * (paddleAxis - spawn - radius - paddleHeightHalf); // calculate height to fall

        // Calculate delta T
        // using physics equation dy = v0t + 1/2at^2 solved for time in the form
        // t = (-v0 +- sqrt(v0^2 + 2*a*dy)) / a
        float determinant = (Mathf.Pow(speed, 2) + (2 * gravity * deltaH));
        float time = (-speed + negative * Mathf.Sqrt(determinant)) / gravity;

        if(float.IsNaN(time)){Debug.LogError("Drop time is NaN.");}
        return time;
    }

    /**
     * Calculates the velocity needed to hit on the next notes' time.
     */
    private float CalcBounceTime()
    {
        float deltaT = notes[currentNote].hitTime - Time.time;

        // Check if notes are out of order
        if(deltaT < 0){Debug.LogError("NOTES ARE OUT OF ORDER ON " + type + " BALL " + id);}

        // Calculate the velocity needed to hit at given deltaT.
        // Comes from the kinematic equation v = v0 + at solved for v0
        // v0 = -at
        // we calculate only time to reach the peak, so (t/2)
        velocity = -acceleration * (deltaT / 2);
        return deltaT;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void MoveActions()
    {
        // UPDATE VELOCITY
        Vector2 velocityStep = acceleration * Time.deltaTime;

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
        if(axis == Axis.y)
        {
            if(transform.position.y < -screenBounds.y && !missed)
            {
                missed = true;
            }
        }
        else if(axis == Axis.x)
        {
            if((transform.position.x < -screenBounds.x || transform.position.x > screenBounds.x) && !missed)
            {
                missed = true;
            }
        }

        return missed;
    }

    public override void MissActions(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Paddle"){
            caught = true;
            catchTimes[currentNote] = Time.time;
        }
    }

    public override void CatchActions()
    {
        base.CatchActions();
        velocity = -velocity;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void ExitActions()
    {
        StartCoroutine(WaitThenDestroy());
    }

    IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        exit = true;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DEBUG
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
}
