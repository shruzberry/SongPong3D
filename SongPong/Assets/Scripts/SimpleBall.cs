using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBall : MonoBehaviour
{
    // REFERENCES

    // ATTRIBUTES
    private int id;
    private float size;
    private float radius;

    // COMPONENTS
    private Rigidbody2D rb;
    private Vector2 screenBounds;

    // NOTES
    private List<Note> notes;

    // VELOCITY
    private float velocityX;
    private float velocityY;
    private float gravity;
    private float spawnTime;
    private float catchTime;

    // BOUNCE
    private int numBouncesLeft;

    // STATE
    private bool isFalling;

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

    void Awake(){
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

    public void UpdateBall()
    {
        if(transform.position.y < -screenBounds.y){
           handleMiss();
        }
    }

    public void FixedUpdateBall()
    {
        velocityY += gravity * Time.fixedDeltaTime;

        moveBall();
    }

    public void dropBall(){
        spawnTime = Time.time;
    }

    private void moveBall()
    {
        Vector2 newPos = new Vector2(velocityX * Time.deltaTime, velocityY * Time.deltaTime);
        rb.MovePosition((Vector2)transform.position + newPos);
    }

    public float getSize(){return size;}
    public void setDropSpeed(float speed){velocityY = speed;}
    public void setAcceleration(float acc){gravity = acc;}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Paddle"){
            velocityY = -velocityY;
            catchTime = Time.time;
            print("Time to catch: " + (catchTime - spawnTime));
        }
    }

    public float getHitTime(){
       return notes[0].getHitTime();
    }

    public int getSpawnColumn(){return notes[0].getColumn();}

    public void setPosition(Vector3 loc){
        transform.position = loc;
    }

    private void handleMiss(){
        print("Missed Ball");
        Destroy(this.gameObject);
    }
}
