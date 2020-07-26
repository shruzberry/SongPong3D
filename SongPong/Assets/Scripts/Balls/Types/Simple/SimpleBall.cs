﻿using System.Collections;
using UnityEngine;
using Types;

[RequireComponent(typeof(CircleCollider2D))]
public class SimpleBall : Ball
{
    //________ATTRIBUTES____________
    protected float radius;

    //________MOVEMENT______________
    protected Vector2 velocity;
    private float speed;
    private float gravity;

    //________COMPONENTS____________
    Vector3 screenBounds;
    public Rigidbody2D rb;
    public Animator animator;

    //________MOVEMENT______________
    private float deltaH;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void InitializeBall(Game game, BallData data, BallDropper dropper)
    {
        base.InitializeBall(game, data, dropper);

        // MOTION
        speed = dropper.startSpeed;
        gravity = dropper.gravity;
        velocity = speed * axisDirVector;

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
        Vector2 velocityStep = axisDirVector * (gravity * Time.deltaTime);

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
        float positionOnAxis = Vector2.Dot(transform.position, axisVector);
        float maxValueOnAxis = Vector2.Dot(screenBounds, axisVector);
        if(positionOnAxis < -maxValueOnAxis)
        {
            missed = true;
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
            //paddle = other.gameObject.GetComponent<Paddle>();
            caught = true;
            catchTimesBeats[currentNote] = song.GetSongTimeBeats();
        }
    }

    public override void OnCatchActions()
    {
        base.OnCatchActions();
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
