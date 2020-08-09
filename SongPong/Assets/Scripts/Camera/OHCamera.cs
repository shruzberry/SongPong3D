using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OHCamera : MonoBehaviour
{
    Track track;
    Camera cam;

    // Start is called before the first frame update
    public void Initialize()
    {
        cam = GetComponent<Camera>();
        track = FindObjectOfType<Track>();
        FrameTrack();
    }

    private void FrameTrack()
    {
        // The length the camera will track to frame
        float trackHeight = track.GetHeight();
        float frustrumWidth = trackHeight / cam.aspect;

        // Calculate distance camera should be to fill frustrum with the track
        var distance = frustrumWidth * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        transform.position = new Vector3(0, track.transform.position.y + distance, track.GetMiddle());
        transform.eulerAngles = new Vector3(90,90,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
