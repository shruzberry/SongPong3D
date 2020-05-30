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
    public float currentBeat;
    [HideInInspector]
    public float songTime;

    [HideInInspector]
    public int numBeats;
    [HideInInspector]
    public float songLength;

    private AudioSource source;
    private BallDropper ballDropper;
    private int startTime;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* STATIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float ToBeat(float time)
    {
        return (float)((time / 60.0f) * songData.bpm);
    }

    public float ToTime(float beat)
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

        ballDropper.ballMapName = newSongData.name;
    }

    public void Play(){source.Play();}
    
    public void Pause(){source.Pause();}

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

    public float GetSongTime()
    {
        return source.time;
    }


/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void goToTime(float time)
    {
        print("jumping to time: " + time);
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
        ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        source.clip = songData.song;
        songLength = source.clip.length;
        numBeats = (int)((songLength / 60.0f) * songData.bpm);
    }

    void Start()
    {
        LoadSong(songData);
        JumpToStart();
    }

    void Update()
    {
        currentBeat = ToBeat(songTime);
        songTime = source.time;
    }
}
