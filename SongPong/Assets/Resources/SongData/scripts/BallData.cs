using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

[CreateAssetMenuAttribute(fileName="Ball", menuName="Song Data/Ball")]
public class BallData : ScriptableObject
{
    public int id;
    public BallTypes type;
    public NoteData[] notes;
    public GameObject prefab;

    public void OnEnable()
    {
        prefab = Resources.Load("Prefabs/SimpleBall") as GameObject;
        type = BallTypes.simple;
        notes = new NoteData[1];
        notes[0] = (NoteData) ScriptableObject.CreateInstance(typeof(NoteData));
    }

    void SaveData()
    {
        var note = Editor.CreateInstance<NoteData>();
        AssetDatabase.CreateAsset(note, "Assets/Resources/SongData/paradise/Notes/");
    }
}


