using UnityEngine;

public class CatchState : BallState
{
    public CatchState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.CatchActions();

        if(ball.currentNote < ball.numNotes)
        {
            ball.moveTimes = ball.CalcMoveTime();        
            ball.SetState(new MoveState(ball));
        }
        else
        {
            ball.SetState(new ExitState(ball));
        }
    }

    public override void OnStateExit()
    {   
        ball.caught = false;
    }

}