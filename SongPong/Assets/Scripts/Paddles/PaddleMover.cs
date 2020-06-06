using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PaddleMover : MonoBehaviour
{
    public float radius; // the half-width or radius of the paddle
    public float height; // the height / thickness of the paddle

    public float speed = 10.0f;

    [HideInInspector]
    public Vector2 paddleAxis;
    [HideInInspector]
    public float axisValue;

    private Vector2 _movement;
    private Vector2 paddlePos;

    public void Move(CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    void Update()
    {
        paddlePos += _movement * speed * Time.deltaTime;
        paddlePos = Clamp.ClampToAxis(paddlePos, radius, paddleAxis, axisValue);
        transform.position = paddlePos;
    }
}
