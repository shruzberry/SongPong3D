using UnityEngine;

public class IdleState : BallState
{
    public IdleState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void Tick()
    {
        ball.ready = true; // in our case, immediately make the ball ready

        if(ball.ready)
        {
            ball.ReadyActions();
            ball.SetState(new MoveState(ball));
        }
    }

    public override void OnStateExit()
    {
        ball.GetComponent<SpriteRenderer>().enabled = true;

        //one fixed time step is added because the first move hasn't been called yet
        ball.spawnTime = Time.time + Time.fixedDeltaTime;
    }

}