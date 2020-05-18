using UnityEngine;

public class ExitState : BallState
{
    public ExitState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.ExitActions();
    }

    public override void Tick()
    {
        Debug.Log("EXIT");

        if(ball.exit)
        {
            ball.SetState(new IdleState(ball));
        }
    }

    public override void OnStateExit()
    {
    }

}