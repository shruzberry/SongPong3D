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
    public GameObject EndGameOverlay;
    public GameObject InGameOverlay;
    public Image songLogo_img;
    public Image songLogo_end;

    //_____ ATTRIBUTES __________________
    private Sprite songLogo;

    //_____ STATE  ______________________
    //_____ OTHER _______________________


    // Start is called before the first frame update
    public void Initialize()
    {
        game = FindObjectOfType<Game>();
        
        game.onGameStart += ShowInGameUI;
        game.onGameEnd += ShowEndGameUI;
        game.onGameRestart += ShowInGameUI;

        InitSongLogo();
    }

    public void Restart()
    {
        ShowInGameUI();
    }

    public void ShowInGameUI()
    {
        EndGameOverlay.SetActive(false);
        InGameOverlay.SetActive(true);
    }

    public void ShowEndGameUI()
    {
        EndGameOverlay.SetActive(true);
        InGameOverlay.SetActive(false);
    }

    private void InitSongLogo()
    {
        songLogo = game.songData.songLogo;
        songLogo_img.sprite = songLogo;
        songLogo_end.sprite = songLogo;
    }

}
