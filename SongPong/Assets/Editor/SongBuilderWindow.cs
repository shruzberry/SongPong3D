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
using UnityEditor;

public class SongBuilderWindow : EditorWindow 
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    //give each section a texture, rect, and maybe color
    Texture2D headerSectionTexture;

    Color headerSectionColor = new Color(0, 1, 1, 1);

    Rect headerSection;

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
        headerSectionTexture = new Texture2D(1,1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();
    }

    // Update function called every time we interact with window
    void OnGUI()
    {
        DrawLayouts();
    }

    // Paint textures based on Rect object data
    void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 25;

        GUI.DrawTexture(headerSection, headerSectionTexture);
    }

    // Put all components in a draw region of their own
}
