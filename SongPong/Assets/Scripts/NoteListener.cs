/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: NoteListener.cs
Purpose: A toggleable tool that listens for clicks on the screen and
adds them to a list as notes

________ USAGE ________
* Create an instance of NoteListener
* Call NoteListener.Enable();
* Click around the game view at different times
* Access NoteListener.data;

________ ATTRIBUTES ________
+ NoteData data

________ FUNCTIONS ________
+ ToString()
+ Enable()
+ Disable()
+ Clear()
+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class NoteListener : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public List<Note> data;

    public bool enabled;
    private SongController sc;
    private SpawnInfo si;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* Constructor
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    
    public NoteListener()
    {
        //Note n = new Note(0, 0.0f, 0);
        data = new List<Note>();
        //data.Add(n);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Enable(){enabled = true;}

    public void Disable(){enabled = false;}

    public void Clear(){data.Clear();}

    public string ToString()
    {
        string str = "";

        foreach (Note n in data)
        {
            str += "(col: " + n.column + ", beat: " + n.hitTime + ")";
            str += "\n";
        }

        if (str == "")
            str = "No Notes Loaded";

        return str;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Awake()
    {
        sc = GameObject.Find("SongController").GetComponent<SongController>();
        si = GameObject.Find("Spawner").GetComponent<SpawnInfo>();

        data = new List<Note>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown("space") && enabled)
        {
            int col = si.GetNearestColumn(Input.mousePosition, true);
            float time = sc.currentBeat;

            data.Add(new Note(99, time, col)); //id
            print("adding ball: " + col + ", " + time);
        }
    }
}