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

public class MenuLoader : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public List<SongData> songs;
    public Button buttonPrefab;
    public float verticalOffset;
    public int spacing;

    private Vector2 position;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Start()
    {
        position = new Vector2(0.0f, verticalOffset);
        loadAllSongs();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void loadAllSongs()
    {
        foreach(SongData song in songs)
        {
            AddButton(buttonPrefab, song.name);
            print("laoding song: " + song.name);
        }
    }

    private void AddButton(Button prefab, string name)
    {
        Button button = Instantiate(prefab);
        button.name = name;
        //button.onClick.AddListener(callback);
 
        Text text = button.GetComponentInChildren<Text>();
        if (text)
            text.text = name;
 
        RectTransform transform = button.GetComponent<RectTransform>();
        transform.SetParent(GameObject.Find("Menu").transform, false);
        transform.anchoredPosition = position;
        position = position + (Vector2.down * spacing);
    }
}
