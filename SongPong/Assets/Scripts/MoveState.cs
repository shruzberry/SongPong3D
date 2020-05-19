using UnityEngine;

public class MoveState : BallState
{
    public MoveState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        
    }

    public override void Tick()
    {
        if(ball.caught)
        {
            ball.SetState(new CatchState(ball));
        }

        if(ball.CheckMiss())
        {
            ball.SetState(new MissState(ball));
        }
    }

    public override void FixedTick()
    {
        ball.MoveActions();
    }

    public override void OnStateExit()
    {
        
    }

}