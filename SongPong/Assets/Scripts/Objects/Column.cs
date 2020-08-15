using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    private Track track;

    public float width = 1;

    // Start is called before the first frame update
    public void Initialize(float position)
    {
        track = FindObjectOfType<Track>();
        transform.position = new Vector3(position, 0.3f, 0);
        transform.localScale = new Vector3(width / 100, 1, track.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
