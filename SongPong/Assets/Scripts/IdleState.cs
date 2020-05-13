using UnityEngine;

public class IdleState : BallState
{
    public override void OnStateEnter()
    {

    }

    public override void Tick()
    {
        Debug.Log("IDLE");
    }

    public override void OnStateExit()
    {
        
    }

}