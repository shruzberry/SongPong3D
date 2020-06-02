using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenuAttribute(fileName="ballmanager", menuName="BallManager")]
[System.Serializable]
[ExecuteInEditMode]
public class BallManager : ScriptableObject
{
    private int ballID;
    List<BallData> ballDatas = new List<BallData>();
    public int numberOfBalls;

    private void OnValidate()
    {
        Debug.LogWarning("Updating Balls...");
        UpdateBalls();
        SortBalls();
    }

    private void Update() 
    {
        Debug.LogError("UPDATE");
    }

    private void UpdateBalls()
    {
        ballDatas = new List<BallData>(); // reset list
        ballID = 0;

        // load all BallData from the Resources folder, then add to the list
        var loaded = Resources.LoadAll("SongData/data/paradise/Balls/", typeof(BallData));
        foreach(var loadedData in loaded)
        {
            ballDatas.Add(loadedData as BallData);
        }

        numberOfBalls = ballDatas.Count;
        
        // add an event to each ball to check when they update
        foreach(BallData ballData in ballDatas)
        {
            ballData.id = ballID++; // assign id and increment
            ballData.onBallValidate += SortBalls;
        }
    }

    private void SortBalls()
    {
        ballDatas.Sort(BallData.CompareBallsBySpawnTime);
        foreach(BallData bd in ballDatas)
        {
            bd.SetName();
        }
    }
}
