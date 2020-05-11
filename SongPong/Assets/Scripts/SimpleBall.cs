using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;
    public float dropSpeed = 1.0f;

    //________MOVEMENT______________
    public float acceleration;
    private float deltaH;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void InitializeBallSpecific()
    {
        // ATTRIBUTES
        size = GetComponent<Collider2D>().bounds.size.y;
        radius = size / 2;

        if(ballAxis == Axis.y){InitializeBallYAxis();}
        else if(ballAxis == Axis.x){InitializeBallXAxis();}
    }

    private void InitializeBallYAxis()
    {
        velocity = new Vector2(0, dropSpeed);
        acceleration = -3;
    }

    private void InitializeBallXAxis()
    {
        velocity = new Vector2(dropSpeed, 0);
        acceleration = -3;
    }

    public override float GetSpawnTimeOffset()
    {
        // Get Paddle Info
        float paddleAxis = paddle.getPaddleAxis();
        float paddleHeightHalf = paddle.getPaddleHeight() / 2;
        
        // Determine Delta H (Height)
        if(ballAxis == Axis.y)
        {
            deltaH = paddleAxis - spawnLoc.y + radius + paddleHeightHalf;
        }
        else if(ballAxis == Axis.x)
        {
            deltaH = paddleAxis - spawnLoc.x + radius + paddleHeightHalf;
        }

        // Calculate delta T
        // using physics equation dy = v0t + 1/2at^2 solved for time in the form
        // t = (-v0 +- sqrt(v0^2 + 2ady)) / a
        float determinant = (Mathf.Pow(dropSpeed, 2) + (2 * acceleration * deltaH));
        float time = (-dropSpeed - Mathf.Sqrt(determinant)) / acceleration;

        DebugDropTime(time, deltaH, paddleHeightHalf);

        return time;
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * IDLE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void HandleIdle(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ACTIVATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void HandleActivate(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void HandleMove()
    {
        if(ballAxis == Axis.y)
        {
            velocity.y += acceleration * Time.deltaTime;
        }
        else if(ballAxis == Axis.x)
        {
            velocity.x += acceleration * Time.deltaTime;
        }
        Vector3 newPos = new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0.0f);
        rb.MovePosition(transform.position + newPos);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MISS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void CheckMiss()
    {
        if(ballAxis == Axis.y)
        {
            if(transform.position.y < -screenBounds.y && !missed)
            {
                ChangeState(State.Missed);
                missed = true;
            }
        }
        else if(ballAxis == Axis.x)
        {
            if((transform.position.x < -screenBounds.x || transform.position.x > screenBounds.x) && !missed)
            {
                ChangeState(State.Missed);
                missed = true;
            }
        }
    }

    protected override void HandleMiss(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

   private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Paddle"){
            caught = true;
        }
    }

    protected override void CheckCatch()
    {
        if(caught)
        {
            ChangeState(State.Caught);
            catchTime = Time.time;
            DebugCatchTime();
        }
        caught = false;
    }

    protected override void HandleCatch()
    {
        timesCaught++;

        velocity.y = -velocity.y;

        if(timesCaught < 1)
        {
            ChangeState(State.Moving);
        }
        else
        {
            ChangeState(State.Exit);
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void HandleExit()
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
