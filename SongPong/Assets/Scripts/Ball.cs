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
using Types;

public abstract class Ball : MonoBehaviour
{
    #region Variables

    //___________ATTRIBUTES_____________
    public int id;
    protected BallTypes type;
    protected Vector2 spawnLoc;
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

    //___________DATA___________________
    protected NoteData[] notes;
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
    public float catchTime;

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

    public void InitializeBall(BallData data, SpawnInfo spawner, Paddle paddle)
    {
        // INITIALIZE ID AND NOTES
        this.id = data.id;
        this.ballData = data;
        this.notes = data.notes;
        this.type = data.type;

        // INDEXING
        currentNote = 0;
        numNotes = notes.Length;

        // REFERENCES
        this.paddle = paddle;

        // APPEARANCE
        gameObject.layer = LayerMask.NameToLayer("Balls");

        // SET SPAWN LOCATION
        axis = spawner.gameAxis; // set the ball's axis
        int spawnNumber = notes[currentNote].hitPosition; // the first note's spawn location
        spawnLoc = spawner.GetSpawnLocation(spawnNumber);
        transform.position = spawnLoc;

        // DIRECTION
        direction = notes[currentNote].noteDirection;

        // CALL BALL IMPLEMENTATION'S CONSTRUCTOR
        InitializeBallSpecific();

        // START IN IDLE STATE
        SetState(new IdleState(this));
    }

    public abstract void InitializeBallSpecific();

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

    abstract public void CatchActions();

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
        Destroy(this.gameObject);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float getSize(){return size;}

    public float NextHitTime(){return notes[currentNote].hitTime;}

    public bool checkIfFinished(){return exit;}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * DEBUG
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected void DebugDropTime()
    {
        print("Expected Ball Drop Time: " + moveTime + " sec.");
    }

    protected void DebugCatchTime()
    {
        //print("CatchTime: " + catchTime);
        //print("SpawnTime: " + spawnTime);
        //print("Time to catch: " + (catchTime - spawnTime));
        Debug.Log("Caught Ball " + id + " at time " + Time.time);
    }
}
