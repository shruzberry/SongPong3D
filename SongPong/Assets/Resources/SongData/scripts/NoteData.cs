﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenuAttribute(fileName="note", menuName="Song Data/Note")]
public class NoteData : ScriptableObject
{
    public int hitPosition;
    public float hitTime;
    public Direction noteDirection;
}