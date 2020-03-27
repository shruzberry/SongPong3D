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
    public string songPath;
    public string notemapPath;
    public float songBPM = 1;
    private AudioSource song; // The Song that will be located and changed

    // UI
    public Text display;
    public Slider songSlider;

    // Notes
    private List<string> notes = new List<string>();
    private int noteIndex = 0;

    // Time
    private float songTime = 0.0f; // Time to change the song to on slider input
    private int currentBeat;

    // State
    public bool finishedNotes = false;

    void Awake()
    {
        song = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentBeat = 0;
        loadSong();
    }

    void Update()
    {
        // read out the beat number to thee screen
        currentBeat = (int)((song.time / 60.0f) * songBPM);
        display.text = "Beat: " + currentBeat.ToString();
        //move slider with song
        songSlider.value = (song.time / song.clip.length);
    }

    public string[] readNote(int index){
        string note = notes[index];
        string[] noteinfo = note.Split('\t');
        return noteinfo;
    }

    public float getNextHitTime(){
        string[] note = readNote(currentBeat);
        float spawnTime = float.Parse(note[1]);
        return spawnTime;
    }

    public void nextNote()
    {
        currentBeat++;
    }

    public bool hasNextNote(){
        if(currentBeat < notes.Count - 1){
            return true;
        } else {
            finishedNotes = true;
            return false;
        }
    }

    public void loadSong(){
        StreamReader reader = new StreamReader(notemapPath);
        string currLine = reader.ReadLine(); // this skips the labels row
        while((currLine = reader.ReadLine()) != null){
            notes.Add(currLine);
        }
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
