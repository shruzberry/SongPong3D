using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Behavior : MoveBehavior
{
    public static float CalcMoveTime(GameObject ball, Vector2 pointA, Vector2 pointB, Vector2 axisVector, float speed, float gravity)
    {
        //TODO Convert to Collider2D for more accuracy
        float radius = ball.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        if(radius == 0) Debug.LogError("Ball radius is null or zero. Move time will be inaccurate.");
        Debug.Log("RAD: " + radius);
        // Calculate delta H
        // |(AB • axisVector)| - ballradius
        float delta = Mathf.Abs(Vector2.Dot(pointA - pointB, axisVector)) - (radius);

        // Calculate delta T using physics equation dy = v0t + 1/2at^2 solved for time in the form
        // t = (-v0 +- sqrt(v0^2 + 2*a*dy)) / a
        float determinant = (Mathf.Pow(speed, 2) + (2 * gravity * delta));
        float time = (-speed + Mathf.Sqrt(determinant)) / gravity;

        if(float.IsNaN(time)) Debug.LogError("TIME IS NAN.");
        return time;
    }
}
