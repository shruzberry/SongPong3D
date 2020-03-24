using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    public float offsetX = 0;
    public float offsetY = 0;

    void Update()
    {
        this.transform.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
    }
}
