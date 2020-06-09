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

    private Vector2 _movement;
    private Vector2 paddlePos;

    private void Awake() 
    {
        height = GetComponent<Collider2D>().bounds.size.y;
    }

    public void Move(CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    void Update()
    {
        paddlePos += _movement * speed * Time.deltaTime;
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
