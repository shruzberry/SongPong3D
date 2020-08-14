/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: BallDropper
Purpose: Takes a list of balls spawns and drops them at the correct time
Associations: Song
________ USAGE ________
* Description of how to appropriatly use
________ ATTRIBUTES ________
+ public: brief description of usage
* protected: brief description of usage
- private: brief description of usage
________ FUNCTIONS ________
+ publicFunction(): description of usage
- privateFunction(): description of usage
 +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections.Generic;
using UnityEngine;
using System;

public class BallDropper : MonoBehaviour
{
    //___________References______________
    private Game game;
    private Track track;
    private SongController song;

    //___________Settings________________
    [Header("Settings")]
    public bool dropEnabled = true; // should balls drop?
    public bool debugBalls = false;
    private float lagAmount = 0.07f; // arbitrary "lag" amount apparent in all ball behaviors, meant to be 0.0f

    //___________Loader__________________
    [Header("Loader")]
    public string dataLocation = "SongData/data/";
    public string ballMapName; // name of the current song

    //___________SPAWN___________________
    [HideInInspector]
    public float ballDropHeight;
    [HideInInspector]
    public Vector3 spawnLoc;

    //___________Balls___________________
    private List<BallData> ballDataList = new List<BallData>(); // all ball data in this scene
        private int numBalls; // total number of balls in ballDataList

    private List<Ball> activeBallList = new List<Ball>(); // all balls that have been activated, and thus update

    //__________Ball Attributes__________
    [Header("Ball Settings")]
    public float startSpeed;
    public float gravity;
    public float bounceHeightBase;
    [HideInInspector]
    public Vector3 ballAxis; // the axis that balls move down primarily
    [HideInInspector]
    public float size;

    //__________Ball Types_______________
    private GameObject simpleBall;
    private GameObject bounceBall;

    //___________Move Behaviors__________
    private float fallTimeBeats;
    public Vector2 fallAxisBounds;

    //___________Events__________________
    public delegate void OnBallSpawned(Ball ball);
    public event OnBallSpawned onBallSpawned;

    //___________State___________________
    [Header("State")]
    public int currentBallIndex; // pointer that keeps track of where we are in ballDataList
    private bool isFinished = false; // any balls left to update


/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Initialize(Game game, SongController song)
    {
        // REFERENCES
        this.game = game;
        this.song = song;
        track = FindObjectOfType<Track>();

        // PREFABS
        simpleBall = Resources.Load("Prefabs/SimpleBall") as GameObject;
        bounceBall = Resources.Load("Prefabs/BounceBall") as GameObject;

        // EVENTS
        song.onSongFastForward += FastForwardBalls;
        song.onSongRewind += RewindBalls;

        // POINTERS
        currentBallIndex = 0;

        // ATTRIBUTES
        ballAxis = Vector3.forward;
        size = track.columnWidth;
        fallAxisBounds = track.GetBoundsFallAxis();

        SetSpawnLocation();

        // LAG ADJUSTMENT
        if(lagAmount != 0.0f)
        {
            Debug.LogWarning("BallDropper initialized with " + lagAmount + "sec. of lag");
        }
    }

    private void CalcMoveTimes()
    {
        float paddleLoc = FindObjectOfType<PaddleMover>().GetPaddleTopLoc();

        float fallTime = Fall_Behavior.CalcMoveTime(size/2, spawnLoc.z, paddleLoc, startSpeed, gravity);
        Debug.Log("Falltime: " + fallTime + "s. Beats: " + song.ToBeat(fallTime));
        fallTimeBeats = song.ToBeat(fallTime);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Update()
    {
        if(dropEnabled && !song.hasStarted)
        {
            CheckPreSpawn();
        }
        if(dropEnabled && song.hasStarted)
        {
            CheckSpawn();
        }
        UpdateActiveBalls();
        CheckActiveBallsForDestroy();
    }

    private void UpdateActiveBalls()
    {
        // Update all active balls
        foreach(Ball ball in activeBallList)
        {
            ball.UpdateBall();
        }
    }

    private void FixedUpdate()
    {
        foreach(Ball ball in activeBallList){
            ball.FixedUpdateBall();
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * FAST-FORWARD / REWIND
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void FastForwardBalls()
    {
        ClearActiveBalls();

        BallData checkBall = ballDataList[currentBallIndex];
        float newSongTime = song.GetSongTimeBeats();
        while(!isFinished && checkBall.notes[0].hitBeat - fallTimeBeats < newSongTime)
        {
            NextBall();
            checkBall = ballDataList[currentBallIndex];
        }
    }

    public void RewindBalls()
    {
        ClearActiveBalls();

        BallData checkBall = ballDataList[currentBallIndex];
        float newSongTime = song.GetSongTimeBeats();
        while(checkBall.notes[0].hitBeat - fallTimeBeats > newSongTime)
        {
            PreviousBall();
            if(currentBallIndex == 0) break;
            checkBall = ballDataList[currentBallIndex];
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SPAWN
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Spawn balls before the song starts
     **/
    public void CheckPreSpawn()
    {
        bool spawning = false;
        BallData checkBall = ballDataList[currentBallIndex];
        float dropBeat = checkBall.notes[0].hitBeat - fallTimeBeats;

        if(!isFinished && dropBeat <= song.ToBeat(game.GetGameTime()) - fallTimeBeats)
        {
            spawning = true;
            // Check other balls for spawn 
            // Because we want balls with same hit time to spawn in same Update loop so their motions are synced
            while(spawning)
            {
                Ball ball = SpawnBall(checkBall);
                checkBall.PulseActive();

                // if there are no balls left, exit the loop
                if(isFinished){ spawning = false; break;}
                else
                {
                    checkBall = ballDataList[currentBallIndex];
                    dropBeat = checkBall.notes[0].hitBeat - fallTimeBeats;
                    // If the next ball isn't ready to drop, continue
                    if(!(dropBeat <= song.ToBeat(Time.time - fallTimeBeats) + lagAmount)){ spawning = false; }
                }
            }
        }
    }
    /**
     * Checks the next ball in the ballDataList to see if it is ready to drop
     * If it is, it spawns the ball and any others ready to spawn
     * 
     * NOTE: This function assumes the ballDataList is PRE-SORTED
     **/
    public void CheckSpawn()
    {
        bool spawning = false;
        BallData checkBall = ballDataList[currentBallIndex];
        float dropBeat = checkBall.notes[0].hitBeat - fallTimeBeats;

        // If ball has negative time (usually if hitBeat = 0) 
        if(dropBeat < 0) 
        {
            Debug.LogWarning("Ball " + checkBall.name + " has invalid hitBeat.");
            NextBall();
        }

        // Check first ball for spawn
        if(!isFinished && dropBeat <= song.GetSongTimeBeats() + lagAmount)
        {
            spawning = true;
            // Check other balls for spawn 
            // Because we want balls with same hit time to spawn in same Update loop so their motions are synced
            while(spawning)
            {
                Ball ball = SpawnBall(checkBall);
                checkBall.PulseActive();

                // if there are no balls left, exit the loop
                if(isFinished){ spawning = false; break;}
                else
                {
                    checkBall = ballDataList[currentBallIndex];
                    dropBeat = checkBall.notes[0].hitBeat - fallTimeBeats;
                    // If the next ball isn't ready to drop, continue
                    if(!(dropBeat <= song.GetSongTimeBeats() + lagAmount)){ spawning = false; }
                }
            }
        }
    }

    public Ball SpawnBall(BallData data)
    {
        if(!data.enabled) return null; // if the ball is disabled, don't spawn it

        try{
            Ball ball = Instantiate(data.prefab).GetComponent<Ball>();
            ball.transform.parent = transform; // set BallDropper object to parent

            ball.InitializeBall(game, data, this);

            // This lets anyone who is subscribed to the onBallSpawned event subscribe to the ball's events
            if(onBallSpawned != null) onBallSpawned(ball);

            // Add to list of balls
            activeBallList.Add(ball);

            NextBall();

            return ball;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /**
     * Updates the currentBallIndex to the next location, if possible
     **/
    private void NextBall()
    {
        if(currentBallIndex < numBalls - 1){ currentBallIndex++; }
        else { isFinished = true; }
    }

    private void PreviousBall()
    {
        if(currentBallIndex > 0) { currentBallIndex--; isFinished = false;}
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DESTROY
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void CheckActiveBallsForDestroy()
    {
        List<Ball> destroyBalls = new List<Ball>();

        foreach(Ball ball in activeBallList)
        {
            if(ball.exit)
            {
				destroyBalls.Add(ball);
			}
        }
        foreach(Ball rmBall in destroyBalls)
        {
            activeBallList.Remove(rmBall);
            rmBall.DestroyBall();
        }
    }

    private void ClearActiveBalls()
    {
        foreach(Ball ball in activeBallList)
        {
            ball.DestroyBall();
        }
        activeBallList.Clear();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * PUBLIC ACCESSORS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Activate()
    {
        LoadBalls();
        if(debugBalls) DebugLoadedBalls();
        // balls not instantiated yet
        CalcMoveTimes();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * LOADER
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Creates a list of BallData from the resources folder for the active song
     **/
    public void LoadBalls()
    {
        // check if was given a name
        if(String.IsNullOrEmpty(ballMapName)) Debug.LogError("Ball Map Name is null or empty.");

        List<BallData> ballData = game.GetBallData();

        if(ballData != null)
        {
            foreach(BallData ball in ballData)
            {
                if(ball.CheckValid())
                    ballDataList.Add(ball);
                else
                    Debug.LogWarning(ball.name + " won't spawn because it has invalid data.");
            }
        }

        numBalls = ballDataList.Count;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DEBUG
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    
    public void DebugLoadedBalls()
    {
        if(ballDataList.Count == 0)
        {
            Debug.LogWarning("NO BALLS WERE LOADED.");
        }
        foreach(BallData ballData in ballDataList)
        {
            Debug.Log(ballData.name);
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void SetSpawnLocation()
    {
        ballDropHeight = track.GetTop() - 30;
        spawnLoc = new Vector3(0, track.gameYValue, ballDropHeight);
    }

    public Vector3 GetSpawnLocation(int spawnNum)
    {
        return new Vector3(track.ballCols[spawnNum], track.gameYValue, ballDropHeight); 
    }

    public BallData[] GetAllBallData()
    {
        string path = dataLocation + ballMapName + "/Balls/";
        BallData[] ballData = Resources.LoadAll<BallData>(path);
        return ballData;
    }
    public List<Ball> GetActiveBalls(){return activeBallList;}
    public float GetFallTimeBeats(){return fallTimeBeats;}

    public float GetTimeToFallBeats()
    {
        return fallTimeBeats;
    }

}
