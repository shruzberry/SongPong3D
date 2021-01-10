/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: MenuLoader.cs
Purpose: Loads all songs in SongData Foler into the select menu as buttons

________ USAGE ________
* Attach to GameObject
* Connect it to the UI Menu Object
* Call .LoadAllSongs();

________ ATTRIBUTES ________


________ FUNCTIONS ________
+ LoadAllSongs();

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* DEPENDENCIES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Types;

public class MenuLoader : MonoBehaviour
{
    public Button buttonPrefab;
    public float verticalOffset;

    public int spacing;

    private Vector2 position;
    private Object[] songs;

    LevelChanger levelChanger;

    public void Awake()
    {
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }
    
    public void Start()
    {
        position = new Vector2(0.0f, verticalOffset);
        loadAllSongs();
        createSongButtons();
    }

    private void loadAllSongs()
    {
        songs = Resources.LoadAll("SongData", typeof(SongData));
    }

    private void createSongButtons()
    {
        foreach(SongData song in songs)
        {
            AddButton(buttonPrefab, song);
        }
    }

    private void AddButton(Button prefab, SongData sd)
    {
        // Create Button
        Button button = Instantiate(prefab);
        button.name = sd.songName;
 
        // Create Label
        Text text = button.GetComponentInChildren<Text>();
        if (text)
            text.text = sd.songName;

        // Add scene transition listener
        button.onClick.AddListener(() => 
        {
            levelChanger.SetSong(sd);
            levelChanger.FadeToNextLevel();
        });
 
        // Put Under Menu in hierarchy
        RectTransform transform = button.GetComponent<RectTransform>();
        transform.SetParent(GameObject.Find("Menu").transform, false);
        transform.anchoredPosition = position;
        position = position + (Vector2.down * spacing);
    }
}
