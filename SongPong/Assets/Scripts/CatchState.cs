using UnityEngine;

public class CatchState : BallState
{
    public CatchState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.CatchActions();
    }

    public override void Tick()
    {
        Debug.Log("CATCH");
        ball.catchesLeft--;

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