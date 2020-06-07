/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
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

using UnityEngine;
using System.Collections.Generic;
using System;
using Types;

public abstract class Ball : MonoBehaviour
{
    #region Variables

    //___________ATTRIBUTES_____________
    public int id;
    public BallTypes type;
    public Vector2 spawnLoc;
    protected float size;

    protected Axis axis;
    protected Vector2 axisVector; // the unit vector that represents the base of the axis and direction
    protected Vector2 otherAxisVector; // the unit vector that represents the other axis (with same direction)

    //___________REFERENCES_____________
    //protected PlayerInputHandler paddleManager;
    protected SpawnInfo spawnInfo;
    public SongController song;
    
    //___________COMPONENTS_____________
    protected SpriteRenderer ball_renderer;

    //___________STATE__________________
    protected BallState currentState;

    public bool ready = false;
    public bool caught = false;
    public bool missed = false;
    public bool exit = false;

    //___________EVENTS_________________
    public delegate void BallReady(Ball ball);
    public event BallReady onBallReady;

    public delegate void BallCaught(Ball ball);
    public event BallCaught onBallCaught;

    //___________DATA___________________
    protected NoteData[] notes;
    [HideInInspector]
    public BallData ballData;

    //___________MOVEMENT_______________
    public float negative;

    //___________TIME___________________
    public float spawnTime;
    public float[] catchTimes;

    //___________INDEXING________________
    public int numNotes;
    public int currentNote;

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

    public void InitializeBall(BallData data, AxisManager axisManager, SpawnInfo spawner,/* PlayerInputHandler paddleManager,*/ SongController song)
    {
        // REFERENCES
        //this.paddleManager = paddleManager;
        this.spawnInfo = spawner;
        this.song = song;

        // COMPONENTS
        this.ball_renderer = GetComponent<SpriteRenderer>();

        // INITIALIZE ID AND NOTES
        this.ballData = data;
        this.id = data.id;
        this.type = data.type;

        // APPEARANCE
        this.name = id.ToString() + "_" + type.ToString();
        gameObject.layer = LayerMask.NameToLayer("Balls");

        // NOTES
        this.notes = data.notes;
        currentNote = 0;
        numNotes = notes.Length;

        // INDEXING
        catchTimes = new float[numNotes + 1];

        // SET SPAWN LOCATION
        axis = axisManager.gameAxis; // set the ball's axis
        spawnLoc = GetNotePosition(currentNote);
//        Debug.Log("SPAWN: " + spawnLoc);
        transform.position = spawnLoc;

        // DIRECTION
        SetAxisVectors();

        // CALL BALL IMPLEMENTATION'S CONSTRUCTOR
        InitializeBallSpecific();

        // CHECK FOR ERRORS
        if(CheckForInvalid() == true)
        {
            Debug.LogWarning("BALL " + name + " did not initialize because it has incorrect parameters.");
        }

        // START IN IDLE STATE
        SetState(new IdleState(this));
    }

    public virtual void SetAxisVectors()
    {
        negative = (notes[currentNote].noteDirection == Direction.negative) ? -1.0f : 1.0f;
        
        if(axis == Axis.y && negative == 1.0f) {axisVector = new Vector2(0,1); otherAxisVector = new Vector2(1,0);}
        else if(axis == Axis.y && negative == -1.0f) {axisVector = new Vector2(0,-1); otherAxisVector = new Vector2(1,0);}
        else if(axis == Axis.x && negative == 1.0f) {axisVector = new Vector2(1,0); otherAxisVector = new Vector2(0,1);}
        else if(axis == Axis.x && negative == -1.0f) {axisVector = new Vector2(-1,0); otherAxisVector = new Vector2(0,-1);}
/*
        Vector2 testVector = new Vector2(2.90917239817623f,3.0f);
        Debug.Log("Test Vector: " + testVector);
        Debug.Log("Axis Vector: " + axisVector);
        float dotProduct = Vector2.Dot(testVector, axisVector);
        Debug.Log("Dot Product: " + dotProduct);
*/
    }

    public void SetDirection()
    {
    }

    public virtual void InitializeBallSpecific(){}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * NOTES
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void NextNote()
    {
        currentNote++;
    }

    /**
     * Returns the position in world coordinates of the given note
     */
    public Vector2 GetNotePosition(int index)
    {
        int spawnNum = notes[index].hitPosition;
        return spawnInfo.GetSpawnLocation(spawnNum);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ERROR
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected virtual bool CheckForInvalid(){return false;}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * IDLE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public virtual void ReadyActions(){if(onBallReady != null) onBallReady(this);}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void UpdateBall()
    {
        currentState.Tick();
    }

    public void FixedUpdateBall()
    {
        currentState.FixedTick();
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVING
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public abstract void MoveActions();
    public virtual void ResetMove(){}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public virtual void CatchActions()
    {
        if(onBallCaught != null) onBallCaught(this); // call the onBallCaught event, if there are subscribers
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MISS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public abstract bool CheckMiss();
    public abstract void MissActions();

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public abstract void OnExitActions();

    public abstract void ExitActions();

    public void DeleteBall()
    {
        Destroy(this);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float NextHitTime(){return notes[currentNote].hitTime;}

    public NoteData[] getNotes(){return notes;}

}
