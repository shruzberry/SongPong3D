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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BallDropper : MonoBehaviour
{   
    //___________References______________
    private SongController song;
    private AxisManager axisManager;
    private SpawnInfo spawner;
    private Paddle paddle;

    //___________Events__________________
    public delegate void OnBallSpawned(Ball ball);
    public event OnBallSpawned onBallSpawned;
    
    //___________Balls___________________
    private List<Ball> balls = new List<Ball>(); // every ball in this song
    private List<Ball> waitingBallList = new List<Ball>(); // all balls waiting to drop
    private List<Ball> activeBallList = new List<Ball>(); // all balls that have been activated, and thus update
    private List<Ball> finishedBallList = new List<Ball>(); // all balls that have exited-
    private int ballID = 0;

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
        axisManager = FindObjectOfType<AxisManager>();
        spawner = FindObjectOfType<SpawnInfo>();
        paddle = FindObjectOfType<Paddle>();
    }

    void Start()
    {
        LoadBalls();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Update()
    {
        CheckDrop();

        UpdateActiveBalls();

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

            if(ball.checkIfFinished()) { 
				finishedBallList.Add(ball);
			}
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DROP
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void CheckDrop() 
    {
        if(waitingBallList.Count != 0)
        {
            List<Ball> droppedBalls = new List<Ball>();

            // check each ball that hasn't been dropped yet to see if it should be dropped.
            // this method is very brute force and could be improved by sorting the balls first
            foreach(Ball ball in waitingBallList)
            {
                if(!isFinished && ball.NextHitTime() - ball.moveTime < Time.time)
                {
                    droppedBalls.Add(ball);
                    activeBallList.Add(ball);
                }
            }

            // remove any balls that dropped from the waiting pool
            // (this action must occur outside of the foreach loop)
            foreach(Ball ball in droppedBalls)
            {
                waitingBallList.Remove(ball);
            }
        }
	}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SPAWN
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void spawnBall(BallData data)
    {
        try{
            Ball ball = Instantiate(data.prefab).GetComponent<Ball>();
            ball.transform.parent = transform; // set BallDropper gameobject to parent
            // Initialize the ball with id and notes
            data.id = ballID++;

            ball.InitializeBall(data, axisManager, spawner, paddle);
            
            // SUBSCRIBE ACTIONS TO THIS BALL
            // This lets anyone who is subscribed to the onBallSpawned event subscribe to the ball's events
            if(onBallSpawned != null) onBallSpawned(ball);
            // Add to list of balls
            balls.Add(ball);
        } 
        catch(Exception e)
        {
            ballID--;
        }
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

    public List<Ball> getActiveBalls(){return activeBallList;}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * LOADER
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void LoadBalls()
    {
        string path = dataLocation + ballMapName + "/Balls/";
        BallData[] ballData = Resources.LoadAll<BallData>(path);

        if(balls != null)
        {
            foreach(BallData data in ballData)
            {
                if(data.enabled)
                {
                    spawnBall(data);
                }
            }
        }
        waitingBallList = balls;
    }
}
