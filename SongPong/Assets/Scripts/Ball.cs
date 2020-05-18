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
    public BallData ballData;

    //___________ATTRIBUTES_____________
    public int id;
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

    //___________NOTES__________________
    private NoteData[] notes;

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

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * STATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public StateEvent onStateChange;
    public class StateEvent : UnityEvent<State> { }

    // Given a state, if not equal to the current state
    // sets current state to the new state and
    // invokes the StateEvent
    public void ChangeState(State s)
    {
        if (status == s) return;
        status = s;
        if (onStateChange != null)
            onStateChange.Invoke(status);

    }

    public void AddToStatusChange(UnityAction<State> action)
    {
        if(onStateChange == null)
            onStateChange = new StateEvent();

        onStateChange.AddListener(action);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void InitializeBall(int id, NoteData[] notes)
    {
        // INITIALIZE ID AND NOTES
        this.id = id;
        this.notes = notes;

        // REFERENCES
        paddle = GameObject.Find("Paddle").GetComponent<Paddle>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // APPEARANCE
        gameObject.layer = LayerMask.NameToLayer("Balls");

        // SET SPAWN LOCATION
        SpawnInfo spawner = GameObject.Find("Spawner").GetComponent<SpawnInfo>();
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
        if(status == State.Moving)
        {
            HandleMove();
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * IDLE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    protected abstract void HandleIdle();

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ACTIVATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void TriggerActivation(){ChangeState(State.Activated);}

    protected abstract void HandleActivate();

    private void Activate()
    {
        spawnTime = Time.time;
        ChangeState(State.Moving);
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

    private void DebugStatus()
    {
        Debug.Log(status);
    }
}
