using UnityEngine;

public class SimpleBall : Ball
{
    //_____ SETTINGS ____________________

    //_____ REFERENCES __________________

    //_____ COMPONENTS __________________
    private Rigidbody rb;
    private Animator animator;

    //_____ ATTRIBUTES __________________
    protected float radius;

    //_____ BOOLS _______________________

    //_____ OTHER _______________________

    //_____ MOVEMENT ____________________
    [Header("Movement")]
    private Vector2 fallAxisBounds;
    protected Vector3 velocity;
    public float speed;
    public float gravity;
    private float deltaH;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void Initialize(Game game, BallData data, BallDropper dropper)
    {
        base.Initialize(game, data, dropper);

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
        }
        return missed;
    }

    public override void MissActions(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

   private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            caught = true;
            catchTimesBeats[currentNote] = song.GetSongTimeBeats(); // set catchBeat
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
