﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;
using System;
using UnityEngine.Events;

[CreateAssetMenuAttribute(fileName="Ball", menuName="Ball")]
public class BallData : ScriptableObject
{
    [HideInInspector]
    public int id;
    public bool enabled = true;
    public BallTypes type;
    public NoteData[] notes;
    [HideInInspector]
    public GameObject prefab;

    public delegate void OnBallValidate();
    public event OnBallValidate onBallValidate;
    
    public void OnEnable()
    {
        SetPrefab();
    }

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

    private void SetPrefab()
    {
        switch(type)
        {
            case BallTypes.simple:
                prefab = Resources.Load("Prefabs/SimpleBall") as GameObject;
                break;
            case BallTypes.bounce:
                prefab = Resources.Load("Prefabs/BounceBall") as GameObject;
                break;
            default:
                break;
        }
    }

    /**
     * Sort this balls' notes according to their hit time
     */
    protected void SortNotes(NoteData[] notes)
    {
        try
        {
            List<NoteData> noteList = new List<NoteData>();
            foreach(NoteData nd in notes)
            {
                // If a note is null, don't need to keep sorting.
                if(nd == null)
                {
                    Debug.LogWarning("Ball \"" + name + "\" has null notes."); 
                    
                }
                noteList.Add(nd);
            }

            if(noteList.Count > 0)
            {
                noteList.Sort(NoteData.CompareNotesByHitTime);
            }
            this.notes = noteList.ToArray();
        }
        catch (Exception)
        {
            Debug.LogError("Ball \"" + name + "\" has one or more invalid notes.");
        }
    }

    public static int CompareBallsBySpawnTime(BallData a, BallData b)
    {
        return a.notes[0].hitTime.CompareTo(b.notes[0].hitTime);
    }
}