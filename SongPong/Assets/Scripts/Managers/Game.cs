using System.Collections.Generic;
using UnityEngine;

public enum GameType {OnePlayer, TwoPlayer};

[ExecuteInEditMode]
public class Game : MonoBehaviour
{
    //_____ SETTINGS ____________________
    [Header("Path")]
    public string rootDataPath = "SongData/data/"; // relative to Resources/
    private string dataPath;

    [Header("Game")]
    public GameType gameType;

    //_____ REFERENCES __________________
    private InputMaster inputMaster;
    
    //_____ COMPONENTS __________________
    [Header("Song")]
    [SerializeField]
    private SongData editorSong;
    public SongData songData;

    [Header("Managers")]
    public Track track;
    public SongController songController;
    public PaddleManager paddleManager;
    public BallDropper ballDropper;
    private List<BallData> balls;

    //_____ ATTRIBUTES __________________
    private float startTime;

    //_____ STATE  _______________________
    //_____ OTHER _______________________

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void OnEnable() 
    {
        ReloadBallData();
    }

    /**
     * Called when the Game starts
     */
    public bool Initialize(SongData song)
    {
        startTime = Time.time;

        //OHCamera ohCam = FindObjectOfType<OHCamera>();
        inputMaster = FindObjectOfType<InputHandler>().inputMaster;
        songController = FindObjectOfType<SongController>();
        paddleManager = FindObjectOfType<PaddleManager>();
        track = FindObjectOfType<Track>();
        ballDropper = FindObjectOfType<BallDropper>();

        // ENVIRONMENT
        track.Initialize(this);
        //ohCam.Initialize();

        // LOAD SONG
        this.songData = song;
        songController.Initialize(inputMaster);
        songController.LoadSong(song);

        // PADDLES
        paddleManager.Initialize(this, track);

        // BALLS
        balls = LoadBallData(song.name);
        SortBalls();

        // BALL DROPPER
        ballDropper.Initialize(this, songController, track);
        ballDropper.ballMapName = song.songName;

        float waitBeats = ballDropper.GetTimeToFallBeats();

        songController.Play(waitBeats);
        //songController.Play();

        return true;
    }

    /**
     * Called by LevelLoader when game is being initialized through the Song Scene
     * Sets the song equal to the song from the SongBuilder editor
     */
    public bool InitializeEditor()
    {
        if(editorSong != null)
        {
            Initialize(editorSong);
            return true;
        }
        else
        {
            Debug.LogWarning("No editor song set. Use SongBuilder to fix this issue.");
            return false;
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * LOADING
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Called by SongBuilder to reload ball data files when the song gets changed
     **/
    public void ReloadBallData()
    {
        if(editorSong != null){
            balls = LoadBallData(editorSong.name);
            SortBalls();
        }
    }

    /**
     * Loads all BallData ScriptableObjects from the Resources file for the song
     * and stores them into the Game's list of Balls
     **/
    public List<BallData> LoadBallData(string songName)
    {
        string path = rootDataPath + songName + "/Balls/";

        BallData[] ballData = Resources.LoadAll<BallData>(path);
        List<BallData> balls = new List<BallData>();

        foreach(BallData bd in ballData)
        {
            balls.Add(bd);
        }
        return balls;
    }

    /**
     * Sort balls by their first hit time
     **/
    public void SortBalls()
    {
        balls.Sort(BallData.CompareBallsBySpawnTime);
    }

    public void SortNotes()
    {
        foreach(BallData ball in balls)
        {
            ball.SortNotes();
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void SetEditorSong(SongData song)
    {
        editorSong = song;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public List<BallData> GetBallData()
    {
        return balls;
    }

    public SongData GetEditorSong()
    {
        return editorSong;
    }

    public float GetGameTime()
    {
        return Time.time - startTime;
    }
}
