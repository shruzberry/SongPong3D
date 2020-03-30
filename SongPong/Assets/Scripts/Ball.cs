using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ball : MonoBehaviour
{
    // ATTRIBUTES
    protected int id;
    protected float size;
    protected float radius;

    // COMPONENTS
    protected Rigidbody2D rb;
    protected Vector2 screenBounds;

    // NOTES
    protected List<Note> notes;

    // VELOCITY
    protected float velocityX;
    protected float velocityY;
    protected float gravity;
    protected float spawnTime;
    protected float catchTime;

    // BOUNCE
    protected int numBouncesLeft;

    // STATE
    protected bool isFalling = false;
    protected bool isFinished = false;
    protected bool missed = false;

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public void initBallInfo(int id, List<Note> noteList)
    {
        this.id = id;
        this.notes = noteList;
    }

    public void initBallPhysics(Vector3 pos, float velocity, float acceleration){
        setPosition(pos);
        this.velocityY = velocity;
        gravity = acceleration;
    }
    protected void Awake() {
        rb = GetComponent<Rigidbody2D>();
        size = GetComponent<Collider2D>().bounds.size.y;

        radius = size / 2;
    }
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        gameObject.layer = LayerMask.NameToLayer("Balls");
        velocityX = 0;
        isFalling = true;
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void UpdateBall()
    {
        if(transform.position.y < -screenBounds.y){
           handleMiss();
        }
    }

    public void FixedUpdateBall()
    {
        velocityY += gravity * Time.fixedDeltaTime;

        Vector2 newPos = new Vector2(velocityX * Time.deltaTime, velocityY * Time.deltaTime);
        rb.MovePosition((Vector2)transform.position + newPos);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * COLLIDE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Paddle"){
            velocityY = -velocityY;
            catchTime = Time.time;
            print("Time to catch: " + (catchTime - spawnTime));
            handleCatch();
        }
    }

    public void DeleteBall(){
        Destroy(this.gameObject);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public float getSize(){return size;}
    public float getHitTime(){
       return notes[0].getHitTime();
    }
    public int getSpawnColumn(){return notes[0].getColumn();}
    public bool checkIsFinished(){return isFinished;}
    public bool checkMissed(){return missed;}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public void setPosition(Vector3 loc){transform.position = loc;}
    public void setDropSpeed(float speed){velocityY = speed;}
    public void setAcceleration(float acc){gravity = acc;}

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * ABSTRACT EVENT HANDLERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    abstract public void handleDrop();
    abstract protected void handleCatch();
    abstract protected void handleMiss();
}
