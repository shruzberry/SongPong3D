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

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Types;

public class SongBuilder : EditorWindow
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // References
    Game game;
    LevelChanger levelChanger;
    SongData songData;
    SongController songController;
    List<BallData> ballList = new List<BallData>();

    // Navigation bar
    int navButtonHeight = 20;
    int navButtonWidth = 35;

    // SONG
    int jumpToTime;

    // Color
    private Color dividerLineColor = new Color(0f,0f,0f,0.1f);

    // Button Textures
    private Texture2D addNoteTexture;
    private Texture2D deleteNoteTexture;

    Rect toolbarSection;
    Rect navBarSection;
    Rect viewSection;

    const float sidePadding = 10.0f;

    Vector2 ballDataScrollPosition;

    Color defaultColor;
    Color oldColor;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* STARTUP FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    [MenuItem("Window/Song Builder")]
    static void OpenWindow()
    {
        SongBuilder window = (SongBuilder)GetWindow(typeof(SongBuilder), false, "Song Builder");
        window.minSize = new Vector2(460, 420);
        window.maxSize = new Vector2(510, 420);
        window.Show();
    }
    
    public void OnEnable()
    {
        levelChanger = FindObjectOfType<LevelChanger>();
        game = FindObjectOfType<Game>();
        songController = FindObjectOfType<SongController>();
        
        songData = game.GetEditorSong();
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
            InitRowLayouts();

            DrawNavSettings();

            if(songData != null)
            {
                DrawToolBar();
                DrawBallData();
            }
        }
        else
        {
            GUILayout.Label("You must be in the Song Scene to use this tool");
        }
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* ROW
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void InitRowLayouts()
    {
        navBarSection.x = sidePadding;
        navBarSection.y = 0;
        navBarSection.width = (position.width - (2 * sidePadding));
        navBarSection.height = 60;

        toolbarSection.x = sidePadding;
        toolbarSection.y = navBarSection.height + 15; 
        toolbarSection.width = (position.width - (2 * sidePadding));
        toolbarSection.height = 20;

        float topMargin = 10;
        float botMargin = 10;
        float heightLeft = position.height - (toolbarSection.y + toolbarSection.height + topMargin);
        viewSection.x = sidePadding;
        viewSection.y = toolbarSection.y + toolbarSection.height + topMargin;
        viewSection.width = (position.width - (2 * sidePadding));
        viewSection.height = heightLeft - botMargin;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* SONG CONTROLS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void DrawNavSettings()
    {
        GUILayout.BeginArea(navBarSection);
            GUILayout.Label("Navigation");

            game = FindObjectOfType<Game>();
            songController = FindObjectOfType<SongController>();

            GUILayout.BeginHorizontal();
                if(songData == null) {songData = game.GetEditorSong();}
                SongData newData = (SongData)EditorGUILayout.ObjectField(songData, typeof(SongData), true, GUILayout.MaxWidth(187));
                if(songData != newData)
                {
                    Debug.Log("CHANGE SONG TO " + newData.name);
                    songData = newData;
                    game.SetEditorSong(songData);
                    game.ReloadBallData();
                    EditorUtility.SetDirty(game);
                }
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
                if(Application.isPlaying)
                    GUILayout.Label("Beat: " + songController.GetSongTimeBeats(), GUILayout.Width(100));

                // Song Slider
                jumpToTime = (int) EditorGUILayout.Slider((float)jumpToTime, songData.startBeat, songData.endBeat);

            EditorGUILayout.EndHorizontal();
            }
        GUILayout.EndArea();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* TOOLBAR
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void DrawToolBar()
    {
        GUIStyle b = new GUIStyle(GUI.skin.button);

        GUILayout.BeginArea(toolbarSection);
            GUILayout.BeginHorizontal();

                // Create Simple Ball
                ChangeColor(Color.green);
                if (GUILayout.Button("+ Simple Ball", b, GUILayout.Width(100)))
                {
                    BallData new_ball = SongEdit.CreateBall(typeof(SimpleBallData));
                    ballList.Add(new_ball);
                }
                ResetColor();

                // Sort Balls
                ChangeColor(new Color(0.909f, 0.635f, 0.066f));
                if (GUILayout.Button("Sort Balls", b, GUILayout.Width(80)))
                {
                    game.SortBalls();
                }
                ResetColor();

                // Sort Notes
                ChangeColor(new Color(0.717f, 0.262f, 0.937f));
                if (GUILayout.Button("Sort Notes", b, GUILayout.Width(80)))
                {
                    game.SortNotes();
                }
                ResetColor();

            GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* BALL LIST
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void DrawBallData()
    {
        GUILayout.BeginArea(viewSection);

            // DRAW BALLS IN SONG
            ballDataScrollPosition = GUILayout.BeginScrollView(ballDataScrollPosition,
                                                    GUILayout.Width(viewSection.width),
                                                    GUILayout.Height(viewSection.height));
                DrawBallDataList(Color.blue);
            GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private Dictionary<BallData, int> newBalls = new Dictionary<BallData, int>();
    private List<BallData> deleteBalls = new List<BallData>();
    private Dictionary<BallData, int> typeChangeBalls = new Dictionary<BallData, int>();

    void DrawBallDataList(Color color)
    {
        ballList = game.GetBallData();

        GUIStyle s = new GUIStyle(GUI.skin.button);
        GUIStyle b = new GUIStyle(GUI.skin.button);
        s.alignment = TextAnchor.MiddleLeft;
        b.alignment = TextAnchor.MiddleCenter;
        var w = GUILayout.Width(100);
        var line_h = GUILayout.Height(20);

        float yPos = 0;
        List<BallLabel> labels = new List<BallLabel>();

        newBalls.Clear();
        deleteBalls.Clear();
        typeChangeBalls.Clear();
        
        //Ball Field
        foreach(BallData ball in ballList)
        {
            BallLabel ballLabel = new BallLabel(ball, yPos);

            if(isLabelVisible(ballLabel)){
                DrawUILine(dividerLineColor);
            
                EditorGUILayout.BeginHorizontal();

                    yPos += ballLabel.height;

                    //________Create New Ball___________________
                    ResetColor();
                    ChangeColor(Color.green);
                    if (GUILayout.Button("+", b, GUILayout.Width(25), line_h))
                    {
                        newBalls.Add(ball, ballList.IndexOf(ball));
                    }
                    ResetColor();

                    //________Delete Ball___________________
                    ChangeColor(Color.red);
                    if (GUILayout.Button("-", b, GUILayout.Width(25), line_h))
                    {
                        deleteBalls.Add(ball);
                    }
                    ResetColor();

                    //________Refresh Ball___________________
                    ChangeColor(Color.yellow);
                    if (GUILayout.Button("R", b, GUILayout.Width(25), line_h))
                    {
                        //SongData.DeleteBall(ball);
                        BallData repairedBall;
                        if (ball.GetType() == typeof(SimpleBallData))
                        {
                            repairedBall = SongEdit.CreateBall(typeof(SimpleBallData), ball.notes);
                        }    
                        else
                        {
                            Debug.Log("its a bounce ball");
                            repairedBall = SongEdit.CreateBall(typeof(BounceBallData), ball.notes);
                        }

                        ballList.Add(repairedBall);
                        
                        SongEdit.DeleteBallAndNotes(ball);
                        ballList.Remove(ball);

                        game.SortBalls();
                    }
                    ResetColor();

                    //________Ball Type___________________
                    GUILayout.Label("type:", GUILayout.Width(40), line_h);
                    BallTypes ball_type = (BallTypes)EditorGUILayout.EnumPopup("", ball.type, s, w);
                    if(ball_type != ball.type)
                    {
                        ball.type = ball_type; // if new type selected, set equal to type
                        typeChangeBalls.Add(ball, ballList.IndexOf(ball));
                        EditorUtility.SetDirty(ball);
                    }

                    //________Enabled / Disabled Field___________________
                    //ball.enabled = GUILayout.Toggle(ball.enabled, "Enabled", s, w);
                    
                    if(Application.isPlaying)
                    {
                        float timeDiff = ball.notes[0].hitBeat - songController.GetSongTimeBeats();
                        
                        if(Mathf.Abs(timeDiff) < 4.0f && timeDiff > 0.0f)
                        {
                            ChangeColor(Color.cyan);    
                        }
                        else if(Mathf.Abs(timeDiff) < 2.0f)
                            ChangeColor(Color.yellow);
                            
                        EditorGUILayout.ObjectField(ball, typeof(Object), true);

                        ResetColor();
                    }

                EditorGUILayout.EndHorizontal();

                DrawOptions(ball);

                if(ball.notes != null)
                {
                    int numNotes = DrawNotes(ball);
                }
            }
            else
            {
                GUILayout.Space(60);
                yPos += 60;
            }
        }
        // --------- END BALL ITERATION --------------------------

        foreach(KeyValuePair<BallData, int> pair in newBalls)
        {
            BallData new_ball = SongEdit.CreateBall(typeof(SimpleBallData));
            ballList.Insert(pair.Value + 1, new_ball);
        }

        foreach(BallData ball in deleteBalls)
        {
            SongEdit.DeleteBallAndNotes(ball);
            ballList.Remove(ball);
        }

        foreach(KeyValuePair<BallData, int> pair in typeChangeBalls)
        {
            BallData ball = pair.Key;
            BallData new_ball = SongEdit.ChangeBallType(ball, ball.type); // create new ball and copy info
            ballList.Insert(pair.Value, new_ball);

            SongEdit.DeleteBallAndNotes(ball);
            ballList.Remove(ball);
        }

    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* OPTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void DrawOptions(BallData ball)
    {
        GUIStyle s = new GUIStyle(GUI.skin.button);
        var line_h = GUILayout.Height(20);
        s.alignment = TextAnchor.MiddleLeft;

        GUILayout.BeginHorizontal();
            GUILayout.Space(31.0f);
            if(ball.options != null)
            {
                foreach(BallOption option in ball.options)
                {
                    GUIContent option_label = new GUIContent(option.opt_name + ":");

                    GUILayout.Label(option_label, GUILayout.Width(100), line_h);

                    option.value = EditorGUILayout.FloatField("", option.value, s, GUILayout.Width(25), line_h);
                }
            }
        GUILayout.EndHorizontal();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* NOTES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private List<NoteData> deleteNotes = new List<NoteData>();
    private List<NoteData> newNotes = new List<NoteData>();

    int DrawNotes(BallData ball)
    {
        // STYLE
        GUIStyle s = new GUIStyle(GUI.skin.button);
        s.alignment = TextAnchor.MiddleLeft;

        GUIStyle b = new GUIStyle(GUI.skin.button);
        b.alignment = TextAnchor.MiddleCenter;

        var col_field_width = GUILayout.Width(40);
        var beat_field_width = GUILayout.Width(45);
        var dir_field_width = GUILayout.Width(100);
        var line_h = GUILayout.Height(20);

        deleteNotes.Clear();
        newNotes.Clear();

        // --------- BEGIN NOTES ITERATION --------------------------
        foreach(NoteData note in ball.notes)
        {
            if(note == null)
            {
                deleteNotes.Add(note);
                Debug.LogWarning("Deleted a null note from " + songData.name);
                break;
            }

            GUILayout.BeginHorizontal();
                GUILayout.Space(31.0f);

                GUIStyle cool = new GUIStyle(GUI.skin.button);
                cool.padding = new RectOffset(2,2,2,2);

                ChangeColor(Color.green);
                //________Add New Note___________________
                if (GUILayout.Button(addNoteTexture, cool, GUILayout.Width(20), line_h))
                {
                    newNotes.Add(note); // Mark note to be added after iteration
                }
                ResetColor();

                //________Remove Note___________________
                ChangeColor(Color.red);
                if (GUILayout.Button(deleteNoteTexture, cool, GUILayout.Width(20), line_h))
                {
                    deleteNotes.Add(note); // Mark note to be deleted after iteration
                }
                ResetColor();

                //________Set Column___________________
                GUILayout.Label("Col:", GUILayout.Width(25), line_h);
                int new_col = EditorGUILayout.IntField("", note.hitPosition, s, col_field_width, line_h);
                if(note.hitPosition != new_col)
                {
                    note.hitPosition = new_col;
                    EditorUtility.SetDirty(note);
                }

                //________Set Hit Beat___________________
                GUILayout.Label("Beat:", GUILayout.Width(32), line_h);
                float new_beat = EditorGUILayout.FloatField("", note.hitBeat, s, beat_field_width, line_h);
                if(note.hitBeat != new_beat)
                {
                    note.hitBeat = new_beat;
                    EditorUtility.SetDirty(note);
                    //ball.SortNotes();
                }

                //________Set Direction___________________
                GUILayout.Label("Direction:", GUILayout.Width(56), line_h);
                note.noteDirection = (Direction)EditorGUILayout.EnumPopup("", note.noteDirection, s, dir_field_width, line_h);

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
            SongEdit.InsertNote(ball, note, ball.notes.IndexOf(note));
        }

        ResetColor();

        return ball.notes.Count;
    }

    static void ModeChanged ()
    {
        FindObjectOfType<Game>().SortBalls();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private bool isLabelVisible(BallLabel label)
    {
        float scrollPos = ballDataScrollPosition.y;
        float scrollHeight = viewSection.height;
        float bottom = label.yMax;

        // Check Top
        if(scrollPos > bottom)
        {
            return false;
        }
        // Check Bottom
        if(bottom >= scrollPos + scrollHeight)
        {
            return false;
        }
        return true;
    }

    void CheckBallActivity(BallData ball, Color oldColor, Color setColor)
    {
        Profiler.BeginSample("Ball Activity Sample");

        float timeDifference = Mathf.Abs(ball.notes[0].hitBeat - songController.GetSongTimeBeats());
        if (timeDifference == 0.0f)
            timeDifference = 0.001f;
        if (timeDifference < 8.0f)
            GUI.backgroundColor = oldColor + (setColor/(timeDifference));
        else
            GUI.backgroundColor = oldColor;
        Repaint();

        Profiler.EndSample();
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

    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding/2;
        r.x -= 2;
        r.width += 6;
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
