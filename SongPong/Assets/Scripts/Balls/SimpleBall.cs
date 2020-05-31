using System.Collections;
using UnityEngine;
using Types;

public class SimpleBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;

    //________MOVEMENT______________
    protected Vector2 velocity;
    protected Vector2 acceleration;
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

        acceleration = gravity * axisVector;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ERROR CHECKING
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    /**
     * Make sure that this ball fits the requirements for a simple ball
     **/
    protected override bool CheckForInvalid()
    {
        bool error = false;

        if(numNotes > 1) error = true;
        if(float.IsNaN(moveTime)) error = true;

        return error;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE TIME
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override float CalcMoveTime()
    {
        return CalcTimeToFall(spawnLoc, paddleManager.GetPaddleLocation(Paddles.P1));
    }

    /**
     * Calculates the time it would take this ball to fall between pointA and pointB
     **/
    public float CalcTimeToFall(Vector2 pointA, Vector2 pointB)
    {
        // Calculate delta H
        // |(AB • axisVector) +- ballradius|
        float delta = Mathf.Abs(Vector2.Dot(pointA - pointB, axisVector) - (negative * radius));

        // Calculate delta T using physics equation dy = v0t + 1/2at^2 solved for time in the form
        // t = (-v0 +- sqrt(v0^2 + 2*a*dy)) / a
        float determinant = (Mathf.Pow(speed, 2) + (2 * gravity * delta));
        float time = (-speed + Mathf.Sqrt(determinant)) / gravity;

        if(float.IsNaN(time)) Debug.LogError("TIME IS NAN.");

        return time;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void MoveActions()
    {
        // UPDATE VELOCITY
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
        if(other.gameObject.tag == "Paddle"){
            paddle = other.gameObject.GetComponent<Paddle>();
            
            caught = true;
            catchTimes[currentNote] = song.GetSongTime();
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
        yield return new WaitForSeconds(3.0f);
        exit = true;
    }
}
