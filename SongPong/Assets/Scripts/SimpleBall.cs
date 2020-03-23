using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBall : MonoBehaviour
{
    // ATTRIBUTES
    private int ballNum;
    private Vector3 size;

    // COMPONENTS
    private Rigidbody2D rb;

    // VELOCITY
    private float velocityX;
    private float velocityY;
    private float gravity;

    // BOUNCE
    private int numBouncesLeft;
    private double spawnTime;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        size = GetComponent<Collider2D>().bounds.size;
    }

    void Start()
    {
        velocityX = 0;
        velocityY = -4;
        gravity = -3;
    }

    void FixedUpdate()
    {
        velocityY += gravity * Time.fixedDeltaTime;

        //moveBall();
    }

    private void moveBall(){
        Vector2 newPos = new Vector2(velocityX * Time.deltaTime, velocityY * Time.deltaTime);
        rb.MovePosition((Vector2)transform.position + newPos);
    }

    public float getBallSize(){
        return size.y;
    }
}
