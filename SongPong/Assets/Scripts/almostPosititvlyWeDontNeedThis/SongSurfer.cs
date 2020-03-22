using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSurfer : MonoBehaviour
{
    public float songBPM = 1;
    public Text display;
    public Slider songSlider;

    private AudioSource song; // The Song that will be located and changed
    private float songTime = 0.0f; // Time to change the song to on slider input
    private int currentBeat;

    void Awake()
    {
        song = GetComponent<AudioSource>();
    }

    void Update()
    {
        // read out the beat number to thee screen
        currentBeat = (int)((song.time / 60.0f) * songBPM);
        display.text = "Beat: " + currentBeat.ToString();
        // move slider with song
        songSlider.value = (song.time / song.clip.length);
    }

    // called automatically when slider is changed
    public void setTime(float time)
    {
        if (Input.GetMouseButtonDown(0))
        {// This avoids the audio from crackling as the song and the slider are both updating each other
          song.time = (song.clip.length * time);
          song.Play();
        }
    }
}
