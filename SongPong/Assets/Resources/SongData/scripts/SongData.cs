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

    public AudioClip song;

    // SONG METADATA
    public string songName;
    public string artistName;
    public int bpm;

    public GameType axis;
    public int startBeat = 0;
    public int endBeat = 0;
    public float offset = 0;
    [HideInInspector]
    public string myPath;
    //[HideInInspector]
    public string dataPath;

    public void OnEnable()
    {
#if UNITY_EDITOR
        myPath = AssetDatabase.GetAssetPath(this);
        dataPath = GetDataPath();
#endif
    }

    public string GetDataPath()
    {
        string str;
        
        int lastSlashPos = myPath.LastIndexOf("/");
        str = myPath.Substring(0, lastSlashPos);

        return str;
    }
}