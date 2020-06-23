using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class BounceBallData : BallDataNew
{
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
