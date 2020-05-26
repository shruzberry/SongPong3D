/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: SongEditorWindow.cs
Purpose: Add Balls and Notes quicker

________ USAGE ________
* Go to Windows/Song Builder to open the editor
* Use controls to create balls

________ ATTRIBUTES ________


________ FUNCTIONS ________

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* DEPENDENCIES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SongEditWindow : EditorWindow
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    
    SongController songController;
    NoteListener noteListener;
    
    string ballName = "";
    int colNum = 0;
    float beatNum = 0.0f;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    [MenuItem("Window/Song Editor")]
    static void OpenWindow()
    {
        SongEditWindow window = (SongEditWindow)GetWindow(typeof(SongEditWindow), false, "Song Editor");
        window.minSize = new Vector2(200, 200);
        window.Show();
    }

    void OnEnable()
    {
        songController = GameObject.Find("SongController").GetComponent<SongController>();
        noteListener = GameObject.Find("NoteListener").GetComponent<NoteListener>();    
    }

    void OnGUI()
    {
        GUILayout.Label("Ball Name");
        GUILayout.Label(ballName);

        ballName = EditorGUILayout.TextField("Ball Name: ", ballName);
        colNum = EditorGUILayout.IntField("Col: ", colNum);
        beatNum = EditorGUILayout.FloatField("Beat: ", beatNum);
        
        if (GUILayout.Button("Create Simple + Note"))
        {
            NoteData nd = new NoteData();
            nd.hitPosition = colNum;
            nd.hitBeat = beatNum;
            nd.name = ballName;
            SongEdit.CreateSimple(ballName, nd);
        }
    }
}
