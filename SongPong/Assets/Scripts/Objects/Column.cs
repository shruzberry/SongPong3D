﻿using UnityEngine;

/**
 * The lines that draw to form columns on the track
 */
public class Column : MonoBehaviour
{
    //_____ SETTINGS ____________________
    //_____ REFERENCES __________________
    private Track track;

    //_____ COMPONENTS __________________
    //_____ ATTRIBUTES __________________
    public float width = 1;

    //_____ STATE  ______________________
    //_____ OTHER _______________________

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // Start is called before the first frame update
    public void Initialize(float position)
    {
        track = FindObjectOfType<Track>();
        transform.position = new Vector3(position, 0.3f, 0);
        transform.localScale = new Vector3(width / 100, 1, track.transform.localScale.z);
    }

}
