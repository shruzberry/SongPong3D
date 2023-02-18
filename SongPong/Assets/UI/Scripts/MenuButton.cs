using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour
{
    private void Awake() 
    {
        GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    public void GoToMenu()
    {
        FindObjectOfType<Game>().ResumeGame();
        FindObjectOfType<LevelChanger>().ReturnToMenu();
    }
}
