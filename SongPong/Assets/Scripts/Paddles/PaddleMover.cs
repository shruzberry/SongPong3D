using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(BoxCollider))]
public class PaddleMover : MonoBehaviour
{
    //__________ATTRIBUTES________________
    public float radius; // the half-width or radius of the paddle
    public float height; // the height / thickness of the paddle
    public float yValue;
    public float distFromTrack;

    //__________MOVEMENT________________
    public float speed = 10.0f;
    public float sprint;

    public Vector3 paddleAxis;
    public Vector2 bounds;

    private Vector3 _movement;
    private Vector3 paddlePos;

    private float sprintTriggerTime;

    private WLED leftWLED;
    private WLED rightWLED;

    private void Awake() 
    {
        radius = GetComponent<BoxCollider>().bounds.size.x / 2;
        height = GetComponent<BoxCollider>().bounds.size.z;
    }

    public void Initialize(PaddleManager pm)
    {
        LED[] leds = GetComponentsInChildren<LED>();
        foreach(LED led in leds)
        {
            led.Initialize(pm);
        }

        leftWLED = this.transform.Find("paddle").transform.Find("WLED_L").GetComponent<WLED>();
        rightWLED = this.transform.Find("paddle").transform.Find("WLED_R").GetComponent<WLED>();

        leftWLED.Initialize(this);
        rightWLED.Initialize(this);
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

        paddlePos = Clamp.ClampToAxis(paddlePos, paddleAxis, bounds);
        paddlePos.y = yValue;
        paddlePos.z = distFromTrack;

        transform.position = paddlePos;

        if(_movement == Vector3.left)
        {
            leftWLED.TurnLEDOn();
            rightWLED.TurnLEDOff();
        }
        else if(_movement == Vector3.right)
        {
            leftWLED.TurnLEDOff();
            rightWLED.TurnLEDOn();
        }
        else
        {
            leftWLED.TurnLEDOn();
            rightWLED.TurnLEDOn();
        }
    }

    /**
     * Returns the paddles' axis adjusted so that it lies on the top of the paddle instead of the middle
     */
    public float GetPaddleTopLoc()
    {
        float paddleHalfHeight = height / 2;
        return distFromTrack + paddleHalfHeight;
    }
}
