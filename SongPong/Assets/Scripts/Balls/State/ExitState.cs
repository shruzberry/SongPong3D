using UnityEngine;

public class ExitState : BallState
{
    public ExitState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.OnExitActions();
    }

    public override void Tick()
    {
        ball.ExitActions();
        if(ball.exit)
        {
            ball.SetState(new IdleState(ball));
        }
    }

    public override void OnStateExit()
    {
    }

}