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
    private float deafultBounceHeight = 2.0f;

    // PROPERTIES
    public override int MinNotes {get{return minNotes;}}
    public override int MaxNotes {get{return maxNotes;}}

    public override void OnEnable() 
    {
        base.OnEnable();
        Initialize(this.name);
    }

    public override void Initialize(string name)
    {
        this.name = name;
        type = BallTypes.bounce;
        if(notes == null) notes = new List<NoteData>();

        if(options == null)InitializeOptions();
        InitializeOption(deafultBounceHeight, "Bounce Height");
    }

    protected override void SetPrefab()
    {
        prefab = Resources.Load("Prefabs/BounceBall") as GameObject;
    }
}
