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
    //___________Balls___________________
    private BallData[] ballData; // stores ball data

    private List<Ball> balls = new List<Ball>();
    private List<Ball> activeBallList = new List<Ball>(); // all balls that have been activated, and thus update
    private List<Ball> finishedBallList = new List<Ball>(); // all balls that have exited
    private int currentBallIndex; // which ball to drop
    private Ball currentBall;

    //___________Loader__________________
    public string dataLocation = "SongData/data/";
    public string ballMapName; // name of the current song

    //___________State___________________
    private bool isFinished = false; // any balls left to update

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Start()
    {
        loadBalls();
        currentBallIndex = 0;
        currentBall = balls[currentBallIndex];
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
        float dropTime = currentBall.dropTime; // this should probably just be set once
        if(!isFinished && currentBall.getHitTime() - dropTime < Time.time)
        {
            currentBall.TriggerActivation();
            activeBallList.Add(currentBall);
            nextBall();
        }
	}

    public void nextBall() 
    {
        if(currentBallIndex < balls.Count - 1){
            currentBallIndex++;
        }
        else{
            isFinished = true;
            print("Last ball reached.");
        }
        currentBall = balls[currentBallIndex];
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SPAWN
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void spawnBall(BallData data, NoteData[] notes)
    {
        if(notes.Length == 0)
        {
            Debug.LogError("No notes have been assigned to this ball.");
            return;
        }

        Ball ball = Instantiate(data.prefab).GetComponent<Ball>();
        ball.transform.parent = transform; // set BallDropper gameobject to parent
        ball.ballData = data;

        // Initialize the ball with id and notes
        ball.InitializeBall(data.id, notes);

        // Add to list of balls
        balls.Add(ball);
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

    public void loadBalls()
    {
        string path = dataLocation + ballMapName + "/Balls/";
        ballData = Resources.LoadAll<BallData>(path);

        if(balls != null)
        {
            foreach(BallData data in ballData)
            {
                NoteData[] notes = data.notes;
                
                spawnBall(data, notes);
            }
        }
    }
}
