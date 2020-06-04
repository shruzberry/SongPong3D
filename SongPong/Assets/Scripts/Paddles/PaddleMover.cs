using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    public float radius; // the half-width or radius of the paddle
    public float height; // the height / thickness of the paddle

    public float speed = 10.0f;
    public int playerIndex = 0;

    public Vector2 paddleAxis;
    public float axisValue;

    private Vector2 _movement;
    private Vector2 paddlePos;

    public void Initialize(Vector2 axis, float axisValue)
    {
        paddleAxis = axis;
        this.axisValue = axisValue;
    }

    public void SetInputVector(Vector2 direction)
    {
        _movement = direction;
    }
    
    private void Awake() 
    {
        radius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        paddlePos += _movement * speed * Time.deltaTime;
        paddlePos = Clamp.ClampToAxis(paddlePos, radius, paddleAxis, axisValue);
        transform.position = paddlePos;
    }
}
