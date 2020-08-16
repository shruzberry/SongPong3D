using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Purpose: Manages song navigation using beats rather than seconds
 */
public class SongController : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    //_____ SETTINGS ____________________
    [Tooltip("The number of seconds to fast-forward or rewind")]
    public float skipIncrement = 1;

    //_____ REFERENCES __________________
    private BallDropper ballDropper;
    private InputMaster input;

    //_____ COMPONENTS __________________
    public SongData songData;
    private AudioSource source;

    //_____ ATTRIBUTES __________________
    private string songName;
    [HideInInspector]
    public int numBeats; // total number of beats in this song
    [HideInInspector]
    public float songLengthSeconds;
    private int startTime; // the time the song's audio should start

    //_____ STATE  _______________________
    [HideInInspector]
    public bool isLoaded = false;
    [HideInInspector]
    public bool isPlaying;
    public bool hasStarted;
    public float returnToMenuConst = 2.0f;

    //_____ OTHER _______________________
    private float waitSec; // number of seconds to wait before starting playing the song

    //_____ EVENTS _______________________
    public delegate void OnSongFastForward();
    public event OnSongFastForward onSongFastForward;
    
    public delegate void OnSongRewind();
    public event OnSongRewind onSongRewind;

    public delegate void OnSongEnd();
    public event OnSongEnd onSongEnd;

    public delegate void OnSceneEnd();
    public event OnSceneEnd onSceneEnd;


/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* INITIALIZE
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Initialize(InputMaster inputMaster)
    {
        this.input = inputMaster;
        source = GetComponent<AudioSource>();
    }

    public void LoadSong(SongData newSongData)
    {
        // SONG INFO
        songData = newSongData;
        songName = songData.songName;
        source.clip = songData.song; // which audio file
        songLengthSeconds = source.clip.length; // how long the song is
        numBeats = (int)((songLengthSeconds / 60.0f) * songData.bpm);
        startTime = newSongData.startBeat;

        isLoaded = true;
        hasStarted = false;

        JumpToStart();

        // FF and RW controls
        input.Song.FastForward.performed += SkipForward;
        input.Song.Rewind.performed += SkipBackward;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* SONG CONTROLS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Play()
    {
        isPlaying = true;
        hasStarted = true;
        source.Play();
    }

    public void Play(float waitBeats)
    {
        float waitSec = ToTime(waitBeats);
        this.waitSec = waitSec;
        StartCoroutine(WaitThenPlay(waitSec));
    }

    IEnumerator WaitThenPlay(float waitSec)
    {
        yield return new WaitForSeconds(waitSec);
        Play();
    }

    public void Pause()
    {
        isPlaying = false;
        source.Pause();
    }

    public void SkipForward(InputAction.CallbackContext context)
    {
        FastForward(skipIncrement);
    }

    public void SkipBackward(InputAction.CallbackContext context)
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

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Update()
    {
        CheckForEnd();
        CheckForRetMenu();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void CheckForEnd()
    {
        if (isPlaying && (GetSongTimeBeats() > songData.endBeat))
        {
            isPlaying = false;
            if(onSongFastForward != null) 
            {
                onSongEnd();
                Invoke("SendOnSceneEnd", 1.5f);
                source.volume -= 0.01f;
            } 
        }
    }

    public void CheckForRetMenu()
    {
        //if (songPlaying && (ToBeat(source.time) > songData.endBeat + returnToMenuConst)) onSceneEnd();
        
    }

    void SendOnSceneEnd()
    {
        if(onSceneEnd != null) onSceneEnd();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * TIME FUNCTIONS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void GoToTime(float time)
    {
        source.time = time;
    }

    public void JumpToStart()
    {
        GoToTime(ToTime(songData.startBeat) + songData.offset);
    }

    public void JumpToBeat(float beat)
    {
        GoToTime(
            ToTime(beat)
        );
    }

    public void JumpToEnd()
    {
        GoToTime(songData.endBeat);
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
 * CALCULATIONS
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
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float GetBPM()
    {
        return songData.bpm;
    }

    public float GetSongTimeSeconds()
    {
        return source.time - waitSec;
    }

    public float GetSongTimeBeats()
    {
        if(!hasStarted)
        {
            return ToBeat(-waitSec + Time.time);
        }
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
}
