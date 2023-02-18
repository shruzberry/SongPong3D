using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndgameUI : MonoBehaviour
{
    public TextMeshProUGUI songName_text;
    public TextMeshProUGUI artistName_text;

    private Canvas canvas;

    public void Initialize()
    {
        Game game = FindObjectOfType<Game>();
        canvas = GetComponent<Canvas>();

        songName_text.text = game.songData.name;
        artistName_text.text = game.songData.artistName;
    }

    public void EnableUI()
    {
        if(canvas != null) canvas.enabled = true;
    }

    public void DisableUI()
    {
        if(canvas != null) canvas.enabled = false;
    }
}
