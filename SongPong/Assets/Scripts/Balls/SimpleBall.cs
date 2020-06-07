using System.Collections;
using UnityEngine;
using Types;

public class SimpleBall : Ball
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
    public Animator animator;

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
        animator = GetComponent<Animator>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
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

        return error;
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
        if(!ball_renderer.isVisible) missed = true;
        return missed;
    }

    public override void MissActions(){}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

   private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Paddle"){            
            caught = true;
            catchTimes[currentNote] = song.GetSongTime();
        }
    }

    public override void CatchActions()
    {
        base.CatchActions();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void OnExitActions()
    {
        animator.SetTrigger("isFinished");
        velocity = -velocity;
    }

    public override void ExitActions()
    {
        MoveActions();
    }

    public void OnAnimationFinish()
    {
        exit = true;
    }

    IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(3.0f);
        exit = true;
    }
}
