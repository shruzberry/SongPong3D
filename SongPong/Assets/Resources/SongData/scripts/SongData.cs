﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName="Song", menuName="Song Data/Song")]
public class SongData : ScriptableObject
{
    public float currentTime;
    public int bpm;
    public string name;
    public AudioClip song;
    public float startTime = 0.0f;
    public float endTime = 100.0f;
}