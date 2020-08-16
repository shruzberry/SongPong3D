using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

/**
 *  A BallData implementation for BounceBalls
 */

public class BounceBallData : BallData
{
    //_____ SETTINGS ____________________
    private float defaultBounceHeight = 6.0f;

    //_____ REFERENCES _________________

    //_____ COMPONENTS ________________

    //_____ ATTRIBUTES __________________
    private const int minNotes = 2;
    private const int maxNotes = int.MaxValue;
    public override int MinNotes {get{return minNotes;}}
    public override int MaxNotes {get{return maxNotes;}}

    //_____ BOOLS ______________________

    //_____ OTHER ______________________
    public string prefabPath = "Prefabs/Balls/BounceBall";

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

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
        InitializeOption(defaultBounceHeight, "Bounce Height");
    }

    protected override void SetPrefab()
    {
        prefab = Resources.Load(prefabPath) as GameObject;
    }
}
