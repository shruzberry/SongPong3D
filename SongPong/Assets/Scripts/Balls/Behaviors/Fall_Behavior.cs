using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Behavior : MoveBehavior
{
    public static float CalcMoveTime(float radius, Vector2 pointA, Vector2 pointB, Vector2 axisVector, float speed, float gravity)
    {
        if(radius == 0)
        {
            Debug.LogError("Radius is zero. Move Time will be imprecise.");
        }
        // ---------- DEBUG Speed and Gravity ---------------------------------
        /*
        Debug.Log("<><><><><><><><><><><><><><><><><>");
        Debug.Log("Speed: " + speed);
        Debug.Log("Gravity: " + gravity);
        Debug.Log("<><><><><><><><><><><><><><><><><>");
        */

        // ---------- DEBUG Delta ---------------------------------
        /*
        Debug.Log("<><><><><><><><><><><><><><><><><>");
        Debug.Log("Point A: " + pointA);
        Debug.Log("Point B: " + pointB);
        Debug.Log("Radius: " + radius);
        Debug.Log("Delta: " + delta);
        Debug.Log("<><><><><><><><><><><><><><><><><>");
        */
        
        //Debug.Log("RAD: " + radius);
        // Calculate delta H
        // |(AB • axisVector)| - ballradius
        float delta = Mathf.Abs(Vector2.Dot(pointA - pointB, axisVector)) - (radius);

        if(delta < 0)
        {
            Debug.LogError("Delta is negative. Size too large.");
        }

        // Calculate delta T using physics equation dy = v0t + 1/2at^2 solved for time in the form
        // t = (-v0 +- sqrt(v0^2 + 2*a*dy)) / a
        float determinant = (Mathf.Pow(speed, 2) + (2 * gravity * delta));
        float time = (-speed + Mathf.Sqrt(determinant)) / gravity;

        if(float.IsNaN(time)) Debug.LogError("TIME IS NAN.");
        return time;
    }
}
