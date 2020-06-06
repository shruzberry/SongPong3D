using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAxis : MonoBehaviour
{
    public AxisManager axisManager;
    public GameObject paddle1;
    public GameObject paddle2;

    public float padding;

    void Awake()
    {
        axisManager = GameObject.Find("Game").GetComponent<AxisManager>();

        switch (axisManager.gameAxis)
        {
            case Axis.x:
                InitAxisX();
                break;

            case Axis.y:
                InitAxisY();
                break;
        }
    }

    void InitAxisX()
    {
        PaddleMover pm1 = paddle1.GetComponent<PaddleMover>();
        PaddleMover pm2 = paddle2.GetComponent<PaddleMover>();

        pm1.paddleAxis = new Vector2(0, 1);
        pm2.paddleAxis = new Vector2(0, 1);

        pm1.axisValue = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * padding, 0, 0)).x;
        pm2.axisValue = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * (1.0f - padding), 0, 0)).x;;

        pm1.transform.Rotate(0,0,-90);
        pm2.transform.Rotate(0,0,90);

        //paddle1.transform.eulerAngles.z = -90;
    }

    void InitAxisY()
    {
        paddle2.SetActive(false);

        PaddleMover pm1 = paddle1.GetComponent<PaddleMover>();

        pm1.paddleAxis = new Vector2(1, 0);
        pm1.axisValue = Camera.main.ScreenToWorldPoint(new Vector3(0, (Screen.height * padding), 0)).y + pm1.height;
    }
}
