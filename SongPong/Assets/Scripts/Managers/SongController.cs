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

    public SongData songData;
    //[HideInInspector]
    public int currentBeat;
    [HideInInspector]
    public float songTime;

    [HideInInspector]
    public int numBeats;
    [HideInInspector]
    public float songLength;

    private AudioSource source;
    private int startTime;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* STATIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public int ToBeat(float time)
    {
        return (int)((time / 60) * songData.bpm);
    }

    public float ToTime(int beat)
    {
        return (60.0f / songData.bpm) * (beat);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void LoadSong(SongData newSongData)
    {
        songData = newSongData;
        source.clip = songData.song;
        songLength = source.clip.length;
        numBeats = (int)((songLength / 60.0f) * songData.bpm);
        startTime = newSongData.startBeat;
    }

    public void Play(){source.Play();}
    
    public void Pause(){source.Pause();}

    public void UpdateSongTime()
    {
        goToTime(songData.currentTime);
    }

    public void JumpToStart()
    {
        JumpToBeat(songData.startBeat + (ToBeat(songData.offset)));
    }

    public void JumpToBeat(int beat)
    {
        goToTime(
            ((float)(beat * 60)) / songData.bpm
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


/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void goToTime(float time)
    {
        source.time = time;
        songTime = source.time;
        source.Play();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = songData.song;
        songLength = source.clip.length;
        numBeats = (int)((songLength / 60.0f) * songData.bpm);
    }

    void Start()
    {
        LoadSong(songData);
        JumpToBeat(startTime);
    }

    void Update()
    {
        currentBeat = (int)((source.time / 60.0f) * songData.bpm);
        songTime = source.time;
    }
}
