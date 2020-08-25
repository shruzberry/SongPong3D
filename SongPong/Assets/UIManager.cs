using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //_____ SETTINGS ____________________
    //_____ REFERENCES __________________
    private Game game;

    //_____ COMPONENTS __________________
    [Header("Components")]
    public Image songLogo_img;

    //_____ ATTRIBUTES __________________
    private Sprite songLogo;

    //_____ STATE  ______________________
    //_____ OTHER _______________________



    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        InitSongLogo();
    }

    private void InitSongLogo()
    {
        songLogo = game.songData.songLogo;
        songLogo_img.sprite = songLogo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
