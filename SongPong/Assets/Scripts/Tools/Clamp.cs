using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp : MonoBehaviour
{
    /**
     * Vector2 position : the position of the GameObject to Clamp
     * float radius : the radius (bounds/2) of the GameObject
     * Vector2 clampAxis : the vector that is the direction of the axis
     *      (0,1) would be a horizontal axis
     *      (1,0) would be a vertical axis
     * float min : the lowest value possible
     * float max : the hightest value possible
     */
    public static Vector2 ClampToAxis(Vector2 position, float radius, Vector2 axisDir)
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords
        Vector3 normalizedAxisVector = Abs(Vector3.Normalize(axisDir));
        float axisShift = Vector2.Dot(normalizedAxisVector, axisDir);
        //Debug.Log("AXIS: " + axisDir);
        //Debug.Log("NORM: " + normalizedAxisVector);

        // (0,1) moves sideways
        if(normalizedAxisVector == Vector3.up || normalizedAxisVector == -Vector3.up)
        {
            if(axisShift > screenBounds.y)
            {
                Debug.LogError("AXIS SHIFT EXCEEDS SCREEN BOUNDS");
                return position;
            }
            position.x = Mathf.Clamp(position.x, -screenBounds.x + radius, screenBounds.x - radius);
            position.y = axisShift;
        }
        // (1,0) moves up/down
        else if(axisDir == Vector2.right)
        {
            if(axisShift > screenBounds.x)
            {
                Debug.LogError("AXIS SHIFT EXCEEDS SCREEN BOUNDS");
                return position;
            }
            position.x = axisShift;
            position.y = Mathf.Clamp(position.y, -screenBounds.y + radius, screenBounds.y - radius);
        }
        return position;
    }

    /**
     * Returns the absolute value of a vector (all values are positive)
     */
    public static Vector2 Abs (Vector2 vec)
    {
        return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
    }
}
