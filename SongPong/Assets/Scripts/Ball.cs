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
    private BallTypes type;
    protected Vector2 spawnLoc;
    protected float size;

    public Axis axis;
    public Vector2 dirVector;

    //___________REFERENCES_____________
    protected Paddle paddle;

    //___________STATE__________________
    protected BallState currentState;

    public bool ready = false;
    public bool caught = false;
    protected bool missed = false;
    public bool exit = false;

    //___________COMPONENTS_____________
    protected Vector2 screenBounds;

    //___________DATA___________________
    private NoteData[] notes;
    public BallData ballData;

    //___________MOVEMENT_______________
    protected Vector2 velocity;
    protected Vector2 acceleration;
    protected Direction direction;

    //___________TIME___________________
    public float spawnTime;
    public float dropTime; // time it takes from spawn to target
    public float catchTime;

    //___________CATCHES________________
    public int catchesLeft;

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
        Debug.Log(id);
        this.notes = data.notes;
        this.type = data.type;

        // REFERENCES
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        this.paddle = paddle;

        // APPEARANCE
        gameObject.layer = LayerMask.NameToLayer("Balls");

        // SET SPAWN LOCATION
        axis = spawner.gameAxis; // set the ball's axis
        int spawnNumber = notes[0].hitPosition; // the first note's spawn location
        spawnLoc = spawner.GetSpawnLocation(spawnNumber);

        // DIRECTION
        direction = notes[0].noteDirection;

        if(axis == Axis.y)
        {
            if(direction == Direction.positive)
            {
                dirVector = new Vector2(0,1);
            }
            else
            {
                dirVector = new Vector2(0,-1);
            }
        }
        else
        {
            if(direction == Direction.positive)
            {
                dirVector = new Vector2(1,0);
            }
            else
            {
                dirVector = new Vector2(-1,0);
            }
        }

        // CALL BALL IMPLEMENTATION'S CONSTRUCTOR
        InitializeBallSpecific();

        // START IN IDLE STATE
        SetState(new IdleState(this));
    }

    public void GoToSpawnLoc()
    {
        transform.position = spawnLoc;
        catchesLeft = notes.Length;
    }

    public abstract void InitializeBallSpecific();

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void UpdateBall()
    {
        CheckMiss();
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
    public abstract float CalcDropTime();

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

    public float getHitTime(){return notes[0].hitTime;}

    public bool checkIfFinished(){return exit;}
}
