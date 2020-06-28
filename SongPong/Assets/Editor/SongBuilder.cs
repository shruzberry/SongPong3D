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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Types;

[ExecuteInEditMode]
public class SongBuilder : EditorWindow
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    List<NoteData> deleteNoteList = new List<NoteData>();

    // References
    Game game;
    SongData songData;
    SongController songController;
    List<BallData> ballList = new List<BallData>();

    // Navigation bar
    int navButtonHeight = 20;
    int navButtonWidth = 35;
    //int ballButtonWidth = 100;

    // EVENTS
    public delegate void OnBallDataUpdate();
    public event OnBallDataUpdate onBallDataUpdate;

    // SONG
    int jumpToTime;

    // Color
    private Color dividerLineColor = new Color(0f,0f,0f,0.1f);

    // Button Textures
    private Texture2D addNoteTexture;
    private Texture2D deleteNoteTexture;

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
    
    public void OnEnable()
    {
        game = FindObjectOfType<Game>();
        addNoteTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/CreateNote_tex.png", typeof(Texture2D));
        deleteNoteTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/DeleteNote_tex.png", typeof(Texture2D));
        oldColor = GUI.color;
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

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* SONG CONTROLS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void DrawNavSettings()
    {
        songController = FindObjectOfType<SongController>();

        GUILayout.BeginArea(navBarSection);
            GUILayout.Label("Navigation");

            GUILayout.BeginHorizontal();
                songData = (SongData)EditorGUILayout.ObjectField(songData, typeof(SongData), true, GUILayout.MaxWidth(187));
                game.editorSong = songData;
            GUILayout.EndHorizontal();

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

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* BALL LIST
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void DrawBallData()
    {
        if(songData == null) return;
        GUIStyle b = new GUIStyle(GUI.skin.button);
        BallDropper dropper = FindObjectOfType<BallDropper>();

        GUILayout.BeginArea(viewSection);
            GUILayout.Space(10.0f);

            // CREATE SIMPLE BALL
            GUILayout.BeginHorizontal();
                ChangeColor(Color.green);
                if (GUILayout.Button("+ Simple Ball", b, GUILayout.Width(100)))
                {
                    SongEdit.CreateBall(typeof(SimpleBallData));
                }
                ResetColor();
            GUILayout.EndHorizontal();

            // DRAW BALLS IN SONG
            activeBallsScrollPosition = GUILayout.BeginScrollView(activeBallsScrollPosition,
                                        GUILayout.Width(viewSection.width),
                                        GUILayout.Height(viewSection.height - 75));
                GUILayout.Space(10.0f);
                DrawBallDataList(Color.blue);
            GUILayout.EndScrollView();

        GUILayout.EndArea();
    }

    void DrawBallDataList(Color color)
    {
        ballList = game.GetBallData();

        GUIStyle s = new GUIStyle(GUI.skin.button);
        GUIStyle b = new GUIStyle(GUI.skin.button);
        s.alignment = TextAnchor.MiddleLeft;
        b.alignment = TextAnchor.MiddleCenter;
        var w = GUILayout.Width(100);

        List<BallData> newBalls = new List<BallData>();
        List<BallData> deleteBalls = new List<BallData>();
        List<BallData> typeChangeBalls = new List<BallData>();

        //Ball Field
        foreach(BallData ball in ballList)
        {
            DrawUILine(dividerLineColor);
            EditorUtility.SetDirty(ball);
            GUILayout.BeginHorizontal();

                // Change color of field when ball is close to hit time
                if(Application.isPlaying)
                    CheckBallActivity(ball, oldColor, Color.blue);

                //________Create New Ball___________________
                ChangeColor(Color.green);
                if (GUILayout.Button("+", b, GUILayout.Width(25)))
                {
                    newBalls.Add(ball);
                }
                ResetColor();

                //________Delete Ball___________________
                ChangeColor(Color.red);
                if (GUILayout.Button("-", b, GUILayout.Width(25)))
                {
                    deleteBalls.Add(ball);
                }
                ResetColor();

                //________Ball Type___________________
                GUILayout.Label("type:", GUILayout.Width(40));
                BallTypes ball_type = (BallTypes)EditorGUILayout.EnumPopup("", ball.type, s, w);
                if(ball_type != ball.type)
                {
                    ball.type = ball_type; // if new type selected, set equal to type
                    typeChangeBalls.Add(ball);
                }

                //________Enabled / Disabled Field___________________
                ball.enabled = GUILayout.Toggle(ball.enabled, "Enabled", s, w);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                GUILayout.Space(31.0f);
                if(ball.options != null)
                {
                    foreach(BallOption option in ball.options)
                    {
                        GUIContent option_label = new GUIContent(option.name + ":");
                        GUIStyle style = GUI.skin.box;
                        Vector2 size = style.CalcSize(option_label);

                        GUILayout.Label(option_label, GUILayout.Width(size.x));
                        EditorGUILayout.FloatField("", option.value, s, GUILayout.Width(25));
                    }
                }
            GUILayout.EndHorizontal();

            DrawNotes(ball);
        }
        // --------- END BALL ITERATION --------------------------

        foreach(BallData ball in newBalls)
        {
            SongEdit.CreateBall(typeof(SimpleBallData));
        }

        foreach(BallData ball in deleteBalls)
        {
            SongEdit.DeleteBallAndNotes(ball);
        }

        foreach(BallData ball in typeChangeBalls)
        {
            SongEdit.ChangeBallType(ball, ball.type); // create new ball and copy info
            SongEdit.DeleteBallAndNotes(ball);
        }

        // UPDATE SONG DATA
        game.ReloadBallData();
    }

    void DrawNotes(BallData ball)
    {
        // STYLE
        GUIStyle s = new GUIStyle(GUI.skin.button);
        s.alignment = TextAnchor.MiddleLeft;

        GUIStyle b = new GUIStyle(GUI.skin.button);
        b.alignment = TextAnchor.MiddleCenter;

        var col_field_width = GUILayout.Width(40);
        var beat_field_width = GUILayout.Width(45);
        var dir_field_width = GUILayout.Width(100);

        List<NoteData> deleteNotes = new List<NoteData>();
        List<NoteData> newNotes = new List<NoteData>();

        // --------- BEGIN NOTES ITERATION --------------------------
        foreach(NoteData note in ball.notes)
        {
            EditorUtility.SetDirty(note);

            GUILayout.BeginHorizontal();
                GUILayout.Space(31.0f);

                GUIStyle cool = new GUIStyle(GUI.skin.button);
                cool.padding = new RectOffset(2,2,2,2);

                ChangeColor(Color.green);
                //________Add New Note___________________
                if (GUILayout.Button(addNoteTexture, cool, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    newNotes.Add(note); // Mark note to be added after iteration
                }
                ResetColor();

                //________Remove Note___________________
                ChangeColor(Color.red);
                if (GUILayout.Button(deleteNoteTexture, cool, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    deleteNotes.Add(note); // Mark note to be deleted after iteration
                }
                ResetColor();

                //________Edit Note Data___________________
                GUILayout.Label("col:", GUILayout.Width(25));
                note.hitPosition = EditorGUILayout.IntField("", note.hitPosition, s, col_field_width);
                GUILayout.Label("beat:", GUILayout.Width(32));
                note.hitBeat = EditorGUILayout.FloatField("", note.hitBeat, s, beat_field_width);
                note.noteDirection = (Direction)EditorGUILayout.EnumPopup("", note.noteDirection, s, dir_field_width);

            GUILayout.EndHorizontal();
        }
        // --------- END NOTES ITERATION --------------------------

        // Delete all notes marked to be deleted
        foreach(NoteData note in deleteNotes)
        {
            SongEdit.DeleteNote(ball, note);
        }

        // Add all notes marked to be added
        foreach(NoteData note in newNotes)
        {
            SongEdit.AppendNote(ball, note);
        }

        ResetColor();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void UpdateBallData()
    {
        if(onBallDataUpdate != null) onBallDataUpdate();
    }

    void CheckBallActivity(BallData ball, Color oldColor, Color setColor)
    {
        float timeDifference = Mathf.Abs(ball.notes[0].hitBeat - songController.GetSongTimeBeats());
        if (timeDifference == 0.0f)
            timeDifference = 0.001f;
        if (timeDifference < 8.0f)
            GUI.backgroundColor = oldColor + (setColor/(timeDifference));
        else
            GUI.backgroundColor = oldColor;
    }

    void CopyBall(BallData ball)
    {
        SongEdit.CreateBall(ball.GetType(), ball.notes);
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

    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y+=padding/2;
        r.x-=2;
        r.width +=6;
        EditorGUI.DrawRect(r, color);
    }

    /*
    private void ConvertOldBallDataToNew()
    {
        // Path ( Relative to Resources/ )
        string path = "SongData/data/" + songData.name;
        string ball_path = path + "/Balls/";
        string note_path = path + "/Notes/";
        
        // Load Balls
        BallData[] ballData = Resources.LoadAll<BallData>(ball_path);
        List<BallData> ballList = new List<BallData>();
        foreach(BallData bd in ballData){ballList.Add(bd);}
        Debug.Log(ballList.Count + " outdated balls found.");

        // Copy over old BallData to new version
        List<BallData> deleteBalls = new List<BallData>();
        List<NoteData> deleteNotes = new List<NoteData>();
        foreach(BallData ball in ballList)
        {
            BallTypes type = ball.type;
            SongEdit.CreateBall(type, ball.notes);
            deleteBalls.Add(ball);
            deleteNotes.AddRange(ball.notes);
        }

        // Delete old BallData
        // Currently accesses through active song data
        foreach(BallData ball in deleteBalls)
        {
            AssetDatabase.DeleteAsset(activeSongData.editingSong.dataPath + "/Balls/" + ball.name + ".asset");
        }

        // Delete old Notes (they're copied, don't worry!)
        Debug.Log(deleteNotes.Count + " outdated notes found.");
        foreach(NoteData note in deleteNotes)
        {
            AssetDatabase.DeleteAsset(activeSongData.editingSong.dataPath + "/Notes/" + note.name + ".asset");
        }
    }
*/
}
