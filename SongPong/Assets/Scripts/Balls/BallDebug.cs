using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDebug : MonoBehaviour
{
    public bool printDebug;

    BallDropper ballDropper;
    SongController song;
    // Start is called before the first frame update
    void Awake()
    {
        ballDropper = FindObjectOfType<BallDropper>();
        song = FindObjectOfType<SongController>();

        ballDropper.onBallSpawned += AttachBallListener;
    }

    public void AttachBallListener(Ball ball)
    {
        ball.onBallCaught += DebugCatch;
    }

    public void DebugCatch(Ball ball, Paddle paddle)
    {
        if(!printDebug){return;}

        int currentNote = ball.currentNote;
        float catchTime = 0;

        // for the first drop, use the spawn time in deltaT
        if(currentNote == 0)
        {
            catchTime = ball.catchTimes[0] - ball.spawnTime;
        }
        // for bounces, etc. use the last catch time
        else
        {
            catchTime = ball.catchTimes[currentNote] - ball.catchTimes[currentNote - 1];
        }
        Debug.Log("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
        Debug.Log("Expected Move Time (Beats): " + ballDropper.fallTime);
        Debug.Log("Actual Move Time (Beats): " + song.ToBeat(catchTime));
        Debug.Log("Caught " + ball.type + "Ball " + ball.id + " at beat " + song.currentBeat);
        Debug.Log("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
    }

    public void ToggleDebug()
    {
        print("swapping");
        if (printDebug)
            printDebug = false;
        else
            printDebug = true;
    }
}
