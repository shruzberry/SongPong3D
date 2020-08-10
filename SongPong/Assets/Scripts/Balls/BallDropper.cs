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
    //___________Settings________________
    public float yValue = 2; // the y-value that the balls appear on
    private float lagAmount = 0.07f; // arbitrary "lag" amount apparent in all ball behaviors, meant to be 0.0f
    public bool dropEnabled = true; // should balls drop?

    //___________References______________
    private Game game;
    private Track track;
    private SongController song;

    //___________Spawn___________________
    [Range(1,16)]
    public int NUM_COL = 16; // number of ball columns
    public Vector3 ballAxis; // the axis that balls move down primarily
    public Vector3 spawnLoc;
    public float ballDropHeight;
    [Tooltip("Determines what height the ball spawns are from the top of the screen")]
    private float[] ballCols; // x- or z-positions of each column in world coordinates
    public float ballHeightMod = 0;
    public float columnWidth;

    //___________Balls___________________
    private List<BallData> ballDataList = new List<BallData>(); // all ball data in this scene
        private int numBalls; // total number of balls in ballDataList
        public int currentBallIndex; // pointer that keeps track of where we are in ballDataList

    private List<Ball> activeBallList = new List<Ball>(); // all balls that have been activated, and thus update

    //__________Ball Attributes__________
    public float startSpeed;
    public float size;
    public float gravity;
    public float bounceHeightBase;

    //__________Ball Types_______________
    private GameObject simpleBall;
    private GameObject bounceBall;

    //___________Move Behaviors__________
    private float fallTimeBeats;
    public Vector2 fallAxisBounds;

    //___________Loader__________________
    public static string dataLocation = "SongData/data/";
    public string ballMapName; // name of the current song

    //___________Events__________________
    public delegate void OnBallSpawned(Ball ball);
    public event OnBallSpawned onBallSpawned;

    //___________State___________________
    private bool isFinished = false; // any balls left to update

    //___________Debug___________________
    public bool showColumns = true; // if true, show columns
    public bool printLoadedBalls = false;

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

        // COLUMNS
        SetSpawnLocation();
        calcColumns();

        // EVENTS
        song.onSongFastForward += FastForwardBalls;
        song.onSongRewind += RewindBalls;

        // POINTERS
        currentBallIndex = 0;

        // ATTRIBUTES
        ballAxis = Vector3.forward;
        size = columnWidth;
        fallAxisBounds = track.GetBoundsFallAxis();

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

        if(!isFinished && dropBeat <= song.ToBeat(Time.time) - fallTimeBeats)
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
 * COLUMNS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void SetSpawnLocation()
    {
        ballDropHeight = track.GetTop() - 30;
        spawnLoc = new Vector3(0, yValue, ballDropHeight);
    }

    public Vector3 GetSpawnLocation(int spawnNum)
    {
        return new Vector3(ballCols[spawnNum], yValue, ballDropHeight); 
    }

        private void calcColumns()
    {
        ballCols = new float[NUM_COL+1]; // need n+1 lines to make n column

        float trackLeft = track.transform.position.x - (track.GetWidth() / 2);
        float effScreenW = track.GetWidth() - (2 * track.padding); // the screen width minus the padding

        // STEP for each column
        columnWidth = effScreenW / NUM_COL; // amount of x to move per column

        for(int i = 0; i < NUM_COL+1; i++)
        {
            ballCols[i] = trackLeft + (columnWidth * i + track.padding);
        }
    }

    public int GetNearestColumn(Vector2 screenPos, bool debug = false)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        int nearestColumn = 0;
        float minDist = float.MaxValue;

        float compare;
        if(game.gameType == GameType.OnePlayer){compare = worldPos.x;}
        else if(game.gameType == GameType.TwoPlayer){compare = worldPos.z;}
        else{return 0;}

        for(int f = 0; f < ballCols.Length; f++)
        {
            float delta = Mathf.Abs(ballCols[f] - compare);
            if(delta < minDist)
            {
                minDist = delta;
                nearestColumn = f;
            }
        }
        if(debug){print("NEAREST COLUMN: " + nearestColumn);}

        return nearestColumn;
    }

    private float ConvertToUnits(Camera cam, float p)
    {
        float ortho = cam.orthographicSize;
        float pixelH = cam.pixelHeight;

        return (p * ortho * 2) / pixelH;
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

    public float GetTimeToFallBeats()
    {
        return fallTimeBeats;
    }

}
