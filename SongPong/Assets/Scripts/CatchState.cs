using UnityEngine;

public class CatchState : BallState
{
    public CatchState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.catchesLeft--;

        ball.CatchActions();

        ball.caught = false;

        if(ball.catchesLeft > 0)
        {
            ball.SetState(new MoveState(ball));
        }
        else
        {
            ball.SetState(new ExitState(ball));
        }
    }

    public override void OnStateExit()
    {   
    }

}