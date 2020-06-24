using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class SimpleBallData : BallData
{
    public override void Initialize(string name)
    {
        this.name = name;
        type = BallTypes.simple;
        notes = new List<NoteData>();
    }
    protected override void SetPrefab()
    {
        prefab = Resources.Load("Prefabs/SimpleBall") as GameObject;
    }
}
