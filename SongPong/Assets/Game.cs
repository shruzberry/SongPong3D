using System.Collections.Generic;
using UnityEngine;

public enum GameType {OnePlayer, TwoPlayer};

[ExecuteInEditMode]
public class Game : MonoBehaviour
{
    // PATH
    [Header("Path")]
    public string rootDataPath = "SongData/data/"; // relative to Resources/

    // GAME
    [Header("Game")]
    public GameType gameType;
    public SongData songData;
    private List<BallData> balls;
    private string dataPath;

    // MANAGERS
    [Header("Managers")]
    public Track track;
    public SongController songController;
    public PaddleManager paddleManager;
    public BallDropper ballDropper;

    // EDITOR
    [Header("Editor")]
    [SerializeField]
    private SongData editorSong;

    private void OnEnable() 
    {
        ReloadBallData();
    }

    /**
     * Called when the Game starts
     */
    public bool Initialize(SongData song)
    {
        //OHCamera ohCam = FindObjectOfType<OHCamera>();
        songController = FindObjectOfType<SongController>();
        paddleManager = FindObjectOfType<PaddleManager>();
        track = FindObjectOfType<Track>();
        ballDropper = FindObjectOfType<BallDropper>();

        // ENVIRONMENT
        track.Initialize(this);
        //ohCam.Initialize();

        // LOAD SONG
        this.songData = song;
        songController.LoadSong(song);

        // PADDLES
        paddleManager.Activate();

        // BALLS
        balls = LoadBallData(song.name);
        SortBalls();

        // BALL DROPPER
        ballDropper.Initialize(this, songController);
        ballDropper.Activate();
        ballDropper.ballMapName = song.songName;

        float waitBeats = ballDropper.GetTimeToFallBeats();

        songController.Play(waitBeats);
        //songController.Play();

        return true;
    }

    /**
     * Called by LevelLoader when game is being initialized through the Song Scene
     * Sets the song equal to the song from the SongBuilder editor
     **/
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

    public void SetEditorSong(SongData song)
    {
        editorSong = song;
    }

    public SongData GetEditorSong()
    {
        return editorSong;
    }

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

    public List<BallData> GetBallData()
    {
        return balls;
    }
}
