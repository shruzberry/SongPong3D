using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongInfoDisplay : MonoBehaviour
{
    SongController song;
    Text infoText;

    private void Awake() 
    {
        song = FindObjectOfType<SongController>();
        infoText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string name = song.GetSongName();
        if(string.IsNullOrEmpty(name)) infoText.text = "NULL SONG NAME";
        else
        {
            infoText.text = name;
        }
    }
}
