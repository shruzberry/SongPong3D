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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ball : MonoBehaviour
{
    public BallData ballData;

    //___________ATTRIBUTES_____________
    public int id;
    protected Vector2 spawnLoc;
    protected float size;

    protected Axis ballAxis;

    //___________REFERENCES_____________
    private SpawnInfo spawner;
    protected Paddle paddle;

    //___________STATE__________________
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

        // START IN IDLE STATE
        ChangeState(State.Idle);
    }

    public abstract void InitializeBallSpecific();

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void UpdateBall()
    {
        if((int)status > 0) //If we are activated
        {
            CheckMiss();
            CheckCatch();
            //Add new check to change status right here
        }
        
        //DebugStatus();

        switch(status)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Activated:
                HandleActivate();
                Activate();
                break;
            case State.Caught:
                HandleCatch();
                break;
            case State.Missed:
                HandleMiss();
                Miss();
                break;
            case State.Exit:
                HandleExit();
                break;
            default:
                break;
        }
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
        ChangeState(State.Exit);
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

    private void DebugStatus()
    {
        Debug.Log(status);
    }
}
