using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ClickTimer : MonoBehaviour
{
    // Song Data
    public string songName;
    public float bpm;
    private int numNotes;
    private int numBalls;

    // Interface
    public GameObject ballBuilderUi;

    // Column Seettings
    public float percentPadding = .1f;
    public int numCols = 16; 

    // File Writing
    private string notesPath;
    private string ballsPath;
    string[] notes;
    string[] balls;

    /*********************************
    *       Custom Functions         *
    **********************************/
    
    int GetNextId(string list)
    {// Finds the next available ID in the tsv
        switch (list) //TODO: optimize this later if needed
        {
            case "notes":
                return numNotes + 1;
                break;
            case "balls":
                return numBalls + 1;
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

    void AddNote()
    {// Adds a note to the song's datasheet
        int nextId;
        int colNum;
        float hitTime;

        nextId = GetNextId("notes");
        colNum = getNearestColumn(Input.mousePosition.x);

        /*
        StreamWriter noteWriter = new StreamWriter(notesPath, true);
        noteWriter.WriteLine(nextId + ","); //TODO: implement time and position
        noteWriter.Close();
        */

        numNotes++;
    }

    /*********************************
    *       Runtime Functions        *
    **********************************/

    void Awake()
    {
        // Connect Objects
        notesPath = "../SongPong/Assets/Resources/SongData/" + songName + "/" + songName + "_notes" + ".csv";
        ballsPath = "../SongPong/Assets/Resources/SongData/" + songName + "/" + songName + "_balls" + ".csv";
    }

    void Start()
    {
        InitialRead();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            AddNote();
        }
    }
}
