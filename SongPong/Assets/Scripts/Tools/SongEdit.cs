/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: SongEditorWindow.cs
Purpose: Add Balls and Notes quicker

________ USAGE ________
* Go to SongEdit dropdown in the unity editor and call any public method
* Or Call static SongEdit funtions

________ PUBLIC ________
+ CreateSimple(string name, [NoteData note])
    - Initialize Simple ball with an empty note
    - Optional Seconds parameter defines the note associated with the ball

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* DEPENDENCIES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Types;

public class SongEdit : MonoBehaviour
{

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public static void CreateSimple(string name)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        BallData bd = new BallData();
        NoteData nd = new NoteData();
        
        nd.hitBeat = (int) songController.currentBeat;
        saveNote(nd);

        bd.type = BallTypes.simple;
        bd.name = name;
        bd.notes = new NoteData[1];
        bd.notes[0] = nd;
        saveBall(bd);        
    }

    //[MenuItem("SongEdit/Create Simple + Note Listener")]
    public static void CreateSimple(string name, NoteData nd)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        BallData bd = new BallData();
        
        saveNote(nd);

        bd.type = BallTypes.simple;
        bd.name = name;
        bd.notes = new NoteData[1];
        bd.notes[0] = nd;
        saveBall(bd);        
    }

    public static void CreateBounce(string name, List<NoteData> nd)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        BallData bd = new BallData();
        
        foreach (NoteData noteData in nd)
        {
            
        }

        bd.type = BallTypes.bounce;
        bd.name = name;
        bd.notes = new NoteData[nd.Count];
        
        int i = 0;
        foreach (NoteData noteData in nd)
        {
            noteData.name = name + "_" + i;
            bd.notes[i] = noteData;
            saveNote(noteData);
            
            i++;
        }

        saveBall(bd);        
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

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
