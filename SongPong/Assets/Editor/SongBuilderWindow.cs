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
    //give each section a texture, rect, and maybe color
    float navBarSectionSize = .1f;
    float createSectionSize = .45f;
    float editSectionSize = .45f;
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
    Rect editBallsSection;
    Rect editNotesSection;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    [MenuItem("Window/Song Builder")]
    static void OpenWindow()
    {
        SongBuilderWindow window = (SongBuilderWindow)GetWindow(typeof(SongBuilderWindow), false, "Song Builder");
        window.minSize = new Vector2(250, 250);
        window.Show();
    }

    //The Awake Function
    void OnEnable()
    {
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

    // Update function called every time we interact with window
    void OnGUI()
    {
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
        createSection.y = Screen.height * (navBarSectionSize);
        createSection.width = Screen.width - (Screen.width * activeBallsSectionWidth);
        createSection.height = Screen.height * createSectionSize;

        editSection.x = Screen.width * activeBallsSectionWidth;
        editSection.y = Screen.height * (navBarSectionSize + createSectionSize);
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
    }

    void DrawNavSettings()
    {
        GUILayout.BeginArea(navBarSection);
            GUILayout.Label("Navigation");
        GUILayout.EndArea();
    }
    
    void DrawActiveBalls()
    {
        GUILayout.BeginArea(activeBallsSection);
            GUILayout.Label("Active Balls");
        GUILayout.EndArea();
    }

    void DrawAddNote()
    {
        GUILayout.BeginArea(addNoteSection);
            GUILayout.Label("Add Note");
        GUILayout.EndArea();
    }

    void DrawAddBall()
    {
        GUILayout.BeginArea(addBallSection);
            GUILayout.Label("Create Ball From Notes");
        GUILayout.EndArea();
    }

    void DrawEditBall()
    {

    }

    void DrawEditNote()
    {

    }
}