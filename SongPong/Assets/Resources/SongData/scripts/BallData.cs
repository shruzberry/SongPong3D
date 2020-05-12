using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenuAttribute(fileName="Ball", menuName="Song Data/Ball")]
public class BallData : ScriptableObject
{
    public int id;
    public BallTypes type;
    public NoteData[] notes;
    public GameObject prefab;
}
