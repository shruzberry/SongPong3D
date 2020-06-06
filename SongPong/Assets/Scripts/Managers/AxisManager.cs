using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisManager : MonoBehaviour
{
    //_______AXIS________________
    private Axis defaultAxis; // stores the axis set at the start so if the axis changes, it triggers recalculation
    public Axis gameAxis;
    
    public Vector3 screenBounds;
    public Vector2 paddleAxis_x;
    public Vector2 paddleAxis_y;

    public Vector2 GetPaddleAxis()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords

        paddleAxis_x = new Vector2(screenBounds.x - (screenBounds.x * 0.1f), 0);
        paddleAxis_y = new Vector2(0, -screenBounds.y + (screenBounds.y * 0.15f));

        if(gameAxis == Axis.y)
            return paddleAxis_y;
        else if(gameAxis == Axis.x)
            return paddleAxis_x;
        else{
            Debug.LogError("INVALID AXIS DETECTED.");
            return Vector2.zero;
        }
    }
}
