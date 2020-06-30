using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenuAttribute(fileName="note", menuName="Note")]
public class NoteData : ScriptableObject
{
    public int hitPosition;
    public float hitBeat;

    public Direction noteDirection = Direction.negative;
    private SongController songController;
    
    public static int CompareNotesByHitTime(NoteData a, NoteData b)
    {
        return a.hitBeat.CompareTo(b.hitBeat);
    }

    public bool CheckValid()
    {
        bool valid = true;

        if(hitBeat == 0) valid = false;
        if(hitPosition < 0) valid = false;

        return valid;
    }
}
