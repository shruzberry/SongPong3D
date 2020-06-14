/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: SongBuilder.cs
Purpose: Controls the song and displays active balls in order to locate Note and Ball Data Files
Associations:

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
using UnityEngine.SceneManagement;
using Types;


public class SongBuilder : EditorWindow
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    List<NoteData> deleteNoteList = new List<NoteData>();

    SongController songController;
    SongData songData;
    SongData lastSong;

    //float navBarSectionSize = .15f;

    // for navigation bar
    int navButtonHeight = 20;
    int navButtonWidth = 35;
    //int ballButtonWidth = 100;

    int jumpToTime;

    Rect fullWindow;
    Rect navBarSection;
    Rect viewSection;

    Vector2 activeBallsScrollPosition;

    Color defaultColor;
    Color oldColor;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* STARTUP FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    [MenuItem("Window/Song Builder")]
    static void OpenWindow()
    {
        SongBuilder window = (SongBuilder)GetWindow(typeof(SongBuilder), false, "Song Builder");
        window.minSize = new Vector2(510, 420);
        window.maxSize = new Vector2(510, 420);
        window.Show();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* GUI DRAW FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnGUI()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            DrawRowLayouts();
            DrawNavSettings();
            DrawBallData();
        }
        else
        {
            GUILayout.Label("You must be in the Song Scene to use this tool");
        }
    }

    void DrawRowLayouts()
    {
        fullWindow = new Rect(0, 0, Screen.width, Screen.height);

        navBarSection.x = 0;
        navBarSection.y = 0;
        navBarSection.width = 500;
        navBarSection.height = 60;

        viewSection.x = 0;
        viewSection.y = 60;
        viewSection.width = 500;
        viewSection.height = 400;
    }

    void DrawNavSettings()
    {
        songController = FindObjectOfType<SongController>();
        GUILayout.BeginArea(navBarSection);
            GUILayout.Label("Navigation");
            songData = songController.GetSongData();
            EditorGUILayout.ObjectField(songData, typeof(SongData), true, GUILayout.MaxWidth(187));
            if(songData != null)
            {
            EditorGUILayout.BeginHorizontal();

                // Restart Song
                if (GUILayout.Button("<<", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToStart();
                }

                // Go Back 8 beats
                if (GUILayout.Button("<", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToBeat(songController.GetSongTimeBeats() - 8);
                }

                // Pause/Play
                if (GUILayout.Button("Play", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToBeat(jumpToTime);
                }

                // Go Forward 8 beats
                if (GUILayout.Button(">", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToBeat(songController.GetSongTimeBeats() + 8);
                }

                // Go to end of song
                if (GUILayout.Button(">>", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToEnd();
                }

                // Current Beat
                GUILayout.Label("Beat: " + songController.GetSongTimeBeats(), GUILayout.Width(60));

                // Song Slider
                jumpToTime = (int) EditorGUILayout.Slider((float)jumpToTime, songData.startBeat, songData.endBeat);

            EditorGUILayout.EndHorizontal();
            }
        GUILayout.EndArea();
    }

    void DrawBallData()
    {
        GUIStyle b = new GUIStyle(GUI.skin.button);
        BallDropper dropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();

        GUILayout.BeginArea(viewSection);
            GUILayout.Space(10.0f);

            GUILayout.BeginHorizontal();
                ChangeColor(Color.green);
                if (GUILayout.Button("Add Simple Ball and Note", b, GUILayout.Width(200)))
                {
                    CreateBlankBall();
                }ResetColor();
            GUILayout.EndHorizontal();

            activeBallsScrollPosition = GUILayout.BeginScrollView(activeBallsScrollPosition,
                                        GUILayout.Width(viewSection.width),
                                        GUILayout.Height(viewSection.height - 75));
                GUILayout.Space(10.0f);
                DrawBallDataList(songData.GetAllBallData(), Color.blue);
            GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void DrawBallDataList(BallData[] balls, Color color)
    {
        if(balls == null)
            return;

        GUIStyle s = new GUIStyle(GUI.skin.button);
        GUIStyle b = new GUIStyle(GUI.skin.button);
        s.alignment = TextAnchor.MiddleLeft;
        b.alignment = TextAnchor.MiddleCenter;
        var w = GUILayout.Width(100);

        //Ball Field
        foreach(BallData ball in balls)
        {
            //CheckBallActivity(ball, oldColor, Color.blue);
            GUILayout.BeginHorizontal();

                ChangeColor(Color.green);
                if (GUILayout.Button("+", b, GUILayout.Width(25)))
                {
                    CreateBlankBall();
                }
                ResetColor();

                ChangeColor(Color.red);
                if (GUILayout.Button("-", b, GUILayout.Width(25)))
                {
                    SongEdit.DeleteBall(ball);
                }
                ResetColor();

                GUILayout.Label("type:", GUILayout.Width(40));
                ball.type = (BallTypes)EditorGUILayout.EnumPopup("", ball.type, s, w);
                ball.enabled = GUILayout.Toggle(ball.enabled, "Enabled", s, w);

            GUILayout.EndHorizontal();

            // Note Field
            foreach(NoteData note in ball.notes)
            {
                if (note == null)
                    return;

                GUILayout.BeginHorizontal();
                    GUILayout.Space(31.0f);

                    // Add Note Button
                    ChangeColor(Color.green);
                    if (GUILayout.Button("+", b, GUILayout.Width(25)))
                    {
                        AppendNote(ball);
                    }
                    ResetColor();

                    // Remove Note
                    ChangeColor(Color.red);
                    if (GUILayout.Button("-", b, GUILayout.Width(25)))
                    {
                        SongEdit.DeleteNote(ball, note);
                    }
                    ResetColor();

                    // Edit Note Data

                    GUILayout.Label("col:", GUILayout.Width(25));
                    note.hitPosition = EditorGUILayout.IntField("", note.hitPosition, s, w);
                    GUILayout.Label("beat:", GUILayout.Width(32));
                    note.hitBeat = EditorGUILayout.FloatField("", note.hitBeat, s, w);
                    note.noteDirection = (Direction)EditorGUILayout.EnumPopup("", note.noteDirection, s, w);


                GUILayout.EndHorizontal();

                EditorUtility.SetDirty(note);
            }
            ResetColor();
            EditorUtility.SetDirty(ball);
        }
    }

    void CheckBallActivity(BallData ball, Color oldColor, Color setColor)
    {
        if (ball.activity > 0)
        {
            GUI.color = oldColor + (setColor * (ball.activity / 100));
            ball.activity -= 21.0f * Time.deltaTime;
        }
    }

    void CreateBlankBall()
    {
        if (Application.isEditor)
        {
            //BallData bd = new BallData();
            BallData bd = (BallData)ScriptableObject.CreateInstance("BallData");
            bd.type = BallTypes.simple;
            bd.enabled = true;
            bd.name = "NewBall";

            //NoteData nd = new NoteData();
            NoteData nd = (NoteData)ScriptableObject.CreateInstance("NoteData");
            nd.noteDirection = Direction.negative;
            //nd.hitPosition = 0;
            //nd.hitBeat = 0;
            nd.name = "NewNote";
            SongEdit.CreateSimple("NewBall", nd);
        }
    }

    void AppendNote(BallData ball)
    {
            //NoteData nd = new NoteData();
            NoteData nd = (NoteData)ScriptableObject.CreateInstance("NoteData");
            //nd.hitPosition = 0;
            //nd.hitBeat = 0;
            nd.noteDirection = Direction.negative;
            nd.name = "NewNote";
            SongEdit.saveNote(nd);
            SongEdit.AppendToBall(ball, nd);
    }

    void ChangeColor(Color color)
    {
        defaultColor = GUI.color;
        GUI.color = color;
    }

    void ResetColor()
    {
        GUI.color = defaultColor;
    }

    public void Update()
    {
        Repaint();
    }

    public void OnEnable()
    {
        oldColor = GUI.color;
    }
}
