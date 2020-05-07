using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Song : MonoBehaviour
{
    // Song Info
    public string notesLocation = "Assets/Resources/Note Data/";
    public string songPath;
    public string notemapName;
    public float songBPM = 1;
    private AudioSource song;
    private int currentBeat;

    // UI
    public Text display;
    public Slider songSlider;

    // Reader
    public char delimeter = ',';
    public char noteDelimeter = '/';

    // Notes
    private List<Note> notes = new List<Note>();

    // Time
    private float songTime = 0.0f;

    // State
    public bool finishedNotes = false;

    void Awake()
    {
        song = GetComponent<AudioSource>();
    }

    void Start()
    {
    }

    void Update()
    {
        // read out the beat number to thee screen
        currentBeat = (int)((song.time / 60.0f) * songBPM);
        display.text = "Beat: " + currentBeat.ToString();
        //move slider with song
        songSlider.value = (song.time / song.clip.length);
        // Arrow controlls
        if (Input.GetKeyDown("right"))
            changeTime(5);
        if (Input.GetKeyDown("left"))
            changeTime(-5);


    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * READER
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public void loadSong()
    {
        // Load the Ball Sheet and the Notes Sheet
        string path = notesLocation + notemapName + "/";
        string ballsPath = path + notemapName + "_balls.csv";
        string notesPath = path + notemapName + "_notes.csv";

        StreamReader reader = new StreamReader(notesPath);
        string currLine = reader.ReadLine(); // this skips the labels row

        while((currLine = reader.ReadLine()) != null){
            string[] noteInfo = currLine.Split(delimeter);
            int id = int.Parse(noteInfo[0]);
            float time = float.Parse(noteInfo[1]);
            int col = int.Parse(noteInfo[2]);

            Note n = new Note(id,time,col);
            notes.Add(n);
        }
    }

    public Note getNote(int index){
        return notes[index];
    }

    public List<Note> getNotes(int[] indices){
        List<Note> noteList = new List<Note>();
        foreach(int i in indices){
            noteList.Add(getNote(i));
        }
        return noteList;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * Controls
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    
    public void changeTime(float amount)
    {
        song.time = song.time + amount;
        song.Play();
    }

    // called automatically when slider is changed
    public void setTime(float time)
    {
        if (Input.GetMouseButtonDown(0))
        {// This avoids the audio from crackling as the song and the slider are both updating each other
          song.time = (song.clip.length * time);
          song.Play();
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * Accessor Functions
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float GetSongTime()
    {
        return song.time;
    }
}
