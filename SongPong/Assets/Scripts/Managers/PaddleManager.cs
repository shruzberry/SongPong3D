using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class PaddleManager : MonoBehaviour
{
    AxisManager axisManager;
    Axis gameAxis;

    public GameObject paddlePrefab;
    public Paddle P1;
    public Paddle P2;
    private List<Paddle> paddles = new List<Paddle>();
    private float paddleYAxis;
    private float paddleXAxis;
    private Vector2 paddleAxis_y;
    private Vector2 paddleAxis_x;
    private float paddleRadius;
    private float paddleHeight;

    public Vector2 screenBounds;

    public void Enable() 
    {
        axisManager = FindObjectOfType<AxisManager>();
        paddlePrefab = Resources.Load("Prefabs/Paddle") as GameObject;

        paddleRadius = paddlePrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        paddleHeight = paddlePrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords

        gameAxis = axisManager.gameAxis;
        if(gameAxis == Axis.x) InitializePaddlesX();
        else if(gameAxis == Axis.y) InitializePaddleY();
    }

    public void InitializePaddlesX()
    {
        // Calculate the paddle axis location
        paddleXAxis = screenBounds.x - (screenBounds.x * 0.1f);
        paddleAxis_x = new Vector2(screenBounds.x - (screenBounds.x * 0.1f), 0);

        // SET BOUNDS
        float[] P1bounds = {-paddleXAxis, -paddleXAxis, -screenBounds.y + paddleRadius, screenBounds.y - paddleRadius};
        float[] P2bounds = {paddleXAxis, paddleXAxis, -screenBounds.y + paddleRadius, screenBounds.y - paddleRadius};

        // SPAWN P1
        P1 = Instantiate(paddlePrefab, new Vector2(-paddleXAxis, 0), Quaternion.identity).GetComponent<Paddle>();
        P1.Init(Paddles.P1, P1bounds);
        P1.gameObject.name = "P1";
        P1.transform.parent = this.gameObject.transform;
        P1.transform.eulerAngles = new Vector3(0,0,-90);

        // SPAWN P2
        P2 = Instantiate(paddlePrefab, new Vector2(paddleXAxis, 0), Quaternion.identity).GetComponent<Paddle>();
        P2.Init(Paddles.P2, P2bounds);
        P2.gameObject.name = "P2";
        P2.transform.eulerAngles = new Vector3(0,0,90);
        P2.transform.parent = this.gameObject.transform;
    }

    /**
     * In this version, balls will fall from the top of the screen and there will only be one paddle
     **/
    public void InitializePaddleY()
    {
        // Calculate the paddle axis location
        paddleYAxis = -screenBounds.y + (screenBounds.y * 0.15f);
        paddleAxis_y = new Vector2(0, -screenBounds.y + (screenBounds.y * 0.15f));

        // Set bounds for the paddle
        float[] P1bounds = {-screenBounds.x + paddleRadius, screenBounds.x - paddleRadius, paddleYAxis, paddleYAxis};

        P1 = Instantiate(paddlePrefab).GetComponent<Paddle>();
        P1.Init(Paddles.P1, P1bounds);
        P1.transform.parent = this.gameObject.transform;
        P1.transform.position = new Vector2(0, paddleYAxis);
        P1.transform.eulerAngles = new Vector3(0,0,0);
    }

    public Vector2 GetPaddleAxis()
    {
        if(gameAxis == Axis.y)
            return paddleAxis_y;
        else if(gameAxis == Axis.x)
            return paddleAxis_x;
        else{
            Debug.LogError("INVALID AXIS DETECTED.");
            return Vector2.zero;
        }
    }

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

    public float getPaddleHeight(){return paddleHeight;}
}
