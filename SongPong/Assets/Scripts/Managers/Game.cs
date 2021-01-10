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
    public ReplayButton replayButton;
    
    //_____ COMPONENTS __________________
    [Header("Song")]
    [SerializeField]
    private SongData editorSong;
    public SongData songData;

    [Header("Managers")]
    public Track track;
    public UIManager uIManager;
    public SongController songController;
    public PaddleManager paddleManager;
    public BallDropper ballDropper;
    private List<BallData> balls;

    //_____ ATTRIBUTES __________________
    private float startTime;
    private float waitTimeBeats;

    //_____ STATE  _______________________
    public bool isPaused = false;

    public delegate void OnGameStart();
    public event OnGameStart onGameStart;

    public delegate void OnGameEnd();
    public event OnGameEnd onGameEnd;

    public delegate void OnGamePause();
    public event OnGamePause onGamePause;

    public delegate void OnGameResume();
    public event OnGameResume onGameResume;

    public delegate void OnGameRestart();
    public event OnGameRestart onGameRestart;

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
        ResetGameTime();

        //OHCamera ohCam = FindObjectOfType<OHCamera>();
        inputMaster = FindObjectOfType<InputHandler>().inputMaster;
        uIManager = FindObjectOfType<UIManager>();
        songController = FindObjectOfType<SongController>();
        paddleManager = FindObjectOfType<PaddleManager>();
        track = FindObjectOfType<Track>();
        ballDropper = FindObjectOfType<BallDropper>();

        // ENVIRONMENT
        track.Initialize(this);
        //ohCam.Initialize();

        // LOAD SONG
        this.songData = song;
        songController.Initialize(this, inputMaster);
        songController.LoadSong(song);
        songController.onSongEnd += EndGame;

        // PADDLES
        paddleManager.Initialize(this, track);

        // BALLS
        balls = LoadBallData(song.name);
        SortBalls();

        // BALL DROPPER
        ballDropper.Initialize(this, songController, track);
        ballDropper.ballMapName = song.songName;
        waitTimeBeats = ballDropper.GetTimeToFallBeats();

        // UI
        uIManager.Initialize(this, inputMaster);

        StartGame();

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
 * STATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void StartGame()
    {
        songController.StartSong();
        if(onGameStart != null) onGameStart();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        if(onGamePause != null) onGamePause();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        onGameResume?.Invoke();
    }

    public float GetWaitTimeBeats()
    {
        return waitTimeBeats;
    }

    public void RestartGame()
    {
        ResetGameTime();
        if(onGameRestart != null){onGameRestart();}
    }

    public void EndGame()
    {
        if(onGameEnd != null){onGameEnd();}
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

    private void ResetGameTime()
    {
        startTime = Time.time;
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
