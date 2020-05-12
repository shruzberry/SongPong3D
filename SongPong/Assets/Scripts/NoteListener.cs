﻿/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
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
+ Enable()
+ Disable()
+ Clear()
+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteListener : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public List<NoteData> data;

    private bool enabled;
    private SongController sc;
    private SpawnInfo si;
    
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Enable(){enabled = true;}

    public void Disable(){enabled = false;}

    public void Clear(){data.Clear();}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Awake()
    {
        sc = GameObject.Find("SongController").GetComponent<SongController>();
        si = GameObject.Find("Spawner").GetComponent<SpawnInfo>();
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            NoteData nd = new NoteData();
            
            nd.hitPosition = si.GetNearestColumn(Input.mousePosition);
            nd.hitTime = sc.currentBeat;
            nd.noteDirection = positive;

            data.Add(nd);
        }
    }
}