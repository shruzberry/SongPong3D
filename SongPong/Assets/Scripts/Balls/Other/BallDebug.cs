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
        Debug.Log("Spawned " + ball.name + " at beat: " + song.GetSongTimeBeats() + " and position: " + ball.transform.position);
    }

    public void DebugCatch(Ball ball)
    {
        if(!printCatchDebug){return;}

        int currentNote = ball.currentNote - 1;
        float catchTimeBeats = 0;

        // for the first drop, use the spawn time in deltaT
        if(currentNote == 0)
        {
            catchTimeBeats = ball.catchTimesBeats[currentNote] - ball.spawnTimeBeats;
        }
        // for bounces, etc. use the last catch time
        else
        {
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
}
