using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class SongEdit : MonoBehaviour
{
    [MenuItem("SongEdit/Create Simple + Note")]
    public static void CreateSimple(string name)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        BallData bd = new BallData();
        NoteData nd = new NoteData();
        
        nd.hitBeat = (int) songController.currentBeat;
        nd.name = "Col_" + nd.hitPosition + ",Beat_" + nd.hitBeat;
        saveNote(nd);

        bd.type = BallTypes.simple;
        bd.name = name;
        bd.notes = new NoteData[1];
        bd.notes[0] = nd;
        saveBall(bd);        
    }

    public static void saveBall(BallData type)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();
        SongData songData = songController.songData;

        AssetDatabase.CreateAsset(type, songData.dataPath + "/Balls/" + type.name + ".asset");
        AssetDatabase.SaveAssets ();
        EditorUtility.FocusProjectWindow ();
        Selection.activeObject = type;
    }

    public static void saveNote(NoteData type)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();
        SongData songData = songController.songData;

        AssetDatabase.CreateAsset(type, songData.dataPath + "/Notes/" + type.name + ".asset");
        AssetDatabase.SaveAssets ();
        EditorUtility.FocusProjectWindow ();
        Selection.activeObject = type;
    }
}
