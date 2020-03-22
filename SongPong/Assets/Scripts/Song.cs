using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Song : MonoBehaviour
{
    public string songPath;
    public string notemapPath;

    private List<string> notes = new List<string>();
    private int noteIndex = 0;

    void Start()
    {
        loadSong();
    }

    void Update()
    {
        
    }

    public void readNote(int index){
        string note = notes[index];
        string[] noteinfo = note.Split('\t');
        foreach(string info in noteinfo){
            print(info);
        }
    }

    public void loadSong(){
        StreamReader reader = new StreamReader(notemapPath);
        string currLine = reader.ReadLine(); // this skips the labels row
        while((currLine = reader.ReadLine()) != null){
            notes.Add(currLine);
        }
    }

    public void closeSong(){
    }
}
