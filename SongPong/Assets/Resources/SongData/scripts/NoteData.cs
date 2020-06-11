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
    public Direction noteDirection = Direction.negative;
    private SongController songController;
    
    public static int CompareNotesByHitTime(NoteData a, NoteData b)
    {
        if(a.hitTime == b.hitTime) {throw new System.ArgumentException("contains notes with the same hit time.");}
        return a.hitTime.CompareTo(b.hitTime);
    }

    void OnEnable()
    {
        if (GameObject.Find("SongController"))
        {
            songController = GameObject.Find("SongController").GetComponent<SongController>();
            hitTime = songController.ToTime(hitBeat);
        }
    }
}
