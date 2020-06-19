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

    public float sprint;

    //[HideInInspector]
    public Vector2 paddleAxis;

    private Vector2 _movement;
    private Vector2 paddlePos;

    private float sprintTriggerTime;

    private void Awake() 
    {
        radius = GetComponent<Collider2D>().bounds.size.x / 2;
        height = GetComponent<Collider2D>().bounds.size.y;
    }

    public void Move(CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void Sprint(CallbackContext context)
    {
        sprint = context.ReadValue<float>() * 5.0f;
    }

    void Update()
    {
        paddlePos += _movement * (speed + sprint) * Time.deltaTime;
        paddlePos = Clamp.ClampToAxis(paddlePos, radius, paddleAxis);
        transform.position = paddlePos;
    }

    public Vector2 GetAxis()
    {
        return paddleAxis;
    }

    /**
     * Returns the paddles' axis adjusted so that it lies on the top of the paddle instead of the middle
     */
    public Vector2 GetPaddleTopAxis()
    {
        float paddleHalfHeight = height / 2;
        return paddleAxis - (paddleAxis.normalized * paddleHalfHeight);
    }
}
