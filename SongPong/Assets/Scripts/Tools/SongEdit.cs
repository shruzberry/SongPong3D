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

    public static BallData CreateBall(Type type, List<NoteData> notes = null)
    {
        BallData new_ball = (BallData)ScriptableObject.CreateInstance(type);
        new_ball.Initialize("Generic");

        // If notes were not passed in as a parameter
        // Create new notes up to the min notes of the ball type
        if(notes == null)
        {
            for(int i = 0; i < new_ball.MinNotes; i++)
            {
                new_ball.notes.Add(CreateNote());
            }
        }
        // If notes were passed as parameter,
        // Create new notes, copied from the notes parameter.
        else
        {
            // Copy notes over as long as it doesn't go over the MaxNotes property of the new ball type
            for(int i = 0; i < notes.Count; i++)
            {
                if(new_ball.notes.Count < new_ball.MaxNotes)
                {
                    new_ball.notes.Add(CreateNote(notes[i]));
                }
                else{break;}
            }
            // If ball still needs more notes after copy
            // (Ex. SimpleBall --> BounceBall)
            // Create the rest of the notes
            if(new_ball.notes.Count < new_ball.MinNotes)
            {
                new_ball.notes.Add(CreateNote());
            }
        }
        
        SaveNotes(new_ball.notes);
        SaveBall(new_ball);
        return new_ball;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* EDIT BALLS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public static void SaveBall(BallData type)
    {
        Game game = FindObjectOfType<Game>();

        string dpath = game.GetEditorSong().dataPath + "/Balls/";
        if(!Directory.Exists(dpath))
            System.IO.Directory.CreateDirectory(dpath);

        string path = GetDataName(dpath, "Ball", ".asset");
        AssetDatabase.CreateAsset(type, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = type;
    }

    public static void DeleteBallAndNotes(BallData ball)
    {
        Game game = FindObjectOfType<Game>();
        if(ball.notes.Count > 0)
        {
            foreach(NoteData nd in ball.notes)
            {
                AssetDatabase.DeleteAsset(game.GetEditorSong().dataPath + "/Notes/" + nd.name + ".asset");
            }
        }
        DeleteBall(ball);
    }

    public static void DeleteBall(BallData ball)
    {
        Game game = FindObjectOfType<Game>();
        string path = game.GetEditorSong().dataPath + "/Balls/" + ball.name + ".asset";
        
        AssetDatabase.DeleteAsset(path);
    }

    public static BallData ChangeBallType(BallData ball, BallTypes type)
    {
        BallData new_ball = null;
        switch(type)
        {
            case BallTypes.simple:
                new_ball = CreateBall(typeof(SimpleBallData), ball.notes);
                break;
            case BallTypes.bounce:
                new_ball = CreateBall(typeof(BounceBallData), ball.notes);
                break;
            default:
                Debug.LogError("That ball type has not been created yet.");
                break;
        }
        return new_ball;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* EDIT NOTES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public static void AppendNote(BallData ball, NoteData note)
    {
        if(ball.notes.Count >= ball.MaxNotes)
        {
            Debug.LogWarning("Reached maximum number of notes for this ball type.");
            return;
        }
        NoteData new_note = CreateNote(note);

        bool success = SaveNote(new_note);

        // if the note was successfully created, add it to the list
        if(success) ball.notes.Add(new_note);
    }

    // Create a note into a certain index
    public static void InsertNote(BallData ball, NoteData note , int index)
    {
        if(ball.notes.Count >= ball.MaxNotes)
        {
            Debug.LogWarning("Reached maximum number of notes for this ball type.");
            return;
        }
        NoteData new_note = CreateNote(note);
        bool success = SaveNote(new_note);

        if(success) ball.notes.Insert(index, new_note);
    }

    // Create a new note
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
        Game game = FindObjectOfType<Game>();

        string dpath = game.GetEditorSong().dataPath + "/Notes/";
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

    public static void DeleteNote(BallData ball, NoteData note)
    {
        Game game = FindObjectOfType<Game>();

        ball.notes.Remove(note);
        
        AssetDatabase.DeleteAsset(game.GetEditorSong().dataPath + "/Notes/" + note.name + ".asset");
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
