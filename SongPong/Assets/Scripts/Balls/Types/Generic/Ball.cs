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
using Types;

[RequireComponent(typeof(SphereCollider))]
public abstract class Ball : MonoBehaviour
{
    //___________REFERENCES_____________
    private BallDropper dropper;
    public SongController song;

    //___________ATTRIBUTES_____________
    public int id;
    public BallTypes type;
    public Vector3 spawnLoc;
    protected float size;

    protected GameType gameType;
    protected Vector3 axisVector; // the vector representing which axis the game is played on: (1,0) is x-axis, (0,1) is y-axis
    protected Vector3 otherAxisVector; // the unit vector that represents the other axis (with same direction)

    //___________COMPONENTS_____________
    private SphereCollider ball_collider;
    private GameObject ball_render;

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

    public delegate void BallSpawn(Ball ball);
    public event BallSpawn onBallSpawn;

    //___________DATA____________________
    public List<NoteData> notes;
    [HideInInspector]
    public BallData ballData;

    //___________MOVEMENT________________
    public float negative;

    //___________TIME____________________
    public float spawnTimeBeats;
    public float[] catchTimesBeats;

    //___________NOTES___________________
    public int numNotes;
    public int currentNote;

    //___________OPTIONS_________________
    public List<BallOption> options;

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

    public virtual void InitializeBall(Game game, BallData data, BallDropper dropper)
    {
        // REFERENCES
        this.ballData = data;
        this.dropper = dropper;
        this.song = game.songController;
        this.gameType = game.gameType;

        // COMPONENTS
        ball_collider = GetComponent<SphereCollider>();
        ball_render = transform.Find("Render").gameObject;

        // DATA
        this.type = data.type;
        this.options = data.options;

        // APPEARANCE
        this.id = dropper.currentBallIndex;
        this.name = id.ToString() + "_" + type.ToString();
        gameObject.layer = LayerMask.NameToLayer("Balls");
        size = dropper.size;
        ball_collider.radius = size/2;
        transform.localScale = new Vector2(size,size);
        ball_render.transform.localScale = new Vector2(size, size);

        // NOTES
        this.notes = data.notes;
        currentNote = 0;
        numNotes = notes.Count;

        // INDEXING
        catchTimesBeats = new float[numNotes + 1];

        // POSITION
        spawnLoc = GetNotePosition(currentNote);
        transform.position = spawnLoc;

        // DIRECTION
        SetAxisVectors();

        // START IN IDLE STATE
        SetState(new IdleState(this));

        // TRIGGER SPAWN EVENT
        if(onBallSpawn != null) onBallSpawn(this);
    }

    public virtual void SetAxisVectors()
    {
        negative = (notes[currentNote].noteDirection == Direction.negative) ? -1.0f : 1.0f;

        axisVector = new Vector3(0,0,1);
        otherAxisVector = new Vector3(1,0,0);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * NOTES
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void NextNote(){ currentNote++; }

    /**
     * Returns the position in world coordinates of the given note
     */
    public Vector3 GetNotePosition(int index)
    {
        int spawnNum = notes[index].hitPosition;
        return dropper.GetSpawnLocation(spawnNum);
    }

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
 * IDLE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public virtual void OnIdleExit()
    {
        spawnTimeBeats = song.GetSongTimeBeats();
    }
    
    public virtual void ReadyActions(){if(onBallReady != null) onBallReady(this);}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVING
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public abstract void MoveActions();
    public virtual void ResetMove(){}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CATCH
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public virtual void OnCatchActions()
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

    public void DestroyBall()
    {
        Destroy(gameObject);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public List<NoteData> getNotes(){return notes;}

}
