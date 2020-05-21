using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenuAttribute(fileName="note", menuName="Note")]
public class NoteData : ScriptableObject
{
    public int hitPosition;
    public float hitTime;
    public Direction noteDirection;

    public static int CompareNotesByHitTime(NoteData a, NoteData b)
    {
        return a.hitTime.CompareTo(b.hitTime);
    }
}
