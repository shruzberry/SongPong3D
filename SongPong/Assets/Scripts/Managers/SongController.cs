/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
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
    public float songTime;
    [HideInInspector]
    public int numBeats; // total number of beats in this song
    [HideInInspector]
    public float songLengthSeconds;

    // OTHER
    private int startTime;

    // BOOLS
    [HideInInspector]
    public bool isLoaded = false;
    [HideInInspector]
    public bool isPlaying;

    // COMPONENTS
    private AudioSource source;

    // REFERENCES
    private BallDropper ballDropper;

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
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnEnable()
    {
        // INITIALIZE COMPONENTS
        source = GetComponent<AudioSource>();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void goToTime(float time)
    {
        source.time = time;
        songTime = source.time;
        source.Play();
    }
}
