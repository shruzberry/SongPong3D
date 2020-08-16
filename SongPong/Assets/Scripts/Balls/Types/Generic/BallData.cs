using System.Collections.Generic;
using UnityEngine;
using Types;
using System;

/**
 *  An abstract Data Type that holds information about a Ball
 */

[CreateAssetMenuAttribute(fileName="Ball", menuName="Ball")]
public abstract class BallData : ScriptableObject
{
    //_____ REFERENCES _________________

    //_____ COMPONENTS ________________
    [HideInInspector]
    public GameObject prefab;
    public List<NoteData> notes;
    
    //_____ ATTRIBUTES __________________
    [HideInInspector]
    public int id; // unique id for the ball

    public BallTypes type; // type of the ball
    public List<BallOption> options;

    public abstract int MinNotes{get;}
    public abstract int MaxNotes{get;}

    //_____ BOOLS _______________________
    public bool enabled = true; // whether the ball should be spawned

    //_____ OTHER _____________________
    private string baseBallPath;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public virtual void OnEnable()
    {
        SetPrefab();
    }

    public abstract void Initialize(string name);

    public void SetName()
    {
        this.name = type + "Ball" + id;
    }

    protected void InitializeOptions()
    {
        if(options == null)
        {
            options = new List<BallOption>();
        }
    }

    protected void InitializeOption(float value, string name)
    {
        if(!options.Exists(x => x.opt_name == name))
        {
            options.Add(new BallOption(value, name));
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * VALIDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/    

    public void OnValidate() 
    {
        SortNotes();
    }

    public virtual bool CheckValid()
    {
        bool valid = true;

        foreach(NoteData note in notes)
        {
            valid = note.CheckValid();
        }
        if(prefab == null)
        {
            Debug.LogError("Ball has null prefab");
            valid = false;
        }

        return valid;
    }

    protected abstract void SetPrefab();

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SORTING
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    /**
     * Sort this balls' notes according to their hit time
     */
    public void SortNotes()
    {
        try
        {
            if(notes.Count > 0)
            {
                notes.Sort(NoteData.CompareNotesByHitTime);
            }
        }
        catch (Exception)
        {
            Debug.LogWarning("Ball \"" + name + "\" has one or more invalid notes.");
        }
    }

    public static int CompareBallsBySpawnTime(BallData a, BallData b)
    {
        //if(a.notes.Count <= 0 || b.notes.Count <= 0) return 0;
        return a.notes[0].hitBeat.CompareTo(b.notes[0].hitBeat);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public float GetOption(string name)
    {
        return options.Find(x => x.opt_name == name).value;
    }

}