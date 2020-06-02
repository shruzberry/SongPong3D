using UnityEngine;

public class IdleState : BallState
{
    SongController song;

    public IdleState(Ball ball) : base(ball)
    {
        song = ball.song;
    }

    public override void OnStateEnter()
    {
        ball.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void Tick()
    {
        ball.ready = true; // in our case, immediately make the ball ready

        if(ball.ready)
        {
            ball.ReadyActions();
            ball.SetState(new MoveState(ball));
        }
    }

    public override void OnStateExit()
    {
        ball.GetComponent<SpriteRenderer>().enabled = true;

        ball.spawnTime = song.GetSongTime();
    }

}