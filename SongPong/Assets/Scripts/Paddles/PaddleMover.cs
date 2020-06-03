using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    public float speed = 10.0f;
    public int playerIndex = 0;

    private Vector2 _movement;
    private Vector2 paddlePos;

    public void SetInputVector(Vector2 direction)
    {
        _movement = direction;
    }

    void Update()
    {
        paddlePos += _movement * speed * Time.deltaTime;
        transform.position = paddlePos;
    }
}
