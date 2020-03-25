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

    // Interface
    public GameObject ballBuilderUi;

    // File Writing
    private string notesPath;
    private StreamWriter noteWriter;
    private int nextId;

    /*********************************
    *       Custom Functions         *
    **********************************/
    
    int GetNextId()
    {// Finds the lowest available ID in the tsv
        return 0;
    }

    void AddNote()
    {// Adds a note to the song's datasheet
        nextId = GetNextId();
        noteWriter.WriteLine(nextId + "\t"); //TODO: implement time and position
    }

    /*********************************
    *       Runtime Functions        *
    **********************************/

    void Awake()
    {
        // Connect Objects
        notesPath = "Assets/Resources/SongData/" + songName + "/" + songName + "_notes" + ".tsv";
        noteWriter = new StreamWriter(notesPath, true);

        nextId = GetNextId();
    }
}
