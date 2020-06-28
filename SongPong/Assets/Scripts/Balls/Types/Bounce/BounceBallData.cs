using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class BounceBallData : BallData
{
    // CONSTANTS
    private const int minNotes = 2;
    private const int maxNotes = int.MaxValue;

    // OPTIONS
    private BounceHeight bounceHeight;

    // PROPERTIES
    public override int MinNotes {get{return minNotes;}}
    public override int MaxNotes {get{return maxNotes;}}

    private void OnEnable() {
        base.OnEnable();
        Debug.Log("TESST");
        options = new List<BallOption>();
        bounceHeight = new BounceHeight(1.0f);
        options.Add(bounceHeight);
    }

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
