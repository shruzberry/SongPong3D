/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=

________ DEFENITION ________
Class Name: ScreenManager.cs
Purpose: Translate real world position to screen position based on certain
         Parameters

________ USAGE ________
* Attach to GameObject

________ PUBLIC ________
+ float screenWidth
+ float screenHeight
+ Vector3 screenBounds

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float screenWidth;
    public float screenHeight;
    public Vector3 screenBounds;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // in world coords
        Debug.Log("SC: " + screenBounds);
    }

    void Update()
    {
        
    }
}
