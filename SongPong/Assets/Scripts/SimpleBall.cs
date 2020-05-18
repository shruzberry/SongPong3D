using System.Collections;
using UnityEngine;
using Types;

public class SimpleBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;
    public float speed = 1.0f;
    public float gravity = 3.0f;

    //________COMPONENTS____________
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

        SetDirectionSettings();

        // CALC DROP TIME
        dropTime = CalcDropTime();
    }

    public void SetDirectionSettings()
    {
        velocity = speed * dirVector;
        acceleration = gravity * dirVector;

        speed = (direction == Direction.negative) ? -speed : speed;
        gravity = (direction == Direction.negative) ? -gravity : gravity;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DROP TIME
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override float CalcDropTime()
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

        DebugDropTime(time, deltaH, paddleHeightHalf);
        if(float.IsNaN(time)){Debug.LogError("Drop time is NaN.");}
        return time;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void MoveActions()
    {
        if(catchesLeft > 0)
        {
            // UPDATE VELOCITY
            Vector2 velocityStep = acceleration * Time.deltaTime;

            velocity += velocityStep;

            // UPDATE POSITION

            Vector3 newPos = new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0.0f);

            rb.MovePosition(transform.position + newPos);
        }
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
        }
    }

    public override void CatchActions()
    {
        velocity.y = -velocity.y;
        DebugCatchTime();
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
        yield return new WaitForSeconds(3.0f);
        exit = true;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DEBUG
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void DebugDropTime(float time, float deltaY, float paddleHeightHalf)
    {
        print("Expected Ball Drop Time: " + time + " sec.");
        print("DISTANCE TO FALL (PaddleY - BallDropY + BallRadius + paddleHeightHalf): " + deltaY);
        print("PADDLE HEIGHT: " + paddleHeightHalf);
        print("BALL RADIUS: " + radius);
    }

    private void DebugCatchTime()
    {
        //print("CatchTime: " + catchTime);
        //print("SpawnTime: " + spawnTime);
        print("Time to catch: " + (catchTime - spawnTime));
    }

    void OnDrawGizmos() 
    {
        if(spawnLoc != null){
            Gizmos.color = Color.green;
            Vector2 targetLoc = new Vector2(spawnLoc.x, spawnLoc.y + deltaH);
            Gizmos.DrawLine(spawnLoc, targetLoc);
        }
    }
}
