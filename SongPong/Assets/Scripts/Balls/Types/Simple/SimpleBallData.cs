using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class SimpleBallData : BallData
{
    public string prefabPath = "Prefabs/Balls/SimpleBall";
    private const int minNotes = 1;
    private const int maxNotes = 1;
    public override int MinNotes {get{return minNotes;}}
    public override int MaxNotes {get{return maxNotes;}}

    public override void Initialize(string name)
    {
        this.name = name;
        type = BallTypes.simple;
        notes = new List<NoteData>();
    }
    protected override void SetPrefab()
    {
        prefab = Resources.Load(prefabPath) as GameObject;
    }
}
