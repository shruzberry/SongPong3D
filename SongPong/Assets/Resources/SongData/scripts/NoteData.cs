using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName="note", menuName="Song Data/Note")]
public class NoteData : ScriptableObject
{
    public Vector2 hitPosition;
    public float hitTime;
}
