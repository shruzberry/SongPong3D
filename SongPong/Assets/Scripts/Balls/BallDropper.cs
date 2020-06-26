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
    private SongController song;
    private SpawnInfo spawner;

    //___________Events__________________
    public delegate void OnBallSpawned(Ball ball);
    public event OnBallSpawned onBallSpawned;

    //___________Balls___________________
    private List<BallData> ballDataList = new List<BallData>(); // all ball data in this scene
        private int numBalls; // total number of balls in ballDataList
        private int currentBallIndex; // pointer that keeps track of where we are in ballDataList

    private List<Ball> activeBallList = new List<Ball>(); // all balls that have been activated, and thus update

    //__________Ball Types_______________
    private GameObject simpleBall;
    private GameObject bounceBall;

    //___________Move Behaviors__________
    private float fallTimeBeats;

    //___________Loader__________________
    public static string dataLocation = "SongData/data/";
    public string ballMapName; // name of the current song

    //___________State___________________
    private bool isFinished = false; // any balls left to update

    //___________Debug___________________
    public bool printLoadedBalls = false;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Awake()
    {
        game = FindObjectOfType<Game>();
        song = FindObjectOfType<SongController>();
        spawner = FindObjectOfType<SpawnInfo>();

        simpleBall = Resources.Load("Prefabs/SimpleBall") as GameObject;
        bounceBall = Resources.Load("Prefabs/BounceBall") as GameObject;

        // EVENTS
        song.onSongFastForward += FastForwardBalls;
        song.onSongRewind += RewindBalls;

        // POINTERS
        currentBallIndex = 0;
    }

    private void CalcMoveTimes()
    {
        Vector2 spawnAxis = spawner.spawnAxis;

        Vector2 paddleAxis = FindObjectOfType<PaddleMover>().GetPaddleTopAxis();

        Vector2 axisVector;
        if(game.gameAxis == Axis.y) {axisVector = new Vector2(0,1);}
        else{axisVector = new Vector2(1,0);}

        float fallTime = Fall_Behavior.CalcMoveTime(simpleBall, spawnAxis, paddleAxis, axisVector, 0.0f, 3.0f);
        fallTimeBeats = song.ToBeat(fallTime);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Update()
    {
        UpdateActiveBalls();

        CheckSpawn();

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
        if(!isFinished && dropBeat <= song.GetSongTimeBeats())
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
                    if(!(dropBeat <= song.GetSongTimeBeats())){ spawning = false; }
                }
            }
        }
    }

    public Ball SpawnBall(BallData data)
    {
        if(!data.enabled) return null; // if the ball is disabled, don't spawn it

        try{
            Ball ball = Instantiate(data.prefab).GetComponent<Ball>();
            ball.transform.parent = transform; // set BallDropper gameobject to parent

            ball.InitializeBall(data, game.gameAxis, spawner, song);

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
        if(printLoadedBalls) DebugLoadedBalls();
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
        //ballList.Sort(BallData.CompareBallsBySpawnTime);

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

    public BallData[] GetAllBallData()
    {
        string path = dataLocation + ballMapName + "/Balls/";
        BallData[] ballData = Resources.LoadAll<BallData>(path);
        return ballData;
    }
    public List<Ball> GetActiveBalls(){return activeBallList;}
    public float GetFallTimeBeats(){return fallTimeBeats;}

}
