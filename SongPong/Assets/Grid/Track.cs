using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    private MeshRenderer mesh_renderer;
    private Vector3 size;

    private float width;
    private float height;

    public float padding;
    public float paddingFallAxis;

    private Vector3 position;
    private float bottom;
    private float top;
    private float left;
    private float right;

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Initialize()
    {
        mesh_renderer = GetComponentInChildren<MeshRenderer>();
        position = transform.position;
        size = mesh_renderer.bounds.size;
        width = size.x;
        height = size.z;
        top = transform.position.z + (height / 2);
        bottom = transform.position.z - (height / 2);
        left = transform.position.x - (width / 2);
        right = transform.position.x + (width / 2);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public Vector2 GetBoundsFallAxis()
    {
        float bottom = position.z - (height/2) + paddingFallAxis;
        float top = position.z + (height/2) - paddingFallAxis;
        return new Vector2(bottom, top);
    }
    
    public float GetWidth()
    {
        return width;
    }

    public float GetHeight()
    {
        return height;
    }

    public float GetBottom()
    {
        return bottom;
    }

    public float GetTop()
    {
        return top;
    }

    public float GetLeft()
    {
        return left;
    }

    public float GetRight()
    {
        return right;
    }

    public Vector3 GetSize()
    {
        return size;
    }

}
