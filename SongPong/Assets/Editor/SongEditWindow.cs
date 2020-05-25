/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: SongEditor.cs
Purpose: Add Balls and Notes quicker

________ USAGE ________
* Go to Windows/Song Builder to open the editor
* Use Navigation controls to change song time
* Use Note Listener to capture note information
* Click on balls to find them in the Asset Folder

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
    
    string ballName = "";

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* STARTUP FUNCTIONS
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
        //songEdit = GameObject.Find("SongEdit").GetComponent<SongEdit>();
    }

    void OnGUI()
    {
        GUILayout.Label("Ball Name");
        GUILayout.Label(ballName);

        ballName = EditorGUILayout.TextField("Ball Name: ", ballName);
        
        if (GUILayout.Button("Create Simple + Note"))
        {
            SongEdit.CreateSimple(ballName);
        }
    }
}
