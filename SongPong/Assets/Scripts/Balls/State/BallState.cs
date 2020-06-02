public class BallState
{
    protected Ball ball;
    public BallState(Ball ball)
    {
        this.ball = ball;
    }
    
    public virtual void OnStateEnter(){}

    public virtual void Tick(){}

    public virtual void FixedTick(){}

    public virtual void OnStateExit(){}
}