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
    [HideInInspector]
    public GameObject prefab;

    public void OnEnable()
    {
        SetPrefab();
    }

    private void SetPrefab()
    {
        switch(type)
        {
            case BallTypes.simple:
                prefab = Resources.Load("Prefabs/SimpleBall") as GameObject;
                break;
            case BallTypes.bounce:
                prefab = Resources.Load("Prefabs/BounceBall") as GameObject;
                break;
            default:
                break;
        }
    }
}