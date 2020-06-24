using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;
using System;
using UnityEngine.Events;

[CreateAssetMenuAttribute(fileName="Ball", menuName="Ball")]
public abstract class BallDataNew : ScriptableObject
{
    [HideInInspector]
    public int id;
    public bool enabled = true;
    public BallTypes type;
    //public NoteData[] notes;
    public List<NoteData> notes;
    [HideInInspector]
    public GameObject prefab;

    //[HideInInspector]
    public float activity;

    public delegate void OnBallValidate();
    public event OnBallValidate onBallValidate;
    
    public void OnEnable()
    {
        SetPrefab();
    }

    public abstract void Initialize(string name);

    public void SetName()
    {
        this.name = type + "Ball" + id;
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), name);
    }

    private void OnValidate() 
    {
        SortNotes(notes);
        if(onBallValidate != null) onBallValidate();
    }

    protected abstract void SetPrefab();

    /**
     * Sort this balls' notes according to their hit time
     */
    protected void SortNotes(List<NoteData> notes)
    {
        try
        {
            foreach(NoteData nd in notes)
            {
                // If a note is null, don't need to keep sorting.
                if(nd == null)
                {
                    Debug.LogWarning("Ball \"" + name + "\" has null notes."); 
                    
                }
            }

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
        return a.notes[0].hitTime.CompareTo(b.notes[0].hitTime);
    }

    public void PulseActive()
    {
        activity = 100.0f;
    }
}