using UnityEngine;

public class IdleState : BallState
{
    public IdleState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.GoToSpawnLoc();
        ball.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void Tick()
    {
        Debug.Log("IDLE");

        ball.ready = true; // in our case, immediately make the ball ready

        if(ball.ready)
        {
            ball.SetState(new MoveState(ball));
            ball.spawnTime = Time.time;
        }
    }

    public override void OnStateExit()
    {
        ball.GetComponent<SpriteRenderer>().enabled = true;
    }

}