using UnityEngine;
using System.Collections.Generic;
using Types;

/**
 * This abstract class defines attributes and behaviors that all balls in Song Pong have, including state info,
 * notes, sprites, animations, etc.
 */

[RequireComponent(typeof(SphereCollider))]
public abstract class Ball : MonoBehaviour
{
    //_____ SETTINGS ____________________

    //_____ REFERENCES _________________
    private BallDropper dropper;
    public SongController song;

    //_____ COMPONENTS ________________
    private SphereCollider ball_collider;
    private GameObject ball_render;

    //_____ ATTRIBUTES __________________
    public int id; // unique id each ball receives
    public BallTypes type;
    protected float size;
    public List<BallOption> options;

    public List<NoteData> notes;
    public int numNotes;
    public int currentNote;

    [HideInInspector]
    public BallData ballData;

    //_____ BOOLS ______________________

    //_____ OTHER ______________________
    protected Vector3 axisVector; // the vector representing which axis the game is played on: (1,0) is x-axis, (0,1) is y-axis
    protected Vector3 otherAxisVector; // the unit vector that represents the other axis (with same direction)

    //_____ STATE_______________________
    protected BallState currentState;

    public bool ready = false;
    public bool caught = false;
    public bool missed = false;
    public bool exit = false;

    //_____ EVENTS _____________________
    public delegate void BallReady(Ball ball);
    public event BallReady onBallReady;

    public delegate void BallCaught(Ball ball);
    public event BallCaught onBallCaught;

    public delegate void BallSpawn(Ball ball);
    public event BallSpawn onBallSpawn;

    //_____ MOVEMENT ____________________
    public float negative;

    //_____ TIME ________________________
    public float spawnTimeBeats;
    public float[] catchTimesBeats;


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

    public virtual void Initialize(Game game, BallData data, BallDropper dropper)
    {
        // REFERENCES
        this.ballData = data;
        this.dropper = dropper;
        this.song = game.songController;

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
        size = dropper.GetSize();
        SetSize();

        // NOTES
        this.notes = data.notes;
        currentNote = 0;
        numNotes = notes.Count;
        catchTimesBeats = new float[numNotes + 1];

        // POSITION
        SetAxisVectors();
        transform.position = GetNotePosition(currentNote);

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

    private void SetSize()
    {
        ball_collider.radius = size / 2;
        transform.localScale = new Vector2(size, size);
        ball_render.transform.localScale = new Vector2(size, size);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * NOTES
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void NextNote(){ currentNote++; }

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
