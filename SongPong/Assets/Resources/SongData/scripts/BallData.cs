using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;
using System;

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
    
    public void OnEnable()
    {
        SetPrefab();
    }

    private void OnValidate() 
    {
        SortNotes(notes);
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
                    break;
                }

                noteList.Add(nd);
            }

            if(noteList.Count > 0)
            {
                noteList.Sort(NoteData.CompareNotesByHitTime);
            }
            this.notes = noteList.ToArray();
        }
        catch(Exception e)
        {
            Debug.LogError("Ball \"" + name + "\" has one or more invalid notes.");
        }
    }
}