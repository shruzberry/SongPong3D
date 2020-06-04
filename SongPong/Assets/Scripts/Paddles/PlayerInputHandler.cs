using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private AxisManager axisManager;
    private Vector3 screenBounds;

    private PlayerInput playerInput;
    private PaddleMover paddleMover;
    
    //For shared controls
    private PaddleMover P1;
    private PaddleMover P2;

    // Axes
    private float paddleXAxis;
    private float paddleYAxis;
    private Vector2 paddleAxis_x;
    private Vector2 paddleAxis_y;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* INITIALIZE
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        axisManager = FindObjectOfType<AxisManager>();
        var movers = FindObjectsOfType<PaddleMover>();
        var index = playerInput.playerIndex;
        paddleMover = movers.FirstOrDefault(m => m.playerIndex == index);

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords

        if(axisManager.gameAxis == Axis.x) {InitPaddlesX(movers[0], movers[1]);}
        else if(axisManager.gameAxis == Axis.y) {InitPaddlesY(movers[0], movers[1]);}
    }

    private void InitPaddlesX(PaddleMover mover1, PaddleMover mover2)
    {
        // Calculate the paddle axis location
        paddleXAxis = screenBounds.x - (screenBounds.x * 0.1f);
        paddleAxis_x = new Vector2(screenBounds.x - (screenBounds.x * 0.1f), 0);

        this.P1 = mover1;
        P1.Initialize(Vector2.up, -paddleXAxis);
        P1.transform.eulerAngles = new Vector3(0,0,-90);

        this.P2 = mover2;
        P2.Initialize(Vector2.up, paddleXAxis);
        P2.transform.eulerAngles = new Vector3(0,0,90);

    }

    private void InitPaddlesY(PaddleMover mover1, PaddleMover mover2)
    {
        paddleYAxis = -screenBounds.y + (screenBounds.y * 0.15f);
        paddleAxis_y = new Vector2(0, -screenBounds.y + (screenBounds.y * 0.15f));

        this.P1 = mover1;
        P1.Initialize(Vector2.right, paddleYAxis);
        P1.transform.eulerAngles = new Vector3(0,0,0);

        this.P2 = mover2;
        P2.Initialize(Vector2.right, -paddleYAxis);
        P2.transform.eulerAngles = new Vector3(0,0,180);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* INPUT
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void MoveP1(CallbackContext context)
    {
        if(paddleMover != null)
            P1.SetInputVector(context.ReadValue<Vector2>());
    }

    public void MoveP2(CallbackContext context)
    {
        if(paddleMover != null)
            P2.SetInputVector(context.ReadValue<Vector2>());
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public Vector2 GetPaddleAxis()
    {
        Axis gameAxis = axisManager.gameAxis;
        if(gameAxis == Axis.y)
            return paddleAxis_y;
        else if(gameAxis == Axis.x)
            return paddleAxis_x;
        else{
            Debug.LogError("INVALID AXIS DETECTED.");
            return Vector2.zero;
        }
    }
/*
    public Vector2 GetPaddleLocation(Paddles paddleName)
    {
        // Set which paddle it wants
        Paddle thePaddle;
        if(paddleName == Paddles.P1) thePaddle = P1;
        else thePaddle = P2;

        if(gameAxis == Axis.y)
        {
            Vector3 pos = thePaddle.transform.position;
            float paddleHalfHeight = thePaddle.height / 2;
            return new Vector2(pos.x, pos.y + paddleHalfHeight);
        }
        else
        {
            Vector3 pos = thePaddle.transform.position;
            float paddleHalfHeight = thePaddle.height / 2;

            if(thePaddle == P1)
                return new Vector2(pos.x + paddleHalfHeight, pos.y);
            else
                return new Vector2(pos.x - paddleHalfHeight, pos.y);
        }
    }
*/
    //public float getPaddleHeight(){return paddleHeight;}
}
