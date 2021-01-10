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
    public AudioClip song;
    public GameType axis;
    public int startBeat = 0;
    public int endBeat = 0;
    public float offset = 0;
    [HideInInspector]
    public string myPath;
    //[HideInInspector]
    public string dataPath;

    [Header("UI")]
    public Sprite songLogo;

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