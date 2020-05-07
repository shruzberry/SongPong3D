using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    public float offsetX = 0;
    public float offsetY = 0;
    public bool snapToPaddle = false;

    private float paddleHeight = 110;

    void Update()
    {
        if (snapToPaddle)
        {
            this.transform.position = new Vector3 (Input.mousePosition.x + offsetX, paddleHeight + offsetY, 0);
        }
        else
        {
            this.transform.position = new Vector3 (Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY, 0);
        }
    }
}
