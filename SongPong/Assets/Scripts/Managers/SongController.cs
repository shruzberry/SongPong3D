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
using UnityEngine.InputSystem;

public class SongController : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // SONG INFO
    public SongData songData;
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

    // EVENTS
    public delegate void OnSongFastForward();
    public event OnSongFastForward onSongFastForward;
    
    public delegate void OnSongRewind();
    public event OnSongRewind onSongRewind;

    public delegate void OnSongEnd();
    public event OnSongEnd onSongEnd;

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
        //songData.ballList.Sort(BallData.CompareBallsBySpawnTime);

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
        bool success = Skip(sec);
        if(success && onSongFastForward != null) onSongFastForward();
    }

    private void Rewind(float sec)
    {
        bool success = Skip(-sec);
        if(success && onSongRewind != null) onSongRewind();
    }

    /**
     * Atempts to skip by sec seconds. If it fails, returns false.
     * Otherwise returns true
     **/
    private bool Skip(float sec)
    {
        if(source.time + sec < 0) 
        {
            Debug.LogWarning("Reached start of song.");
            source.time = 0;
            return false;
        }
        else if(source.time + sec > songLengthSeconds)
        {
            Debug.LogWarning("Reached end of song.");
            source.time = songLengthSeconds - 1.0f;
            source.Pause();
            return false;
        }
        else
        {
            source.time += sec;
            source.Play();
            return true;
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
        //input.Song.FastForward.performed += SkipForward;
        //input.Song.Rewind.performed += SkipBackward;
    }

    public void Update()
    {
        CheckForEnd();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void goToTime(float time)
    {
        source.time = time;
        source.Play();
    }

    private bool songPlaying = true;
    public void CheckForEnd()
    {
        if (songPlaying && (ToBeat(source.time) > songData.endBeat))
        {
            songPlaying = false;
            if(onSongFastForward != null) onSongEnd(); 

        }
        else if(!songPlaying && (ToBeat(source.time) < songData.endBeat))
        {
            songPlaying = true;
        }
    }
}
