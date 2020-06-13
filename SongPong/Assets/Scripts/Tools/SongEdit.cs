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
using System;
using System.IO;

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

        nd.hitBeat = (int) songController.GetSongTimeBeats();
        saveNote(nd);

        bd.type = BallTypes.simple;
        bd.name = name;
        bd.notes = new List<NoteData>();
        bd.notes.Add(nd);
        saveBall(bd);
    }

    //[MenuItem("SongEdit/Create Simple + Note Listener")]
    public static void CreateSimple(string name, NoteData nd)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        //BallData bd = new BallData();
        BallData bd = (BallData)ScriptableObject.CreateInstance("BallData");

        saveNote(nd);

        bd.type = BallTypes.simple;
        bd.name = name;
        bd.notes = new List<NoteData>();
        bd.notes.Add(nd);
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
        bd.notes = new List<NoteData>();

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

    public static void AppendToBall(BallData ball, NoteData note)
    {
        ball.notes.Add(note);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public static void saveBall(BallData type)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        AssetDatabase.CreateAsset(type, songController.GetDataPath() + "/Balls/" + GetDataName("Ball") + ".asset");
        AssetDatabase.SaveAssets ();
        EditorUtility.FocusProjectWindow ();
        Selection.activeObject = type;
    }

    public static void saveNote(NoteData type)
    {
        if(!Application.isPlaying)
        {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        string path = songController.GetDataPath() + "/Notes/" + GetDataName("Note") + ".asset";
        if(Directory.Exists(path))
            Debug.Log("Already HERE");

        AssetDatabase.CreateAsset(type, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = type;
        }
    }

    public static void DeleteNote(BallData ball, NoteData note)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        AssetDatabase.DeleteAsset(songController.GetDataPath() + "/Notes/" + note.name + ".asset");
        ball.notes.Remove(note);
    }

    public static void DeleteBall(BallData ball)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        if(ball.notes.Count >= 1)
        {
            foreach(NoteData nd in ball.notes)
            {
                AssetDatabase.DeleteAsset(songController.GetDataPath() + "/Notes/" + nd.name + ".asset");
            }
        }

        AssetDatabase.DeleteAsset(songController.GetDataPath() + "/Balls/" + ball.name + ".asset");

    }

    private static string GetDataName(string s)
    {
        return(s + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss"));
    }
}
