using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class BounceBallData : BallData
{
    private const int minNotes = 2;
    private const int maxNotes = int.MaxValue;
    public override int MinNotes {get{return minNotes;}}
    public override int MaxNotes {get{return maxNotes;}}

    public override void Initialize(string name)
    {
        this.name = name;
        type = BallTypes.bounce;
        notes = new List<NoteData>();
    }

    protected override void SetPrefab()
    {
        prefab = Resources.Load("Prefabs/BounceBall") as GameObject;
    }
}
