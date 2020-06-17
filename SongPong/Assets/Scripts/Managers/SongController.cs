﻿/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: SongController.cs
Purpose: Manages song navigation using beats rather than seconds
Associations:

________ USAGE ________
* Link a reference to this single instance script
* Call public functions to manipulate the song

________ ATTRIBUTES ________
+ int currentBeat
+ int numBeats
+ float songTime
+ float songLength

________ FUNCTIONS ________
+ UpdateSongTime()
+ LoadSong(SongData)
+ JumpToStart()
+ JumpToBeat(int)
+ JumpToTime(float)
+ JumpToEnd()

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* DEPENDENCIES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SongController : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // SONG INFO
    [SerializeField]
    private SongData songData;
    [HideInInspector]
    public string songName;
    [HideInInspector]
    public int numBeats; // total number of beats in this song
    [HideInInspector]
    public float songLengthSeconds;

    // OTHER
    private int startTime;
    [Tooltip("The number of seconds to fast-forward or rewind")]
    public float skipIncrement = 1;

    // BOOLS
    [HideInInspector]
    public bool isLoaded = false;
    [HideInInspector]
    public bool isPlaying;
    private bool songEndReached;

    // COMPONENTS
    private AudioSource source;

    // REFERENCES
    private BallDropper ballDropper;
    private InputMaster input;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float ToBeat(float time)
    {
        return (float)((time / 60.0f) * songData.bpm);
    }

    public float ToTime(float beat)
    {
        return (60.0f / songData.bpm) * (beat);
    }

    public void LoadSong(SongData newSongData)
    {
        source = GetComponent<AudioSource>();

        // SONG INFO
        songData = newSongData;
        songName = songData.songName;
        source.clip = songData.song; // which audio file
        songLengthSeconds = source.clip.length; // how long the song is
        numBeats = (int)((songLengthSeconds / 60.0f) * songData.bpm);
        startTime = newSongData.startBeat;
        songEndReached = false;

        isLoaded = true;
    }

    public void Play()
    {
        isPlaying = true;
        source.Play();
    }

    public void Pause()
    {
        isPlaying = false;
        source.Pause();
    }

    public void UpdateSongTime()
    {
        goToTime(songData.currentTime);
    }

    public void JumpToStart()
    {
        goToTime(ToTime(songData.startBeat) + songData.offset);
    }

    public void JumpToBeat(float beat)
    {
        goToTime(
            ToTime(beat)
        );
    }

    public void JumpToTime(float time)
    {
        goToTime(time);
    }

    public void JumpToEnd()
    {
        goToTime(songData.endBeat);
    }

    public float GetSongTimeSeconds()
    {
        return source.time;
    }

    public float GetSongTimeBeats()
    {
        source = GetComponent<AudioSource>();
        return ToBeat(source.time);
    }

    public string GetSongName()
    {
        return songName;
    }

    public string GetDataPath()
    {
        return songData.dataPath;
    }

    public SongData GetSongData()
    {
        return songData;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* SONG CONTROLS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void SkipForward(InputAction.CallbackContext context)
    {
        FastForward(skipIncrement);
    }

    private void SkipBackward(InputAction.CallbackContext context)
    {
        Rewind(skipIncrement);
    }

    private void FastForward(float sec)
    {
        Skip(sec);
    }

    private void Rewind(float sec)
    {
        Skip(-sec);
    }

    private void Skip(float sec)
    {
        if(source.time + sec < 0) 
        {
            Debug.LogWarning("Attempted to rewind too far." + source.time + sec);
            source.time = 0;
        }
        else if(source.time + sec > songLengthSeconds)
        {
            Debug.LogWarning("Attempted to fast-forward too far." + source.time + sec);
            source.time = songLengthSeconds - 1.0f;
            source.Pause();
        }
        else
        {
            source.time += sec;
            source.Play();
        }
    }


/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnEnable()
    {
        // INITIALIZE COMPONENTS
        source = GetComponent<AudioSource>();
    }

    private void Start() 
    {
        input = FindObjectOfType<InputHandler>().inputMaster;
        input.Song.FastForward.performed += SkipForward;
        input.Song.Rewind.performed += SkipBackward;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void goToTime(float time)
    {
        source.time = time;
        source.Play();
    }
}
