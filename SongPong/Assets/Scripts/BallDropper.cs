﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BallDropper : MonoBehaviour
{
    // REFERENCES
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

    // CONSTANTS
    public float dropSpeed = -1.0f;
    public float gravity = -3;

    // SPAWN
    private Vector2 screenBounds;
    public float ballDropY;
    private float ballRadius;
    private float paddleY;
    private float dropTime;

    // STATE
    private bool isFinished = false;

    // COLUMNS
    private float[] ballCols; // x-positions of each column in world coordinates
    private bool showColumns = true; // if true, show columns
    private static int NUM_COL = 16; // number of ball columns

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Awake() {
        paddle = GameObject.Find("Paddle").GetComponent<Paddle>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        spawnTestBall();
        calcColumns();
    }
    void Start()
    {
        dropTime = calcDropTime();
        loadBalls();
        currentBallIndex = 0;
        currentBall = balls[currentBallIndex];
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
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

    private void Update()
    {
        drawColumns();

        checkDrop();

        updateActiveBalls();

        removeFinishedBalls();
    }

    private void updateActiveBalls()
    {
        // Update all active balls
        foreach(Ball ball in activeBallList){
            ball.UpdateBall();

            if(ball.checkIsFinished()) { 
				finishedBallList.Add(ball);
			} else if(ball.checkMissed()) { 
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

    private void FixedUpdate() 
    {
        foreach(Ball ball in activeBallList){
            ball.FixedUpdateBall();
        }
    }

    public void checkDrop() 
    {
        if(!isFinished){
            if((currentBall.getHitTime() - dropTime < Time.time)){
                currentBall.handleDrop();
                activeBallList.Add(currentBall);
                nextBall();
            }
        }
	}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SPAWN
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void spawnSimpleBall(int id, List<Note> notes)
    {
        // Instantiate the ball
        GameObject b = Instantiate(simpleBall);
        b.transform.parent = transform; // set BallDropper gameobject to parent
        SimpleBall ball = b.GetComponent<SimpleBall>();

        // Pass data from the sheet to the ball
        ball.initBallInfo(id, notes);

        // Put the ball in right place
        int column = ball.getSpawnColumn();
        Vector3 spawnPos = new Vector3(ballCols[column-1], ballDropY, 0);
        ball.initBallPhysics(spawnPos, dropSpeed, gravity);

        balls.Add(ball);
    }

    public void spawnBounceBall(int id, List<Note> notes)
    {
         // Instantiate the ball
        GameObject b = Instantiate(bounceBall);
        b.transform.parent = transform; // set BallDropper gameobject to parent
        BounceBall ball = b.GetComponent<BounceBall>();

        // Pass data from the sheet to the ball
        ball.initBallInfo(id, notes);

        // Put the ball in right place
        int column = ball.getSpawnColumn();
        Vector3 spawnPos = new Vector3(ballCols[column-1], ballDropY, 0);
        ball.initBallPhysics(spawnPos, dropSpeed, gravity);

        balls.Add(ball);
    }

    public void spawnTestBall()
    {
        Vector3 spawnPos = new Vector3((-screenBounds.x * 2), (-screenBounds.y * 2), 0);
        GameObject simp = Instantiate(simpleBall, spawnPos, Quaternion.identity);
        SimpleBall simpScript = simp.GetComponent<SimpleBall>();
        ballRadius = simpScript.getSize() / 2;
        simpScript.DeleteBall();
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CALCULATIONS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private float calcDropTime()
    {
        // Get Paddle Info
        paddleY = paddle.getPaddleYAxis();
        float paddleHeightHalf = paddle.getPaddleHeight() / 2;
        //print("Paddle Height: " + paddleHeightHalf);
        
        // Ball Info
        ballDropY = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height + 200)).y;
        //print("BALL RADIUS: " + ballRadius);
        
        // Determine Delta Y
        float deltaY = paddleY - ballDropY + ballRadius + paddleHeightHalf;
        //print("DISTANCE TO FALL: " + deltaY + " world units");

        // Calculate delta T
        // using physics equation dy = v0t + 1/2at^2 solved for time in the form
        // t = (-v0 +- sqrt(v0^2 + 2ady)) / a
        float determinant = (Mathf.Pow(dropSpeed, 2) + (2 * gravity * deltaY));
        float time = (-dropSpeed - Mathf.Sqrt(determinant)) / gravity;

        print("Expected Ball Drop Time: " + time + " sec.");
        return time;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * READER
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public void loadBalls()
    {
        song.loadSong(); // this should not be here

        // Load the Ball Sheet and the Notes Sheet
        string path = ballsLocation + ballMapName + "/";
        string ballsPath = path + ballMapName + "_balls.csv";

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

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * COLUMNS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void calcColumns()
    {
        ballCols = new float[NUM_COL+1]; // need n+1 lines to make n columns
        int width = Screen.width;
        int screenPadding = (int)(width * 0.1); // padding is 10% of screen width on each side (20% total)
		int effScreenW = width - 2 * screenPadding; // the screen width minus the padding
	    //print("ScreenW: " + width + "screenPadding: " + screenPadding + " EffScreenWidth: " + effScreenW);

		int colStep = effScreenW / NUM_COL; // amount of x to move per column

		for(int i = 0; i < NUM_COL+1; i++) {
			ballCols[i] = Camera.main.ScreenToWorldPoint(new Vector3(colStep * i + screenPadding,0,0)).x;
			//print("Column " + i + " is located at x = " + ballCols[i]);
		}
    }

    void drawColumns()
    {
        int z = 1;
        if (ballCols != null && showColumns)
        {
            // Draw a white line over each column
            for(int i = 0; i < ballCols.Length; i++){
                Vector3 top = new Vector3(ballCols[i], screenBounds.y, z);
                Vector3 bot = new Vector3(ballCols[i],-screenBounds.y, z);
                Debug.DrawLine(top,bot, Color.white,1,false);
            }
        }
    }
}
