using UnityEngine;

public class CatchState : BallState
{
    public CatchState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.NextNote();
        ball.OnCatchActions();

        if(ball.currentNote < ball.numNotes)
        {
            ball.ResetMove();      
            ball.SetState(new MoveState(ball));
            ball.caught = false;
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