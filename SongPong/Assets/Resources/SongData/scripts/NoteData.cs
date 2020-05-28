using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenuAttribute(fileName="note", menuName="Note")]
public class NoteData : ScriptableObject
{
    public int hitPosition;
    public float hitBeat;
    [HideInInspector]
    public float hitTime;
    public Direction noteDirection;
    private SongController songController;
    
    public static int CompareNotesByHitTime(NoteData a, NoteData b)
    {
        return a.hitTime.CompareTo(b.hitTime);
    }

    void OnEnable()
    {
        songController = GameObject.Find("SongController").GetComponent<SongController>();
        hitTime = songController.ToTime(hitBeat);
    }
}
