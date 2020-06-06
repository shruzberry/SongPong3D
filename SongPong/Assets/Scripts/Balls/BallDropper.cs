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
    private SongController song;
    private AxisManager axisManager;
    private Axis axis;
    private SpawnInfo spawner;
    //private PlayerInputHandler paddleManager;
    private Paddle paddle;
    private float paddleAxis;

    //___________Events__________________
    public delegate void OnBallSpawned(Ball ball);
    public event OnBallSpawned onBallSpawned;

    //___________Balls___________________
    private List<BallData> ballDataList = new List<BallData>(); // all ball data in this scene
    private List<BallData> waitingBallDataList = new List<BallData>(); // all ball data that hasn't been used yet

    private List<Ball> activeBallList = new List<Ball>(); // all balls that have been activated, and thus update
    private List<Ball> finishedBallList = new List<Ball>(); // all balls that have exited-

    //__________Ball Types_______________
    private GameObject simpleBall;
    private GameObject bounceBall;

    //___________Move Behaviors__________
    public float fallTime;

    //___________Loader__________________
    public string dataLocation = "SongData/data/";
    public string ballMapName; // name of the current song

    //___________State___________________
    private bool isFinished = false; // any balls left to update

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Awake()
    {
        song = FindObjectOfType<SongController>();
        //paddleManager = FindObjectOfType<PlayerInputHandler>();
        axisManager = FindObjectOfType<AxisManager>();
        axis = axisManager.gameAxis;
        spawner = FindObjectOfType<SpawnInfo>();
        paddle = FindObjectOfType<Paddle>();

        simpleBall = Resources.Load("Prefabs/SimpleBall") as GameObject;
        bounceBall = Resources.Load("Prefabs/BounceBall") as GameObject;
    }

    private void Start()
    {
        LoadBalls();
        CalcMoveTimes();
    }

    private void CalcMoveTimes()
    {
        Vector2 spawnAxis = spawner.spawnAxis;
        //Vector2 paddleAxis = paddleManager.GetPaddleAxis();
        Vector2 paddleAxis = GameObject.Find("Game").GetComponent<AxisManager>().GetPaddleAxis();
        Vector2 axisVector;
        if(axis == Axis.y) {axisVector = new Vector2(0,1);}
        else{axisVector = new Vector2(1,0);}

        fallTime = Fall_Behavior.CalcMoveTime(simpleBall, spawnAxis, paddleAxis, axisVector, 0.0f, 3.0f);
        fallTime = song.ToBeat(fallTime);
        Debug.Log("FALL TIME: " + fallTime);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Update()
    {
        UpdateActiveBalls();

        CheckSpawn();

        RemoveFinishedBalls();
    }

    private void FixedUpdate()
    {
        foreach(Ball ball in activeBallList){
            ball.FixedUpdateBall();
        }
    }

    private void UpdateActiveBalls()
    {
        // Update all active balls
        foreach(Ball ball in activeBallList){
            ball.UpdateBall();

            if(ball.exit) {
				finishedBallList.Add(ball);
			}
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SPAWN
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void CheckSpawn()
    {
        List<BallData> spawnedBalls = new List<BallData>();

        // check every ballData
        // TODO change this to just look at the first ball, but need to sort first
        foreach(BallData ballData in waitingBallDataList)
        {
            // check every note
            // TODO change this to just look at the first note, but need to sort first
            foreach(NoteData note in ballData.notes)
            {
                float dropBeat = note.hitBeat - fallTime;
                if(!isFinished && dropBeat <= song.currentBeat)
                {
                    spawnedBalls.Add(ballData);

                    Ball ball = SpawnBall(ballData);
                }
            }
        }
        RemoveBallsFromWaiting(spawnedBalls);
        spawnedBalls.Clear();
    }

    private void RemoveBallsFromWaiting(List<BallData> spawnedBalls)
    {
        // remove any balls that dropped from the waiting pool
        // (this action must occur outside of the foreach loop)
        foreach(BallData ball in spawnedBalls)
        {
            waitingBallDataList.Remove(ball);
        }
    }

    public Ball SpawnBall(BallData data)
    {
        if(!data.enabled) return null; // if the ball is disabled, don't spawn it

        try{
            Ball ball = Instantiate(data.prefab).GetComponent<Ball>();
            ball.transform.parent = transform; // set BallDropper gameobject to parent

            ball.InitializeBall(data, axisManager, spawner, /*paddleManager,*/ song);

            // This lets anyone who is subscribed to the onBallSpawned event subscribe to the ball's events
            if(onBallSpawned != null) onBallSpawned(ball);

            // Add to list of balls
            activeBallList.Add(ball);

            Debug.Log("DROP BALL " + ball.name + "at beat " + song.currentBeat);

            return ball;
        }
        catch(Exception e)
        {
            return null;
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * RESET
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void ResetBalls()
    {
        activeBallList = new List<Ball>();
        waitingBallDataList = ballDataList;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * REMOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void RemoveFinishedBalls()
    {
        // Destroy any balls that are caught or missed
		foreach(Ball rmBall in finishedBallList)
        {
			activeBallList.Remove(rmBall);
            rmBall.DeleteBall();
		}
        finishedBallList.Clear();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public List<BallData> getWaitingBalls(){return waitingBallDataList;}

    public List<Ball> getActiveBalls(){return activeBallList;}
    public List<Ball> getFinishedBalls(){return finishedBallList;}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * LOADER
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void LoadBalls()
    {
        string path = dataLocation + ballMapName + "/Balls/";
        BallData[] ballData = Resources.LoadAll<BallData>(path);

        if(ballData != null)
        {
            foreach(BallData data in ballData)
            {
                ballDataList.Add(data);
            }
        }
        waitingBallDataList = ballDataList;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * PUBLIC ACCESSORS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Activate(){LoadBalls();}

}
