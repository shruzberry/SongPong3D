using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{
    public AxisManager axisManager;
    public GameObject paddle1;
    public GameObject paddle2;

    public float padding;

    public void Activate()
    {
        axisManager = FindObjectOfType<AxisManager>();

        if(axisManager.gameAxis == Axis.x) InitAxisX();
        else if(axisManager.gameAxis == Axis.y) InitAxisY();
    }

    void InitAxisX()
    {
        PaddleMover pm1 = paddle1.GetComponent<PaddleMover>();
        PaddleMover pm2 = paddle2.GetComponent<PaddleMover>();

        float axisValue = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - (Screen.width * padding),0,0)).x;

        pm1.paddleAxis = new Vector2(-1,0) * axisValue;
        pm2.paddleAxis = new Vector2(1,0) * axisValue;

// NOTE investigate rotate vs. euler angles
        pm1.transform.Rotate(0,0,-90);
        pm2.transform.Rotate(0,0,90);

        //paddle1.transform.eulerAngles.z = -90;
    }

    void InitAxisY()
    {
        float axisValue = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height - (Screen.height * padding),0)).y;

        // PADDLE 1
        PaddleMover pm1 = paddle1.GetComponent<PaddleMover>();
        pm1.paddleAxis = new Vector2(0,-1) * axisValue;
        
        // PADDLE 2
        paddle2.SetActive(false);
    }
}
