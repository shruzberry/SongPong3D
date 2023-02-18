using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    //_____ SETTINGS ____________________

    //_____ REFERENCES __________________
    private Game game;

    //_____ COMPONENTS __________________
    [Header("Components")]
    public EndgameUI endgameUI;
    public IngameUI ingameUI;
    public GameObject PauseGameOverlay;

    //_____ ATTRIBUTES __________________
    private Sprite songLogo;

    //_____ STATE  ______________________

    //_____ OTHER _______________________


    // Start is called before the first frame update
    public void Initialize(Game game, InputMaster input)
    {
        this.game = game;

        input.UI.TogglePauseMenu.performed += TogglePauseGameUI;

        // Initialize all UIs
        ingameUI.gameObject.SetActive(true);
        endgameUI.gameObject.SetActive(true);
        ingameUI.Initialize();
        endgameUI.Initialize();

        game.onGameStart += ShowInGameUI;
        game.onGameRestart += ShowInGameUI;

        game.onGameEnd += ShowEndGameUI;

        game.onGameResume += ResumeGame;

        PauseGameOverlay.SetActive(false);
    }

    public void ResumeGame()
    {
        PauseGameOverlay.SetActive(false);
    }

    public void ShowInGameUI()
    {
        endgameUI.DisableUI();
        ingameUI.EnableUI();
    }

    public void ShowEndGameUI()
    {
        endgameUI.EnableUI();
        ingameUI.DisableUI();
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
}
