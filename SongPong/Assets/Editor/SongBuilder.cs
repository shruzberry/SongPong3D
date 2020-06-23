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

[ExecuteInEditMode]
public class SongBuilder : EditorWindow
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    List<NoteData> deleteNoteList = new List<NoteData>();
    SongController songController;
    ActiveSongData activeSongData;
    SongData songData;
    SongData lastSong;
    List<BallDataNew> ballList = new List<BallDataNew>();

    // Navigation bar
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
            ConvertOldBallDataToNew();
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
        activeSongData = FindObjectOfType<ActiveSongData>();

        GUILayout.BeginArea(navBarSection);
            GUILayout.Label("Navigation");

            GUILayout.BeginHorizontal();
                songData = activeSongData.editingSong;
                EditorGUILayout.ObjectField(songData, typeof(SongData), true, GUILayout.MaxWidth(187));
                GUILayout.Label("Scene > Settings > Editor > Active Song Data > Editing Song");
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
        GUIStyle b = new GUIStyle(GUI.skin.button);
        BallDropper dropper = FindObjectOfType<BallDropper>();

        GUILayout.BeginArea(viewSection);
            GUILayout.Space(10.0f);

            // CREATE SIMPLE BALL
            GUILayout.BeginHorizontal();
                ChangeColor(Color.green);
                if (GUILayout.Button("Add Simple Ball and Note", b, GUILayout.Width(200)))
                {
                    SongEdit.CreateBall(BallTypes.simple);
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
        ballList = songData.GetAllBallData();

        GUIStyle s = new GUIStyle(GUI.skin.button);
        GUIStyle b = new GUIStyle(GUI.skin.button);
        s.alignment = TextAnchor.MiddleLeft;
        b.alignment = TextAnchor.MiddleCenter;
        var w = GUILayout.Width(100);

        List<BallDataNew> newBalls = new List<BallDataNew>();
        List<BallDataNew> deleteBalls = new List<BallDataNew>();
        List<BallDataNew> typeChangeBalls = new List<BallDataNew>();

        //Ball Field
        foreach(BallDataNew ball in ballList)
        {
            EditorUtility.SetDirty(ball);
            GUILayout.BeginHorizontal();

                // Change color of field when ball is close to hit time
                if(Application.isPlaying)
                    CheckBallActivity(ball, oldColor, Color.blue);

                //________Create New Ball___________________
                ChangeColor(Color.green);
                if (GUILayout.Button("+", b, GUILayout.Width(25)))
                {
                    SongEdit.CreateBall(BallTypes.simple);
                }
                ResetColor();

                //________Delete Ball___________________
                ChangeColor(Color.red);
                if (GUILayout.Button("-", b, GUILayout.Width(25)))
                {
                    SongEdit.DeleteBallAndNotes(ball);
                    break;
                }
                ResetColor();

                //________Ball Type___________________
                GUILayout.Label("type:", GUILayout.Width(40));
                BallTypes ball_type = (BallTypes)EditorGUILayout.EnumPopup("", ball.type, s, w);
                if(ball_type != ball.type)
                {
                    ball.type = ball_type; // if new type selected, set equal to type
                    newBalls.Add(SongEdit.ChangeBallType(ball, ball_type)); // create new ball and copy info
                    deleteBalls.Add(ball);
                }

                //________Enabled / Disabled Field___________________
                ball.enabled = GUILayout.Toggle(ball.enabled, "Enabled", s, w);

            GUILayout.EndHorizontal();

            // Note Field
            List<NoteData> deleteNotes = new List<NoteData>();
            List<NoteData> newNotes = new List<NoteData>();

            foreach(NoteData note in ball.notes)
            {
                EditorUtility.SetDirty(note);

                GUILayout.BeginHorizontal();
                    GUILayout.Space(31.0f);

                    //________Add New Note___________________
                    ChangeColor(Color.green);
                    if (GUILayout.Button("+", b, GUILayout.Width(25)))
                    {
                        newNotes.Add(note); // Mark note to be added after iteration
                    }
                    ResetColor();

                    //________Remove Note___________________
                    ChangeColor(Color.red);
                    if (GUILayout.Button("-", b, GUILayout.Width(25)))
                    {
                        deleteNotes.Add(note); // Mark note to be deleted after iteration
                        continue;
                    }
                    ResetColor();

                    //________Edit Note Data___________________
                    GUILayout.Label("col:", GUILayout.Width(25));
                    note.hitPosition = EditorGUILayout.IntField("", note.hitPosition, s, w);
                    GUILayout.Label("beat:", GUILayout.Width(32));
                    note.hitBeat = EditorGUILayout.FloatField("", note.hitBeat, s, w);
                    note.noteDirection = (Direction)EditorGUILayout.EnumPopup("", note.noteDirection, s, w);

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
        // --------- END BALL ITERATION --------------------------

        foreach(BallDataNew ball in newBalls)
        {
            ballList.Add(ball);
        }

        foreach(BallDataNew ball in deleteBalls)
        {
            ballList.Remove(ball);
            SongEdit.DeleteBallAndNotes(ball);
        }

        // UPDATE SONG DATA
        songData.UpdateBallData();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void ConvertOldBallDataToNew()
    {
        string path = "SongData/data/" + songData.songName + "/Balls/";
        Debug.Log(path);
        BallData[] ballData = Resources.LoadAll<BallData>(path);
        List<BallData> ballList = new List<BallData>();
        foreach(BallData bd in ballData)
        {
            Debug.Log("HELLO");
            ballList.Add(bd);
        }

        List<BallData> deleteBalls = new List<BallData>();

        foreach(BallData ball in ballList)
        {
            BallTypes type = ball.type;

            SongEdit.CreateBall(type,ball.notes);
            deleteBalls.Add(ball);
        }

        foreach(BallData ball in deleteBalls)
        {
            AssetDatabase.DeleteAsset(songController.GetDataPath() + "/Balls/" + ball.name + ".asset");
        }
    }

    void CheckBallActivity(BallDataNew ball, Color oldColor, Color setColor)
    {
        float timeDifference = Mathf.Abs(ball.notes[0].hitBeat - songController.GetSongTimeBeats());
        if (timeDifference == 0.0f)
            timeDifference = 0.001f;
        if (timeDifference < 8.0f)
            GUI.backgroundColor = oldColor + (setColor/(timeDifference));
        else
            GUI.backgroundColor = oldColor;
    }

    void CopyBall(BallDataNew ball)
    {
        BallDataNew bd = (BallDataNew)ScriptableObject.CreateInstance("BallDataNew");
        bd.type = ball.type;
        bd.enabled = ball.enabled;
        bd.name = "NewBall";
        
        SongEdit.CreateSimple();

        foreach(NoteData note in ball.notes)
        {
            SongEdit.AppendNote(bd, note);
        }
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
