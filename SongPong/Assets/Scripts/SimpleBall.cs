using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBall : MonoBehaviour
{
    // REFERENCES

    // ATTRIBUTES
    private int ballNum;
    private float size;
    private float radius;

    // COMPONENTS
    private Rigidbody2D rb;
    private Vector2 screenBounds;

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

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        size = GetComponent<Collider2D>().bounds.size.y;

        radius = size / 2;
    }

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        gameObject.layer = LayerMask.NameToLayer("Balls");
        spawnTime = Time.time;
        velocityX = 0;
        isFalling = true;
    }

    void Update()
    {
        if(transform.position.y < -screenBounds.y){
           handleMiss();
        }
    }

    void FixedUpdate()
    {
        velocityY += gravity * Time.fixedDeltaTime;

        moveBall();
    }

    private void moveBall()
    {
        Vector2 newPos = new Vector2(velocityX * Time.deltaTime, velocityY * Time.deltaTime);
        rb.MovePosition((Vector2)transform.position + newPos);
    }

    public float getBallSize(){return size;}
    public void setBallDropSpeed(float speed){velocityY = speed;}
    public void setBallAcceleration(float acc){gravity = acc;}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Paddle"){
            velocityY = -velocityY;
            catchTime = Time.time;
            print("Time to catch: " + (catchTime - spawnTime));
        }
    }


    private void handleMiss(){
        print("Missed Ball");
        Destroy(this.gameObject);
    }
}
