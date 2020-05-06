using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBall : Ball
{
    //________COMPONENTS____________
    protected Rigidbody2D rb;

    //________ATTRIBUTES____________
    protected float radius;
    protected float acceleration;
    protected float startSpeed = -1;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void InitializeBall(Vector3 pos)
    {
        transform.position = pos;
        velocity = new Vector2(0, startSpeed);
        acceleration = -3;
        size = GetComponent<Collider2D>().bounds.size.y;
        radius = size / 2;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * IDLE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void HandleIdle()
    {

    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ACTIVATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void HandleActivate()
    {
        spawnTime = Time.time;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void HandleMove()
    {
        velocity.y += acceleration * Time.fixedDeltaTime;

        print("DT: " + Time.fixedDeltaTime);
        Vector3 newPos = new Vector3(velocity.x * Time.fixedDeltaTime, velocity.y * Time.fixedDeltaTime, 0.0f);
        rb.MovePosition(transform.position + newPos);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MISS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected override void CheckMiss()
    {
        if(transform.position.y < -screenBounds.y && !missed){
            ChangeState(State.Missed);
            missed = true;
        }
    }

    protected override void HandleMiss()
    {
//print("MISS");
        // DO POINT STUFF HERE
    }

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
        }
        caught = false;
    }

    protected override void HandleCatch()
    {
        timesCaught++;

        velocity.y = -velocity.y;
        catchTime = Time.time;

print("Catch #" + timesCaught);
print("Time to catch: " + (catchTime - spawnTime));

        if(timesCaught < 1)
        {
            ChangeState(State.Moving);
        }
        else
        {
            ChangeState(State.Exit);
        }
    }
}
