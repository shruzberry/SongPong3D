using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp : MonoBehaviour
{
    /**
     * Vector2 position : the position of the GameObject to Clamp
     * float radius : the radius (bounds/2) of the GameObject
     * Vector3 clampAxis : the vector that is the direction of the axis
     *      (1,0,0) would clamp on the x axis
     *      (0,1,0) would clamp on the y axis
     *      (0,0,1) would clamp on the z axis
     * float min : the lowest value possible
     * float max : the hightest value possible
     */
    public static Vector3 ClampToAxis(Vector3 position, Vector3 clampAxis, Vector2 bounds)
    {
        clampAxis = Vector3.Normalize(clampAxis);

        // X-AXIS
        // (1,0,0) moves along x-axis (horizontal)
        if(clampAxis == Vector3.right)
        {
            position.x = Mathf.Clamp(position.x, bounds.x, bounds.y);
        }
        else
        {
            Debug.Log("Clamp Axis is not supported.");
        }
        return position;
    }

    /**
     * Returns the absolute value of a vector (all values are positive)
     */
    public static Vector3 Abs (Vector3 vec)
    {
        return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
    }
}
