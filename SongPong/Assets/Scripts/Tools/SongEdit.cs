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
* CREATE BALLS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public static BallDataNew CreateSimple(List<NoteData> notes = null)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        SimpleBallData new_ball = (SimpleBallData)ScriptableObject.CreateInstance(typeof(SimpleBallData));
        new_ball.Initialize("NewSimple");

        if(notes == null)
        {
            NoteData note = CreateNote();
            note.hitBeat = 0;
            new_ball.notes.Add(note);
            SaveNote(note);
        }
        else
        {
            new_ball.notes.Add(CreateNote(notes[0]));
            SaveNotes(new_ball.notes);
        }

        SaveBall(new_ball);

        return new_ball;
    }

    public static BallDataNew CreateBounce(List<NoteData> notes = null)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        BounceBallData new_ball = (BounceBallData)ScriptableObject.CreateInstance(typeof(BounceBallData));
        new_ball.Initialize("NewBounce");

        if(notes == null)
        {
            new_ball.notes.Add(CreateNote()); // NOTE: Creating two notes at the same time gives them the same name
            new_ball.notes.Add(CreateNote());
            SaveNotes(new_ball.notes);
        }
        else
        {
            foreach(NoteData note in notes)
            {
                new_ball.notes.Add(CreateNote(note));
            }
            if(new_ball.notes.Count < 2)
            {
                new_ball.notes.Add(CreateNote());
            }
            SaveNotes(new_ball.notes);
        }

        SaveBall(new_ball);

        return new_ball;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* EDIT BALLS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public static void SaveBall(BallDataNew type)
    {
        ActiveSongData songController = FindObjectOfType<ActiveSongData>();

        string dpath = songController.editingSong.dataPath + "/Balls/";
        if(!Directory.Exists(dpath))
            System.IO.Directory.CreateDirectory(dpath);

        string path = GetDataName(dpath, "Ball", ".asset");
        AssetDatabase.CreateAsset(type, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = type;
    }

    public static BallDataNew CreateBall(BallTypes type, List<NoteData> notes = null)
    {
        BallDataNew newBall;
        switch(type)
        {
            case BallTypes.simple:
                newBall = CreateSimple(notes);
                break;
            case BallTypes.bounce:
                newBall = CreateBounce(notes);
                break;
            default:
                Debug.LogError("THIS BALL TYPE HAS NOT BEEN ADDED YET.");
                return null;
        }
        return newBall;
    }

    public static void DeleteBallAndNotes(BallDataNew ball)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        if(ball.notes.Count > 0)
        {
            foreach(NoteData nd in ball.notes)
            {
                AssetDatabase.DeleteAsset(songController.GetDataPath() + "/Notes/" + nd.name + ".asset");
            }
        }
        DeleteBall(ball);
    }

    public static void DeleteBall(BallDataNew ball)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();
        
        AssetDatabase.DeleteAsset(songController.GetDataPath() + "/Balls/" + ball.name + ".asset");
    }

    public static BallDataNew ChangeBallType(BallDataNew ball, BallTypes type)
    {
        BallDataNew newBall = CreateBall(type, ball.notes);

        return newBall;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* EDIT NOTES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public static void AppendNote(BallDataNew ball, NoteData note)
    {
        NoteData new_note = (NoteData)ScriptableObject.CreateInstance("NoteData");

        new_note.noteDirection = Direction.negative;
        new_note.name = "NewNote";
        new_note.hitPosition = note.hitPosition;
        new_note.hitBeat = note.hitBeat;

        bool success = SaveNote(new_note);

        // if the note was successfully created, add it to the list
        if(success) ball.notes.Add(new_note);
    }

    public static NoteData CreateNote()
    {
        return (NoteData)ScriptableObject.CreateInstance(typeof(NoteData));
    }

    // Create a new note with the info from a previous note
    public static NoteData CreateNote(NoteData note)
    {
        NoteData new_note = CreateNote();
        new_note.hitBeat = note.hitBeat;
        new_note.hitPosition = note.hitPosition;
        new_note.noteDirection = note.noteDirection;

        return new_note;
    }

    public static void SaveNotes(List<NoteData> notes)
    {
        foreach (NoteData noteData in notes)
        {
            SaveNote(noteData);
        }
    }

    public static bool SaveNote(NoteData note)
    {
        ActiveSongData songController = FindObjectOfType<ActiveSongData>();

        string dpath = songController.editingSong.dataPath + "/Notes/";
        Debug.Log("DPATH: " + dpath);
        if(!Directory.Exists(dpath))
            System.IO.Directory.CreateDirectory(dpath);

        string path = GetDataName(dpath, "Note", ".asset");
        if(File.Exists(path))
        {
            Debug.LogWarning("Note with this name already exists. Skipped. Or stop clicking so f***ing fast.");
            return false;
        }
        AssetDatabase.CreateAsset(note, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = note;
        return true;
    }

    public static void DeleteNote(BallDataNew ball, NoteData note)
    {
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();

        ball.notes.Remove(note);
        
        AssetDatabase.DeleteAsset(songController.GetDataPath() + "/Notes/" + note.name + ".asset");
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* GETTERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private static string GetDataName(string root_path, string obj_name, string obj_type)
    {        
        string name = root_path + obj_name + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + obj_type;
        int duplicateNum = 0;

        while(File.Exists(name))
        {
            name = root_path + obj_name + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + duplicateNum.ToString() + obj_type;
            duplicateNum++;
        }

        return name;
    }
}
