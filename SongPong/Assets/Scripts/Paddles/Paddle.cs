using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/**
 * The paddle that the player controls to catch balls
 */
[RequireComponent(typeof(BoxCollider))]
public class Paddle : MonoBehaviour
{
    //_____ SETTINGS ____________________
    //_____ REFERENCES __________________
    PaddleManager paddleManager;

    //_____ COMPONENTS __________________
    private WLED leftWLED;
    private WLED rightWLED;

    //_____ ATTRIBUTES __________________
    public Vector3 paddleAxis;
    public Vector2 bounds;

    public float radius; // the half-width or radius of the paddle
    public float height; // the height / thickness of the paddle
    public float yValue;
    public float distFromTrack;

    //_____ STATE  ______________________

    //_____ MOVEMENT ____________________
    public float speed = 10.0f;
    public float sprint;
    private Vector3 _movement;
    private Vector3 paddlePos;

    private float sprintTriggerTime;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Awake() 
    {
        radius = GetComponent<BoxCollider>().bounds.size.x / 2;
        height = GetComponent<BoxCollider>().bounds.size.z;
    }

    public void Initialize(PaddleManager pm)
    {
        this.paddleManager = pm;
        this.speed = pm.speed;
        this.paddleAxis = pm.paddleAxis;
        this.distFromTrack = pm.distFromTrack;
        this.yValue = pm.yValue;
        this.bounds = pm.bounds;

        InitializeLEDs();
    }

    private void InitializeLEDs()
    {
        LED[] leds = GetComponentsInChildren<LED>();
        foreach(LED led in leds)
        {
            led.Initialize(paddleManager);
        }

        leftWLED = this.transform.Find("paddle").transform.Find("WLED_L").GetComponent<WLED>();
        rightWLED = this.transform.Find("paddle").transform.Find("WLED_R").GetComponent<WLED>();

        leftWLED.Initialize(this);
        rightWLED.Initialize(this);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Move(CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void Sprint(CallbackContext context)
    {
        sprint = context.ReadValue<float>() * 5.0f;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

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

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Returns the paddles' axis adjusted so that it lies on the top of the paddle instead of the middle
     */
    public float GetPaddleTopLoc()
    {
        float paddleHalfHeight = height / 2;
        return distFromTrack + paddleHalfHeight;
    }
}
