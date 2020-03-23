﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    public Vector2 movement;

    public Vector2 screenBounds;
    private float paddleYAxis = -4;
    private float paddleRadius;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords
        paddleRadius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        movePaddle();
    }

    private void movePaddle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 paddlePos = new Vector2(mousePos.x, paddleYAxis); // set paddle pos

        paddlePos.x = Mathf.Clamp(paddlePos.x, -screenBounds.x + paddleRadius, screenBounds.x - paddleRadius); // check x boundary

        transform.position = paddlePos;
    }
}
