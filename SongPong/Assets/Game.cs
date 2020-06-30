using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Game : MonoBehaviour
{
    // PATH
    [Header("Path")]
    public string rootDataPath = "SongData/data/"; // relative to Resources/

    // GAME
    [Header("Game")]
    public Axis gameAxis;
    public SongData songData;
    private List<BallData> balls;
    private string dataPath;

    // EDITOR
    [Header("Editor")]
    public SongData editorSong;

    private void OnEnable() 
    {
        ReloadBallData();
    }

    /**
     * Called when the Game starts
     */
    public void Initialize(SongData song)
    {
        // SONG
        this.songData = song;
        SongController songController = FindObjectOfType<SongController>();
        songController.LoadSong(song);
        songController.JumpToStart();

        // PADDLES
        PaddleManager paddleManager = FindObjectOfType<PaddleManager>();
        paddleManager.Activate();

        // BALLS
        balls = LoadBallData(song.name);
        SortBalls();
        BallDropper ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        ballDropper.Activate();
        ballDropper.ballMapName = song.songName;

        songController.Play();
    }

    /**
     * Called when game is being initialized through the editor
     **/
    public void InitializeEditor()
    {
        Initialize(editorSong);
    }

    public void ReloadBallData()
    {
        balls = LoadBallData(editorSong.name);
    }

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
