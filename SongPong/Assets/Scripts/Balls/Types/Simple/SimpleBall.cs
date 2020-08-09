using System.Collections;
using UnityEngine;
using Types;

public class SimpleBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;

    //________MOVEMENT______________
    [Header("Movement")]
    private Vector2 fallAxisBounds;
    protected Vector3 velocity;
    public float speed;
    public float gravity;

    //________COMPONENTS____________
    private Rigidbody rb;
    private Animator animator;

    //________MOVEMENT______________
    private float deltaH;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void InitializeBall(Game game, BallData data, BallDropper dropper)
    {
        base.InitializeBall(game, data, dropper);

        // MOTION
        fallAxisBounds = dropper.fallAxisBounds;
        speed = dropper.startSpeed;
        gravity = dropper.gravity;
        velocity = speed * axisVector * negative;

        // COMPONENTS
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * IDLE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void OnIdleExit()
    {
        base.OnIdleExit();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void MoveActions()
    {
        // UPDATE VELOCITY
        Vector3 velocityStep = axisVector * (gravity * Time.deltaTime) * negative;

        velocity += velocityStep;

        // UPDATE POSITION
        Vector3 newPos = new Vector3(0.0f, 0.0f, velocity.z * Time.deltaTime);

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
            Debug.Log("MISS");
        }
        return missed;
    }

    public override void MissActions(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

   private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Paddle"){
            //paddle = other.gameObject.GetComponent<Paddle>();
            caught = true;
            catchTimesBeats[currentNote] = song.GetSongTimeBeats();
        }
    }

    public override void OnCatchActions()
    {
        base.OnCatchActions();
        velocity = -velocity;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void OnExitActions()
    {
        animator.SetTrigger("isFinished");
    }

    public override void ExitActions()
    {
        MoveActions();
    }

    public void OnAnimationFinish()
    {
        exit = true;
    }
}
