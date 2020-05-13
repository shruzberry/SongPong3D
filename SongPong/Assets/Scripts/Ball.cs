﻿/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: Ball
Purpose: This abstract class defines attributes and behaviors that all balls in Song Pong have, including state info,
        notes, sprite, animations, etc.
Associations: All child ball types
__________ USAGE ___________
* To create a new type of ball, have it extend this class. You must implement ALL methods required.
________ ATTRIBUTES ________
+ id: the unique ball ID the player uses
+ status: the current state of the ball
* notes: a list of all notes this ball is associated with
* velocity: the velocity in 2D of this ball
________ FUNCTIONS _________
TODO
 +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ball : MonoBehaviour
{
    #region Variables

    //___________ATTRIBUTES_____________
    public int id;
    protected Vector2 spawnLoc;
    protected float size;

    protected Axis ballAxis;

    //___________REFERENCES_____________
    private SpawnInfo spawner;
    protected Paddle paddle;

    //___________STATE__________________

    protected BallState currentState;
    /*
    public enum State
    {
        Idle, // ball is waiting to drop
        Activated, // ball has just been spawned, or activated
        Moving, // ball is actively moving
        Missed, // ball is missed
        Caught, // ball caught by paddle
        Exit // ball is finished
    }
    public State status;
    */
    protected bool caught = false;
    protected bool missed = false;
    protected bool exit = false;

    //___________COMPONENTS_____________
    protected Rigidbody2D rb;
    protected Vector2 screenBounds;

    //___________NOTES__________________
    private NoteData[] notes;

    //___________MOVEMENT_______________
    protected Vector2 velocity;
    protected float spawnTime;
    public float dropTime;
    protected float catchTime;
    protected float direction;

    // BOUNCE
    protected int timesCaught = 0;

    #endregion Variables

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * STATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void SetState(BallState state)
    {
        if(currentState != null)
            currentState.OnStateExit();

        currentState = state;

        if(currentState != null)
            currentState.OnStateEnter();
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void InitializeBall(int id, NoteData[] notes)
    {
        // START IN IDLE STATE
        SetState(new IdleState());

        // INITIALIZE ID AND NOTES
        this.id = id;
        this.notes = notes;

        // COMPONENTS
        rb = GetComponent<Rigidbody2D>();

        // REFERENCES
        spawner = GameObject.Find("Spawner").GetComponent<SpawnInfo>();
        paddle = GameObject.Find("Paddle").GetComponent<Paddle>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // APPEARANCE
        gameObject.layer = LayerMask.NameToLayer("Balls");

        // GO TO SPAWN LOCATION
        ballAxis = spawner.gameAxis; // set the ball's axis
        int spawnNumber = notes[0].hitPosition; // the first note's spawn location
        spawnLoc = spawner.GetSpawnLocation(spawnNumber);
        transform.position = spawnLoc;
        //direction = noteList[0].getDirection();

        // CALL BALL IMPLEMENTATION'S CONSTRUCTOR
        InitializeBallSpecific();
        dropTime = GetSpawnTimeOffset();

    }

    public abstract void InitializeBallSpecific();

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void UpdateBall()
    {
        CheckMiss();
        CheckCatch();
        
        currentState.Tick();
    }

    public void FixedUpdateBall()
    {
        currentState.FixedTick();
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVING
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected abstract void HandleMove();
    public abstract float GetSpawnTimeOffset();

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    abstract protected void CheckCatch();
    abstract protected void HandleCatch();

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MISS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    abstract protected void CheckMiss();
    abstract protected void HandleMiss();

    private void Miss()
    {
        
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    abstract protected void HandleExit();

    public void DeleteBall()
    {
        Destroy(this.gameObject);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float getSize(){return size;}

    public float getHitTime(){return notes[0].hitTime;}

    //public int getSpawnColumn(){return notes[0].getColumn();}

    public bool checkIfFinished(){return exit;}
}
