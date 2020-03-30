using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// TODO: create a function similar to the slider function to...
// 1) Load song by name onto the audio player
// 2) Locate the correct song data file for reading (.tsv)
//    - songName_notes.tsv (for the notes)
//    - songName_data.dat (for bpm, deelay, and style)
//    - Put all in folder named songName
// 3) Create Ball prefabs from file data
// 4) Set slider back to start of song

public class Song : MonoBehaviour
{
    // Song Info
    public string notesLocation = "Assets/Resources/Note Data/";
    public string songPath;
    public string notemapName;
    public float songBPM = 1;
    private AudioSource song; // The Song that will be located and changed

    // UI
    public Text display;
    public Slider songSlider;

    // Reader
    public char delimeter = ',';
    public char noteDelimeter = '/';

    // Notes
    private List<Note> notes = new List<Note>();

    // Time
    private float songTime = 0.0f; // Time to change the song to on slider input

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

    // called automatically when slider is changed
    public void setTime(float time)
    {
        if (Input.GetMouseButtonDown(0))
        {// This avoids the audio from crackling as the song and the slider are both updating each other
          song.time = (song.clip.length * time);
          song.Play();
        }
    }
}
