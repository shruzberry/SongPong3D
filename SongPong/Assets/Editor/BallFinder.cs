/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
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

public class BallFinder : EditorWindow
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    SongController songController;
    SongData songData;

    float navBarSectionSize = .15f;
    
    // for navigation bar
    int navButtonHeight = 20;
    int navButtonWidth = 35;
    int ballButtonHeight = 30;
    int ballButtonWidth = 100;

    int jumpToTime;

    Rect navBarSection;
    Rect viewSection;

    Vector2 activeBallsScrollPosition;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* STARTUP FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    [MenuItem("Window/BallFinder")]
    static void OpenWindow()
    {
        BallFinder window = (BallFinder)GetWindow(typeof(BallFinder), false, "Ball Finder");
        window.minSize = new Vector2(375, 375);
        window.Show();
    }

    void OnEnable()
    {

    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* GUI DRAW FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnGUI()
    {
        DrawRowLayouts();
        DrawNavSettings();
        DrawActiveBalls();
    }

    void DrawRowLayouts()
    {
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
                // Drag in Song

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

    void DrawActiveBalls()
    {
        BallDropper dropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        List<Ball> activeBalls =  dropper.getActiveBalls();
        
        GUILayout.BeginArea(viewSection);
            GUILayout.Label("Active Balls");
            
            activeBallsScrollPosition = GUILayout.BeginScrollView(activeBallsScrollPosition, GUILayout.Width(viewSection.width), GUILayout.Height(viewSection.height - 75));
                foreach(Ball ball in activeBalls)
                {
                    EditorGUILayout.ObjectField(ball.ballData, typeof(Object), true);
                }
            GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public void Update()
    {
        // This is necessary to make the framerate normal for the editor window.
        Repaint();
    }
}

