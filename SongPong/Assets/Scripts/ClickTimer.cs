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
    private List<int> vacantBallIds = new List<int>();
    private List<int> vacantNoteIds = new List<int>();
    
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
    private List<string> notes = new List<string>();
    private List<string> balls = new List<string>();

    // Interface
    public GameObject ballBuilderUi;
    public Text ballListUI;
    public Text noteListUI;

    /*********************************
    *       Helper Functions         *
    **********************************/
    
    int GetNextId(string list)
    {// Finds the next available ID in the tsv
        int id = -1;

        switch (list) //TODO: optimize this later if needed
        {
            case "notes":
                if (vacantNoteIds.Count == 0)
                    id = numNotes;
                else
                {
                    vacantNoteIds.Sort();
                    id = vacantNoteIds[0];
                    vacantNoteIds.Remove(0);     
                }
                break;
            case "balls":
                if (vacantBallIds.Count == 0)
                    id = numBalls;
                else
                {
                    vacantBallIds.Sort();
                    id = vacantBallIds[0];
                    vacantBallIds.Remove(0);     
                }
                break;
            default:
                break;
        }

        return id;
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
        System.IO.StreamReader noteFile = new System.IO.StreamReader(notesPath);
        System.IO.StreamReader ballFile = new System.IO.StreamReader(ballsPath);
        string line;

        while((line = noteFile.ReadLine()) != null)
        {
            notes.Add(line);
        }

        while((line = ballFile.ReadLine()) != null)
        {
            balls.Add(line);
        }

        numNotes = notes.Count;
        numBalls = balls.Count;
    }

    void DisplayList(List<string> list, Text t, string title = "Title")
    {
        string str = title + ":\n";
        foreach(string el in list)
        {
            str += el + "\n";
        }
        t.text = str;
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

        notes.Add(nextId + "," + currentBeat + "," + colNum);

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

        balls.Add(data);

        numBalls++;
    }

    void DeleteNoteById(int id)
    {
        notes[id] = "-1,-1,-1";
        vacantNoteIds.Add(id);
        print(notes[id]);
    }

    void DeleteBallById(int id)
    {
        List<int> noteIds = new List<int>();
        string targetBall = balls[id];

        // pulls out the note id section from the ball string
        int nextComma = targetBall.IndexOf(",") + 1;
        string targetNotes = targetBall.Substring(nextComma, targetBall.Length - nextComma);
        nextComma = targetNotes.IndexOf(",") + 1;
        targetNotes = targetNotes.Substring(nextComma, targetNotes.Length - nextComma);
       
        // puts each not id into the noteIds array
        int noteToAdd;
        int slashPos; 
        while (targetNotes.Contains("/"))
        {
            slashPos = targetNotes.IndexOf("/");
            noteToAdd = int.Parse(targetNotes.Substring(0, slashPos));
            noteIds.Add(noteToAdd);
            targetNotes = targetNotes.Substring(slashPos + 1, targetNotes.Length - slashPos - 1);
        }
        slashPos = targetNotes.IndexOf("/");
        noteToAdd = int.Parse(targetNotes.Substring(0, slashPos));  
        noteIds.Add(noteToAdd);

        print("num to delete: " + noteIds.Count);
        // delete all notes
        foreach (int el in noteIds)
        {
            DeleteBallById(el);
        }

        // delete ball iteself and add to vacant array
        balls[id] = "-1,vacant,-1";
        print("set to" + balls[id]);
        vacantBallIds.Add(id);
    }

    void WriteToFile(string path, List<string> list)
    {
        StreamWriter writer = new StreamWriter(path, false);
        foreach (string el in list)
        {
            writer.WriteLine(el);
            print("yeet" + el);
        }
        writer.Close();
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
        DeleteBallById(0);
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

        // Update UIs
        DisplayList(balls, ballListUI, "Balls");
        DisplayList(notes, noteListUI, "Notes");
    }

    void OnApplicationQuit()
    {
        WriteToFile(notesPath, notes);
        WriteToFile(ballsPath, balls);
    }
}