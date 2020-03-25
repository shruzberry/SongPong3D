﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    public Vector2 movement;

    public Vector2 screenBounds;
    private float paddleYAxis = -4;
    private float paddleRadius;
    private float paddleHeight;

    // Start is called before the first frame update

    void Awake(){
        paddleRadius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        paddleHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        movePaddle();
    }

    public float getPaddleYAxis(){return paddleYAxis;}

    public float getPaddleRadius(){return paddleRadius;}

    public float getPaddleHeight(){return paddleHeight;}

    private void movePaddle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 paddlePos = new Vector3(mousePos.x, paddleYAxis, 2); // set paddle pos

        paddlePos.x = Mathf.Clamp(paddlePos.x, -screenBounds.x + paddleRadius, screenBounds.x - paddleRadius); // check x boundary

        transform.position = paddlePos;
    }
}