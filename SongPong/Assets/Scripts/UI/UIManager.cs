using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    //_____ SETTINGS ____________________

    //_____ REFERENCES __________________
    private Game game;
    private InputMaster input;

    //_____ COMPONENTS __________________
    [Header("Components")]
    public GameObject EndGameOverlay;
    public GameObject InGameOverlay;
    public GameObject PauseGameOverlay;
    public Image songLogo_img;
    public Image songLogo_end;

    //_____ ATTRIBUTES __________________
    private Sprite songLogo;

    //_____ STATE  ______________________

    //_____ OTHER _______________________


    // Start is called before the first frame update
    public void Initialize(Game game, InputMaster input)
    {
        this.game = game;
        this.input = input;

        input.UI.TogglePauseMenu.performed += TogglePauseGameUI;
        
        game.onGameStart += ShowInGameUI;
        game.onGameEnd += ShowEndGameUI;
        game.onGameRestart += ShowInGameUI;
        game.onGameResume += ResumeGame;

        PauseGameOverlay.SetActive(false);

        InitSongLogo();
    }

    public void Restart()
    {
        ShowInGameUI();
    }

    public void ResumeGame()
    {
        PauseGameOverlay.SetActive(false);
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

    public void TogglePauseGameUI(InputAction.CallbackContext context)
    {
        if(PauseGameOverlay.activeSelf == false)
        {
            PauseGameOverlay.SetActive(true);
            game.PauseGame();
        }
        else
        {
            PauseGameOverlay.SetActive(false);
            game.ResumeGame();
        }
    }

    private void InitSongLogo()
    {
        songLogo = game.songData.songLogo;
        songLogo_img.sprite = songLogo;
        songLogo_end.sprite = songLogo;
    }

}
