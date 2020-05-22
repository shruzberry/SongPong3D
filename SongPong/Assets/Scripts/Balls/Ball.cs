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
    public bool canSpawn = true;
    public BallTypes type;
    public Vector2 spawnLoc;
    protected float size;

    protected Axis axis;
    protected Vector2 dirVector;

    //___________REFERENCES_____________
    protected Paddle paddle;

    //___________STATE__________________
    protected BallState currentState;

    public bool ready = false;
    public bool caught = false;
    public bool missed = false;
    public bool exit = false;

    //___________EVENTS_________________
    public delegate void BallReady(Ball ball);
    public event BallReady onBallReady;

    public delegate void BallCaught(Ball ball, Paddle paddle);
    public event BallCaught onBallCaught;

    //___________DATA___________________
    protected List<NoteData> notes;
    [HideInInspector]
    public BallData ballData;

    //___________MOVEMENT_______________
    [SerializeField]
    protected Vector2 velocity;
    protected Vector2 acceleration;
    protected Direction direction;

    //___________TIME___________________
    public float spawnTime;
    public float moveTime; // time it takes from spawn to target
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

    public void InitializeBall(BallData data, AxisManager axisManager, SpawnInfo spawner, Paddle paddle)
    {
        // INITIALIZE ID AND NOTES
        this.id = data.id;
        this.ballData = data;
        this.type = data.type;
        string ballName = id.ToString() + "_" + type.ToString();
        this.name = ballName;

        // NOTES
        this.notes = SortNotes(data.notes);
        currentNote = 0;
        numNotes = notes.Count;

        // INDEXING
        catchTimes = new float[numNotes + 1];

        // REFERENCES
        this.paddle = paddle;

        // APPEARANCE
        gameObject.layer = LayerMask.NameToLayer("Balls");

        // SET SPAWN LOCATION
        axis = axisManager.gameAxis; // set the ball's axis
        int spawnNumber = notes[currentNote].hitPosition; // the first note's spawn location
        spawnLoc = spawner.GetSpawnLocation(spawnNumber);
        transform.position = spawnLoc;

        // DIRECTION
        direction = notes[currentNote].noteDirection;

        // CALL BALL IMPLEMENTATION'S CONSTRUCTOR
        InitializeBallSpecific();

        // CHECK FOR ERRORS
        CheckForInvalid();

        // START IN IDLE STATE
        SetState(new IdleState(this));
    }

    protected List<NoteData> SortNotes(NoteData[] notes)
    {
        try
        {
            List<NoteData> noteList = new List<NoteData>();
            foreach(NoteData nd in notes)
            {
                noteList.Add(nd);
            }
            if(noteList.Count > 0)
            {
                noteList.Sort(NoteData.CompareNotesByHitTime);
            }

            foreach(NoteData nd in noteList)
            {
                //Debug.Log(nd.hitTime);
            }
            return noteList;
        }
        catch(Exception e)
        {
            Debug.LogError("Ball " + name + " has one or more null notes.");
            return null;
        }
    }

    public virtual void InitializeBallSpecific(){}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ERROR
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected virtual void CheckForInvalid(){}

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
    public abstract float CalcMoveTime();

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    virtual public void CatchActions()
    {
        if(onBallCaught != null) onBallCaught(this, paddle); // call the onBallCaught event, if there are subscribers

        if(currentNote < numNotes)
            currentNote++;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MISS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    abstract public bool CheckMiss();
    abstract public void MissActions();

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * EXIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    abstract public void ExitActions();

    public void DeleteBall()
    {
        Destroy(this);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float getSize(){return size;}

    public float NextHitTime(){return notes[currentNote].hitTime;}

    public bool checkIfFinished(){return exit;}

    public List<NoteData> getNotes(){return notes;}
}
