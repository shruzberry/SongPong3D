using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;
using System;
using UnityEngine.Events;

[CreateAssetMenuAttribute(fileName="Ball", menuName="Ball")]
public abstract class BallData : ScriptableObject
{
    [HideInInspector]
    public int id;
    public bool enabled = true;
    public BallTypes type;
    public List<NoteData> notes;
    [HideInInspector]
    public GameObject prefab;
    public string baseBallPath;

    [SerializeField]
    public List<BallOption> options;

    public abstract int MinNotes{get;}
    public abstract int MaxNotes{get;}

    //[HideInInspector]
    public float activity;
    
    public virtual void OnEnable()
    {
        SetPrefab();
    }

    public abstract void Initialize(string name);

    public void SetName()
    {
        this.name = type + "Ball" + id;
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

    public void OnValidate() 
    {
        SortNotes();
    }

    protected abstract void SetPrefab();

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

    public float GetOption(string name)
    {
        return options.Find(x => x.opt_name == name).value;
    }

    public void PulseActive()
    {
        activity = 100.0f;
    }
}