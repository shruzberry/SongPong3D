/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: BallBuilder.cs
Purpose: Creates an instance of a ballData using the note listener buffer

________ USAGE ________
* Attach script to game object
* call createBall method

________ ATTRIBUTES ________
+ List<String> SupportedTypes

________ FUNCTIONS ________
+ createBall(string Type, List<Note> notes)

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class BallBuilder : MonoBehaviour
{
    public void CreateBall(string type, List<Note> notes)
    {
/*
        BallData bd = new Balldata;

        switch (type)
        {
            case "simple":
                foreach(Note n in notes)
                {
                    bd.type = BallTypes.simple;
                    bd.notes[0] = new NoteData();
                }
            break;
        }
    */
    }
    
}
