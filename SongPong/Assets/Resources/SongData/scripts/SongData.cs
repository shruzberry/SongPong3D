using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

[CreateAssetMenuAttribute(fileName="Song", menuName="Song")]
public class SongData : ScriptableObject
{
    [HideInInspector]
    public float currentTime;
    public int bpm;
    public string songName;
    [HideInInspector]
    public string dataPath;
    public AudioClip song;
    public Axis axis;
    public int startBeat = 0;
    public int endBeat = 0;
    public float offset = 0;
    [HideInInspector]
    public string myPath;

    public void OnEnable()
    {
        myPath = AssetDatabase.GetAssetPath(this);
        dataPath = GetDataPath(myPath);
    }

    private string GetDataPath(string myPath)
    {
        string str;
        
        int lastSlashPos = myPath.LastIndexOf("/");
        str = myPath.Substring(0, lastSlashPos);

        return str;
    }
}