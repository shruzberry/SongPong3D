using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDebug : MonoBehaviour
{
    public bool printCatchDebug;
    public bool detailed;

    public bool printSpawnDebug;

    private BallDropper ballDropper;
    private SongController song;
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

        if(!printSpawnDebug) return;
        Debug.Log("Spawned " + ball.name + " at beat " + song.GetSongTimeBeats());
    }

    public void DebugCatch(Ball ball)
    {
        if(!printCatchDebug){return;}

        int currentNote = ball.currentNote;
        float catchTimeBeats = 0;

        // for the first drop, use the spawn time in deltaT
        if(currentNote == 0)
        {
            //Debug.Log("SPAWN TIME: " + ball.spawnTimeBeats);
            //Debug.Log("CATCH TIME: " + ball.catchTimesBeats[currentNote]);
            catchTimeBeats = ball.catchTimesBeats[currentNote] - ball.spawnTimeBeats;
            Debug.Log(ball.catchTimesBeats[currentNote]);
            Debug.Log(ball.spawnTimeBeats);
        }
        // for bounces, etc. use the last catch time
        else
        {
            //Debug.Log("CATCH TIME 1: " + ball.catchTimesBeats[currentNote - 1]);
            //Debug.Log("CATCH TIME 2: " + ball.catchTimesBeats[currentNote]);
            catchTimeBeats = ball.catchTimesBeats[currentNote] - ball.catchTimesBeats[currentNote - 1];
        }
        Debug.Log("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
        if(detailed)
        {
            Debug.Log("Expected Move Time (Beats): " + ballDropper.GetFallTimeBeats());
            Debug.Log("Actual Move Time (Beats): " + catchTimeBeats);
        }
        Debug.Log("Caught " + ball.name + " at beat " + ball.catchTimesBeats[currentNote]);
        Debug.Log("+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=");
    }

    public void ToggleDebug()
    {
        printCatchDebug = (printCatchDebug) ? !printCatchDebug : printCatchDebug;
    }
}
