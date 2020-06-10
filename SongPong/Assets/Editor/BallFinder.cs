﻿/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: BallFinder.cs
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


public class BallFinder : EditorWindow
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    List<NoteData> deleteNoteList = new List<NoteData>();

    SongController songController;
    SongData songData;
    SongData lastSong;

    float navBarSectionSize = .15f;
    
    // for navigation bar
    int navButtonHeight = 20;
    int navButtonWidth = 35;
    //int ballButtonHeight = 30;
    int ballButtonWidth = 100;

    int jumpToTime;

    Rect fullWindow;
    Rect navBarSection;
    Rect viewSection;

    Vector2 activeBallsScrollPosition;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* STARTUP FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    [MenuItem("Window/Ball Finder")]
    static void OpenWindow()
    {
        BallFinder window = (BallFinder)GetWindow(typeof(BallFinder), false, "Ball Finder");
        window.minSize = new Vector2(375, 375);
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
            songData = songController.songData;
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
        navBarSection.width = Screen.width;
        navBarSection.height = Screen.height * navBarSectionSize;

        viewSection.x = 0;
        viewSection.y = navBarSection.height;
        viewSection.width = Screen.width;
        viewSection.height = Screen.height - navBarSection.height;
    }

    void DrawNavSettings()
    {
        songController = GameObject.Find("SongController").GetComponent<SongController>();
        GUILayout.BeginArea(navBarSection);
            GUILayout.Label("Navigation");
            songData = (SongData)EditorGUILayout.ObjectField(songData, typeof(SongData), true, GUILayout.MaxWidth(187));
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
                    songController.JumpToBeat(songController.currentBeat - 8);
                }

                // Pause/Play
                if (GUILayout.Button("Play", GUILayout.Height(navButtonHeight), GUILayout.Width(navButtonWidth)))
                {
                    songController.JumpToBeat(jumpToTime);
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
                
                // Current Beat
                GUILayout.Label("Beat: " + songController.currentBeat, GUILayout.Width(60));

                // Song Slider
                jumpToTime = (int) EditorGUILayout.Slider((float)jumpToTime, songData.startBeat, songData.endBeat);

            EditorGUILayout.EndHorizontal();
            }
        GUILayout.EndArea();
    }

    void DrawBallData()
    {
        BallDropper dropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        List<Ball> activeBalls =  dropper.getActiveBalls();
        
        GUILayout.BeginArea(viewSection);
            activeBallsScrollPosition = GUILayout.BeginScrollView(activeBallsScrollPosition,
                                        GUILayout.Width(viewSection.width),
                                        GUILayout.Height(viewSection.height - 75));
                DrawBallDataList(dropper.getAllBallData(), Color.blue);
            GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void DrawBallDataList(BallData[] balls, Color color)
    {
        GUILayout.Label("Ball Data");
        GUIStyle s = new GUIStyle(GUI.skin.button);
        GUIStyle b = new GUIStyle(GUI.skin.button);
        s.alignment = TextAnchor.MiddleLeft;
        b.alignment = TextAnchor.MiddleCenter;
        var w = GUILayout.Width(100);

        foreach(BallData ball in balls)
        {
            Color oldColor = GUI.color;
            CheckBallActivity(ball, oldColor, Color.blue);
            GUILayout.BeginHorizontal();
                GUI.color = Color.green;
                if (GUILayout.Button("+", b, GUILayout.Width(25)))
                {
                    BallData bd = new BallData();
                    bd.type = BallTypes.simple;
                    bd.enabled = true;
                    bd.name = "NewBall";
                    NoteData nd = new NoteData();
                    nd.noteDirection = Direction.negative;
                    nd.hitPosition = 0;
                    nd.hitBeat = 0;
                    nd.name = "NewNote";
                    SongEdit.CreateSimple("NewBall", nd);
                }
                GUI.color = oldColor;  

                GUI.color = Color.red;
                if (GUILayout.Button("-", b, GUILayout.Width(25)))
                {
                    SongEdit.DeleteBall(ball);
                }
                GUI.color = oldColor; 

                //ball.name = EditorGUILayout.TextField("", ball.name, s, w);                
                ball.type = (BallTypes)EditorGUILayout.EnumPopup("", ball.type, s, w);
                ball.enabled = GUILayout.Toggle(ball.enabled, "Enabled", s, w);
            GUILayout.EndHorizontal();

            foreach(NoteData note in ball.notes)
            {
                GUILayout.BeginHorizontal();
                    GUILayout.Space(31.0f);
                    //add Note Button
                    GUI.color = Color.green;
                    if (GUILayout.Button("+", b, GUILayout.Width(25)))
                    {
                        NoteData nd = new NoteData();
                        nd.hitPosition = 0;
                        nd.hitBeat = 0;
                        nd.noteDirection = Direction.negative;
                        nd.name = "NewNote";
                        SongEdit.saveNote(nd);
                        SongEdit.AppendToBall(ball, nd);
                    }
                    GUI.color = oldColor;

                    GUI.color = Color.red;
                    if (GUILayout.Button("-", b, GUILayout.Width(25)))
                    {
                        SongEdit.DeleteNote(ball, note);
                    }
                    GUI.color = oldColor;

                    //note.name = EditorGUILayout.TextField("", note.name, s, w);
                    note.hitPosition = EditorGUILayout.IntField("", note.hitPosition, s, w); 
                    note.hitBeat = EditorGUILayout.FloatField("", note.hitBeat, s, w);                
                    note.noteDirection = (Direction)EditorGUILayout.EnumPopup("", note.noteDirection, s, w);

                GUILayout.EndHorizontal();
            }
            GUI.color = oldColor;
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

    public void Update()
    {
        Repaint();
    }
}

