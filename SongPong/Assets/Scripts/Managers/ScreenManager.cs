using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public float screenWidth;
    public float screenHeight;
    public Vector3 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords
        Debug.Log("SC: " + screenBounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
