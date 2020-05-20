using UnityEngine;

public class MissState : BallState
{
    public MissState(Ball ball) : base(ball)
    {
    }

    public override void OnStateEnter()
    {
        ball.MissActions();
    }

    public override void Tick()
    {
        Debug.Log("MISS");
        ball.SetState(new ExitState(ball));
    }

    public override void OnStateExit()
    {
        
    }

}