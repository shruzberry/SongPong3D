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

    public void DebugCatch(Ball ball)
    {
        if(!printDebug){return;}

        int currentNote = ball.currentNote;
        float catchTimeBeats = 0;

        // for the first drop, use the spawn time in deltaT
        if(currentNote == 0)
        {
            catchTimeBeats = ball.catchTimesBeats[0] - ball.spawnTimeBeats;
        }
        // for bounces, etc. use the last catch time
        else
        {
            Debug.Log("FIX ME");
            catchTimeBeats = ball.catchTimesBeats[currentNote] - ball.catchTimesBeats[currentNote - 1];
        }
        Debug.Log("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
        Debug.Log("Expected Move Time (Beats) – " + ballDropper.GetFallTimeBeats());
        Debug.Log("Actual Move Time (Beats) – " + catchTimeBeats);
        Debug.Log("Caught " + ball.name + " at beat " + song.GetSongTimeBeats());
        Debug.Log("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
    }
}
