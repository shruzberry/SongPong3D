/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: BallDropper
Purpose: Spawn and Drop balls at the correct time
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
    public Song song;
    private Paddle paddle;
    public GameObject simpleBall;
    public GameObject bounceBall;
    
    // Balls
    private List<Ball> balls = new List<Ball>(); // all balls in this song
    private List<Ball> activeBallList = new List<Ball>(); // all active balls
    private List<Ball> finishedBallList = new List<Ball>();
    private int currentBallIndex;
    private Ball currentBall;

    // Reader
    public string ballsLocation = "Assets/Resources/Note Data/";
    public string ballMapName;
    public char delimeter = ',';
    public char noteDelimeter = '/';
    public int notesColumn = 2;
    public int typeColumn = 1;
    public int idColumn = 0;

    // SPAWN
    private float ballRadius;
    private float paddleY;
    private float dropTime;
    private float deltaY;

    // STATE
    private bool isFinished = false;

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

        updateActiveBalls();

        removeFinishedBalls();
    }

    private void FixedUpdate() 
    {
        foreach(Ball ball in activeBallList){
            ball.FixedUpdateBall();
        }
    }

    private void updateActiveBalls()
    {
        // Update all active balls
        foreach(Ball ball in activeBallList){
            ball.UpdateBall();

            if(ball.checkIfFinished()) { 
				finishedBallList.Add(ball);
			}
        }
    }

    private void removeFinishedBalls()
    {
        // Destroy any balls that are caught or missed
		foreach(Ball rmBall in finishedBallList) {
			activeBallList.Remove(rmBall);
            rmBall.DeleteBall();
		}
        finishedBallList.Clear();
    }

    public void CheckDrop() 
    {
        if(!isFinished && currentBall.getHitTime() - dropTime < Time.time)
        {
            currentBall.TriggerActivation();
            activeBallList.Add(currentBall);
            nextBall();
        }
	}

    public void nextBall() 
    {
        if(currentBallIndex < balls.Count - 2){ // TEMPORARILY 2 REVERT TO 1
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

    public void spawnSimpleBall(int id, List<Note> notes)
    {
        // Instantiate the ball
        SimpleBall ball = Instantiate(simpleBall).GetComponent<SimpleBall>();
        ball.transform.parent = transform; // set BallDropper gameobject to parent

        // Initialize the ball with id and notes
        ball.InitializeBall(id, notes);

        // Get Spawn Time info
        dropTime = ball.GetSpawnTimeOffset();

        // Add to list of balls
        balls.Add(ball);
    }

    public void spawnBounceBall(int id, List<Note> notes)
    {
         // Instantiate the ball
        BounceBall ball = Instantiate(bounceBall).GetComponent<BounceBall>();
        ball.transform.parent = transform; // set BallDropper gameobject to parent

        // Initialize ball with id and notes
        ball.InitializeBall(id, notes);

        // Add to list of balls
        balls.Add(ball);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * READER
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public void loadBalls()
    {
        //print("loading");
        song.loadSong(); // this should not be here

        // Load the Ball Sheet and the Notes Sheet
        string path = ballsLocation + ballMapName + "/";
        //print(path);
        //print(ballsLocation);
        //print(ballMapName);
        string ballsPath = path + ballMapName + "_balls.csv";
        //print(ballsPath);

        StreamReader reader = new StreamReader(ballsPath);
        string currLine = reader.ReadLine(); // this skips the labels row

        while((currLine = reader.ReadLine()) != null){
            // Get info from sheet
            string[] ballInfo = currLine.Split(delimeter);
            int id = int.Parse(ballInfo[idColumn]);
            string type = ballInfo[typeColumn].ToLower();

            // Get the balls' notes from Song
            string[] noteString = ballInfo[notesColumn].Split(noteDelimeter);
            int[] notes = Array.ConvertAll<string, int>(noteString, int.Parse);
            List<Note> noteList = song.getNotes(notes);

            // Spawn the Ball depending on the type
            switch(type){
                case "basic":
                    spawnSimpleBall(id,noteList);
                    break;
                case "bounce":
                    spawnBounceBall(id,noteList);
                    break;
                default:
                    break;
            }
        }
    }
}
