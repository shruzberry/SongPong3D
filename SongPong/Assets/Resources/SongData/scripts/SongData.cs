using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName="Song", menuName="Song")]
public class SongData : ScriptableObject
{
    public float currentTime;
    public int bpm;
    public string name;
    public AudioClip song;
    public int startBeat = 0;
    public int endBeat = 0;
}
