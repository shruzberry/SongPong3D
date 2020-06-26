using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{
    //__________AXIS__________________
    [Header("AXIS")]
    public Game game;
    [Range(0,45)]
    [Tooltip("The percent(%) away from the edge of screen the paddle's axis is.")]
    public int padding; // percent

    //__________PADDLE________________
    [Header("PADDLES")]
    public GameObject paddle1;
    public GameObject paddle2;

    public void Activate()
    {
        game = FindObjectOfType<Game>();
        if(game.gameAxis == Axis.x) InitAxisX();
        else if(game.gameAxis == Axis.y) InitAxisY();
    }

    void InitAxisX()
    {
        PaddleMover pm1 = paddle1.GetComponent<PaddleMover>();
        PaddleMover pm2 = paddle2.GetComponent<PaddleMover>();

        float axisValue = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - (Screen.width * (padding/100.0f)),0,0)).x;

        pm1.paddleAxis = new Vector2(-1,0) * axisValue;
        pm2.paddleAxis = new Vector2(1,0) * axisValue;

// NOTE investigate rotate vs. euler angles
        pm1.transform.Rotate(0,0,-90);
        pm2.transform.Rotate(0,0,90);

        //paddle1.transform.eulerAngles.z = -90;
    }

    void InitAxisY()
    {
        float axisValue = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - (Screen.height * (padding/100.0f)), 0)).y;
        Debug.Log("AXIS VALUE: " + axisValue);

        // PADDLE 2
        PaddleMover pm2 = paddle2.GetComponent<PaddleMover>();
        pm2.paddleAxis = new Vector2(0,-1) * axisValue;
        
        // PADDLE 1
        paddle1.SetActive(false);
    }
}
