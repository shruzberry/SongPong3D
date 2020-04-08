using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ClickTimer : MonoBehaviour
{
    // Ball Building
    public int ballTypeID = 0;
    private string ballType = "basic";
    private List<int> bounces = new List<int>();
    
    // Song Data
    public string songName;
    public float bpm;
    private int numNotes;
    private int numBalls;
    private int currentBeat;
    private AudioSource song;

    // Column Seettings
    public float percentPadding = .1f;
    public int numCols = 16; 

    // File Writing
    private string notesPath;
    private string ballsPath;
    string[] notes;
    string[] balls;

    // Interface
    public GameObject ballBuilderUi;

    /*********************************
    *       Helper Functions         *
    **********************************/
    
    int GetNextId(string list)
    {// Finds the next available ID in the tsv
        switch (list) //TODO: optimize this later if needed
        {
            case "notes":
                return numNotes;
                break;
            case "balls":
                return numBalls;
                break;
            default:
                return -1;
                break;
        }
    }

    int getNearestColumn(float xPos)
    {
        float padding = (int)(Screen.width * percentPadding);
        float adjustedClickLoc = (xPos - padding) / (Screen.width - padding);
        int col = (int)(Mathf.Round((float)numCols * adjustedClickLoc)) + 1;
        
        if (col < 1)
            col = 1;
        if (col > 16)
            col = 16;

        return col;
    }

    void InitialRead()
    {
        string[] notes = System.IO.File.ReadAllLines(notesPath);
        string[] balls = System.IO.File.ReadAllLines(notesPath);

        numNotes = notes.Length;
        numBalls = balls.Length;
    }

    /*********************************
    *       Ball Builders            *
    **********************************/

    int AddNote()
    {// Adds a note to the song's datasheet
        int nextId;
        int colNum;
        float hitTime;

        nextId = GetNextId("notes");
        colNum = getNearestColumn(Input.mousePosition.x);

        StreamWriter noteWriter = new StreamWriter(notesPath, true);
        noteWriter.WriteLine(nextId + "," + currentBeat + "," + colNum);
        noteWriter.Close();

        numNotes++;

        return nextId;
    }

    void BuildBall(string type)
    {
        string data = "";
        switch (type)
        {
            case "basic":
                data = numBalls + ",basic," + AddNote();
                break;

            case "bounce":
                data = numBalls + ",bounce,";
                foreach (var el in bounces)
                {
                    data += el + "/";
                }
                data = data.Substring(0, data.Length - 1);
                break;
            
            default:
                print("No Ball Type Selected");
                break;
        }
        bounces.Clear();
        
        StreamWriter noteWriter = new StreamWriter(ballsPath, true);
        noteWriter.WriteLine(data);
        noteWriter.Close();

        numBalls++;
    }

    /*********************************
    *       Runtime Functions        *
    **********************************/

    void Awake()
    {
        // Connect Objects
        notesPath = "../SongPong/Assets/Resources/SongData/" + songName + "/" + songName + "_notes" + ".csv";
        ballsPath = "../SongPong/Assets/Resources/SongData/" + songName + "/" + songName + "_balls" + ".csv";
        
        song = GetComponent<AudioSource>();
    }

    void Start()
    {
        InitialRead();
    }

    void Update()
    {
        currentBeat = (int)((song.time / 60.0f) * bpm);

        // Click to start creating ball
        if(Input.GetMouseButtonDown(0))
        {
            BuildBall(ballType);
        }

        // Toggle ball types
        if (Input.GetKeyDown("1"))
        {
            ballBuilderUi.SetActive(false);
            ballType = "null";
            ballTypeID = 0;
        }
        if (Input.GetKeyDown("2"))
        {
            ballBuilderUi.SetActive(true);
            ballType = "basic";
            ballTypeID = 1;
        }
        if (Input.GetKeyDown("3"))
        {
            ballBuilderUi.SetActive(true);
            ballType = "bounce";
            ballTypeID = 2;
        }

        // Confirm Ball
        if (Input.GetMouseButtonDown(1))
        {
            bounces.Add(AddNote());
        }
    }
}