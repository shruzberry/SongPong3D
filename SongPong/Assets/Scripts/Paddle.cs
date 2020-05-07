using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Paddle : MonoBehaviour
{
    //____________REFERENCES_______________
    public SpawnInfo spawner;
    public Axis paddleAxis;
    public Vector2 screenBounds;

    //____________MOVEMENT_________________
    public Vector2 movement;
    private float paddleYAxis = 4;
    private float paddleXAxis = 8;

    //____________ATTRIBUTES_______________
    public enum Paddles{P1,P2}; // which paddle is this (P1 is left-side in x-axis mode)
    public Paddles paddleNum;
    private float paddleRadius; // the half-width or radius of the paddle
    private float paddleHeight;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Awake(){
        spawner = GameObject.Find("Spawner").GetComponent<SpawnInfo>();
        paddleAxis = spawner.gameAxis;

        paddleRadius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        paddleHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords

        if(paddleAxis == Axis.y){InitializePaddleYAxis();}
        else if(paddleAxis == Axis.x){InitializePaddleXAxis();}
    }

    private void InitializePaddleYAxis()
    {
        if(paddleNum == Paddles.P1)
        {
            paddleYAxis = -paddleYAxis;
            transform.eulerAngles = new Vector3(0,0,0);
        }
        else if(paddleNum == Paddles.P2)
        {
            transform.eulerAngles = new Vector3(0,0,180);
        }
        else{ Debug.LogError("Paddle Number not set.");}

        transform.position = new Vector2(0, paddleYAxis);
    }

    private void InitializePaddleXAxis()
    {
        if(paddleNum == Paddles.P1)
        {
            paddleXAxis = -paddleXAxis;
            transform.eulerAngles = new Vector3(0,0,-90);
        }
        else if(paddleNum == Paddles.P2)
        {
            transform.eulerAngles = new Vector3(0,0,90);
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Update()
    {
        movePaddle();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * MOVE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

   private void movePaddle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(paddleAxis == Axis.y)
        {
            Vector2 paddlePos = new Vector2(mousePos.x, paddleYAxis); // set paddle pos

            paddlePos.x = Mathf.Clamp(paddlePos.x, -screenBounds.x + paddleRadius, screenBounds.x - paddleRadius); // check x boundary

            transform.position = paddlePos;
        }
        else if(paddleAxis == Axis.x)
        {
            Vector2 paddlePos = new Vector2(paddleXAxis, mousePos.y);

            paddlePos.y = Mathf.Clamp(paddlePos.y, -screenBounds.y + paddleRadius, screenBounds.y - paddleRadius);

            transform.position = paddlePos;
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float getPaddleAxis()
    {
        if(paddleAxis == Axis.y)
            return paddleYAxis;
        else if(paddleAxis == Axis.x)
            return paddleXAxis;
        else{
            Debug.LogError("INVALID PADDLE AXIS DETECTED.");
            return 0;
        }

    }
    public float getPaddleRadius(){return paddleRadius;}
    public float getPaddleHeight(){return paddleHeight;}
}
