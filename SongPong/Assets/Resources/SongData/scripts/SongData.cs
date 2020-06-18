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
    public Axis axis;
    public int startBeat = 0;
    public int endBeat = 0;
    public float offset = 0;
    [HideInInspector]
    public string myPath;
    //[HideInInspector]
    public string dataPath;

    public List<BallData> GetAllBallData()
    {
        string path = "SongData/data/" + songName + "/Balls/";
        BallData[] ballData = Resources.LoadAll<BallData>(path);
        List<BallData> ballList = new List<BallData>();

        int i=0;
        foreach(BallData bd in ballData)
        {
            ballList.Add(ballData[i]);
            i++;
        }

        ballList.Sort(BallData.CompareBallsBySpawnTime);

        return ballList;
    }

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