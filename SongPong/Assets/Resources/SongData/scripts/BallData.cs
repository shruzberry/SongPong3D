using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

[CreateAssetMenuAttribute(fileName="Ball", menuName="Ball")]
public class BallData : ScriptableObject
{
    [HideInInspector]
    public int id;
    public bool enabled = true;
    public BallTypes type;
    public NoteData[] notes;
    public GameObject prefab;

    public void OnEnable()
    {
        //prefab = Resources.Load("Prefabs/SimpleBall") as GameObject;
        //type = BallTypes.simple;
        //notes = new NoteData[1];
    }
}