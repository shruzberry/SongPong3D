using UnityEngine;

public class CatchState : BallState
{
    public CatchState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.CatchActions();

        ball.currentNote++;

        if(ball.currentNote < ball.numNotes)
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
        ball.caught = false;
    }

}