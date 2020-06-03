using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp : MonoBehaviour
{
    private Vector3 screenBounds;
    private Vector2 clampAxis;
    /**
     * AxisCalc
     * Take in Vector2 as position
     * Takes in which edge you want it to clamp to
     * Returns Vector2 with the clamped version of what you passed in
     * 
     * Include Radius in overload version?
     * Include Dimensions as parameters
     */

    private void Awake() 
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords
    }

    /**
     * Vector2 clampAxis : the vector that is the axis the paddle will move on
     * float min : the lowest value possible
     * float max : the hightest value possible
     */
    public static Vector2 ClampToAxis(Vector2 position, Vector2 clampAxis, float min, float max)
    {
        // such as (0,1)
        // moves along the y-axis, up and down
        if(clampAxis.x == 0)
        {
            position.y = Mathf.Clamp(position.y, min, max);
        }
        // such as (1,0)
        // moves along the x-axis, sideways
        else
            position.x = Mathf.Clamp(position.x, min, max);

        return position;
    }

}
