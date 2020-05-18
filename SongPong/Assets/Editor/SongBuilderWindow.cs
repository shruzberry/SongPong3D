/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: SongBuilder.cs
Purpose: A windowed editor extension that creates note and ball data for the resource folder
Associations: 

________ USAGE ________
* Go to Windows/Song Builder to open the editor

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

public class SongBuilderWindow : EditorWindow 
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    
    // Values for functionality
    public SongData songData;
    SongController songController;
    float jumpToTime;

    // actual control functionality
    Vector2 addBallScrollPosition;
    Vector2 activeBallsScrollPosition;
    Vector2 editBallScrollPosition;

    bool toggleNoteListener = false;
    string noteColumn = "0";
    string noteBeat = "0";
    
    // for navigation bar
    int navButtonHeight = 20;
    int navButtonWidth = 35;

    int ballButtonHeight = 30;
    int ballButtonWidth = 100;

    // formatting
    float navBarSectionSize = .15f;
    float createSectionSize = .425f;
    float editSectionSize = .425f;
    float activeBallsSectionWidth = .33f;

    Texture2D navBarSectionTexture;
    Texture2D createSectionTexture;
    Texture2D editSectionTexture;

    Color navBarSectionColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    Color createSectionColor = new Color(0, 0, 0.75f, 0.5f);
    Color editSectionColor = new Color(0.75f, 0 , 0, 0.5f);

    Rect navBarSection;
    Rect createSection;
    Rect editSection;
    Rect activeBallsSection;
    Rect addNoteSection;
    Rect addBallSection;
    Rect editBallSection;
    Rect editNoteSection;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    [MenuItem("Window/Song Builder")]
    static void OpenWindow()
    {
        SongBuilderWindow window = (SongBuilderWindow)GetWindow(typeof(SongBuilderWindow), false, "Song Builder");
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    //The Awake Function
    void OnEnable()
    {
        songController = GameObject.Find("SongController").GetComponent<SongController>();
        InitTextures();
    }

    void InitTextures()
    {
        navBarSectionTexture = new Texture2D(1,1);
        navBarSectionTexture.SetPixel(0, 0, navBarSectionColor);
        navBarSectionTexture.Apply();

        createSectionTexture = new Texture2D(1,1);
        createSectionTexture.SetPixel(0, 0, createSectionColor);
        createSectionTexture.Apply();

        editSectionTexture = new Texture2D(1,1);
        editSectionTexture.SetPixel(0, 0, editSectionColor);
        editSectionTexture.Apply();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* ON GUI DRAW FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // Update function called every time we interact with window
    void OnGUI()
    {
        InitTextures();
        DrawRowLayouts();
        DrawColumLayouts();
        DrawNavSettings();
        DrawActiveBalls();
        DrawAddNote();
        DrawAddBall();
        DrawEditBall();
        DrawEditNote();
    }

    // Paint textures based on Rect object data
    void DrawRowLayouts()
    {
        navBarSection.x = 0;
        navBarSection.y = 0;
        navBarSection.width = Screen.width;
        navBarSection.height = Screen.height * navBarSectionSize;

        createSection.x = Screen.width * activeBallsSectionWidth;
        createSection.y = navBarSection.height;
        createSection.width = Screen.width - (Screen.width * activeBallsSectionWidth);
        createSection.height = Screen.height * createSectionSize;

        editSection.x = Screen.width * activeBallsSectionWidth;
        editSection.y = createSection.y + createSection.height;
        editSection.width = Screen.width - (Screen.width * activeBallsSectionWidth);
        editSection.height = Screen.height * editSectionSize;

        GUI.DrawTexture(navBarSection, navBarSectionTexture);
        GUI.DrawTexture(createSection, createSectionTexture);
        GUI.DrawTexture(editSection, editSectionTexture);
    }

    void DrawColumLayouts()
    {
        activeBallsSection.x = 0;
        activeBallsSection.y = Screen.height * (navBarSectionSize);
        activeBallsSection.width = Screen.width * activeBallsSectionWidth;
        activeBallsSection.height = Screen.height * (1.0f - navBarSectionSize);

        addNoteSection.x = activeBallsSection.width;
        addNoteSection.y = Screen.height * (navBarSectionSize);
        addNoteSection.width = createSection.width / 2;
        addNoteSection.height = createSection.height;

        addBallSection.x = activeBallsSection.width + addNoteSection.width;
        addBallSection.y = Screen.height * (navBarSectionSize);
        addBallSection.width = createSection.width / 2;
        addBallSection.height = createSection.height;

        editBallSection.x = activeBallsSection.width;
        editBallSection.y = addNoteSection.y + addNoteSection.height;
        editBallSection.width = editSection.width / 2;
        editBallSection.height = editSection.height;

        editNoteSection.x = activeBallsSection.width + addNoteSection.width;
        editNoteSection.y = addBallSection.y + addBallSection.height;
        editNoteSection.width = editSection.width / 2;
        editNoteSection.height = editSection.height;
    }

    void DrawNavSettings()
    {
        songController = GameObject.Find("SongController").GetComponent<SongController>();

        GUILayout.BeginArea(navBarSection);
            GUILayout.Label("Navigation");
            EditorGUILayout.BeginHorizontal();
                // Drag in Song
                songData = (SongData)EditorGUILayout.ObjectField(songData, typeof(SongData), true, GUILayout.MaxWidth(200));

                // Restart Song
                if (GUILayout.Button("<<", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToStart();
                }

                // Go Back 8 beats
                if (GUILayout.Button("<", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToBeat(songController.currentBeat - 8);
                }

                // Pause/Play
                if (GUILayout.Button("Play", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToTime(jumpToTime);
                }

                // Go Forward 8 beats
                if (GUILayout.Button(">", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToBeat(songController.currentBeat + 8);
                }

                // Go to end of song
                if (GUILayout.Button(">>", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToEnd();
                }

                // Song Slider

                jumpToTime = EditorGUILayout.Slider(jumpToTime, songData.startTime, songData.endTime);
            EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    
    void DrawActiveBalls()
    {
        GUILayout.BeginArea(activeBallsSection);
            GUILayout.Label("Active Balls");
            
            activeBallsScrollPosition = GUILayout.BeginScrollView(activeBallsScrollPosition, GUILayout.Width(activeBallsSection.width), GUILayout.Height(activeBallsSection.height - 75));
                for(int i = 0; i < 30; i++)
                {
                    if (GUILayout.Button("example ball", GUILayout.Height(ballButtonHeight), GUILayout.Width(activeBallsSection.width - 20)));
                    {
                        
                    }
                }
            GUILayout.EndScrollView();
            
            if (GUILayout.Button("Delete Ball", GUILayout.Height(ballButtonHeight), GUILayout.Width(activeBallsSection.width - 5)));
            {
                DeleteBall();
            }
        GUILayout.EndArea();
    }

    void DrawAddNote()
    {
        GUILayout.BeginArea(addNoteSection);
            GUILayout.Label("Note Buffer:");
            addBallScrollPosition = GUILayout.BeginScrollView(addBallScrollPosition, GUILayout.Width(addNoteSection.width - 10), GUILayout.Height(addNoteSection.height - 40));
                for(int i = 0; i < 30; i++)
                {
                    GUILayout.Label("(Col: 5, Beat: 126)");
                }
            GUILayout.EndScrollView();
            toggleNoteListener = GUILayout.Toggle(toggleNoteListener, "Listen for Notes");
        GUILayout.EndArea();
        
    }

    void DrawAddBall()
    {
        GUILayout.BeginArea(addBallSection);
            GUILayout.Label("Create Ball");

            // Basic
            if (GUILayout.Button("Basic", GUILayout.Height(ballButtonHeight), GUILayout.Width(addBallSection.width)));
            {
                
            }

            // Bounce
            if (GUILayout.Button("Bounce", GUILayout.Height(ballButtonHeight), GUILayout.Width(addBallSection.width)));
            {
                    
            }

        GUILayout.EndArea();
    }

    void DrawEditBall()
    {
        GUILayout.BeginArea(editBallSection);
            GUILayout.Label("Edit Ball");

            editBallScrollPosition = GUILayout.BeginScrollView(editBallScrollPosition, GUILayout.Width(addNoteSection.width), GUILayout.Height(addNoteSection.height - 75));
                for(int i = 0; i < 30; i++)
                {
                    if (GUILayout.Button("(Col: 5, Beat: 126)", GUILayout.Height(ballButtonHeight), GUILayout.Width(activeBallsSection.width - 20)));
                    {
                        
                    }
                }        
            GUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
                // Insert Note
                if (GUILayout.Button("Add Note", GUILayout.Height(ballButtonHeight), GUILayout.Width(editBallSection.width/2)));
                {
                    AddNote();
                }

                // Delete Note
                if (GUILayout.Button("Delete Note", GUILayout.Height(ballButtonHeight), GUILayout.Width(editBallSection.width/2 - 10)));
                {
                    DeleteNote();
                }
            EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void DrawEditNote()
    {
        GUILayout.BeginArea(editNoteSection);
            GUILayout.Label("Edit Note");
            EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Column: ");
                noteColumn = GUILayout.TextField(noteColumn, 6);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Beat: ");
                noteBeat = GUILayout.TextField(noteBeat, 6);
            EditorGUILayout.EndHorizontal();

            // Update
            GUILayout.Space(editNoteSection.height - 112);
            if (GUILayout.Button("Update", GUILayout.Height(ballButtonHeight), GUILayout.Width(editNoteSection.width)));
            {
                UpdateNote(System.Convert.ToInt32(noteColumn), System.Convert.ToInt32(noteBeat));
            }
        GUILayout.EndArea();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* TRIGGERED FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void DeleteBall()
    {

    }

    void AddNote()
    {

    }

    void DeleteNote()
    {

    }

    void UpdateNote(int col, int beat)
    {

    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Start()
    {
        
    }

    void Update()
    {

    }
}