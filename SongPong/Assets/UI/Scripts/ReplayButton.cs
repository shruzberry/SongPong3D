using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ReplayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //_____ SETTINGS ____________________

    //_____ REFERENCES __________________
    private Game game;

    //_____ COMPONENTS __________________
    public TextMeshProUGUI text;

    //_____ ATTRIBUTES __________________
    public Color primaryColor;
    public Color hoverColor;

    //_____ STATE  ______________________
    //_____ OTHER _______________________


    private void OnEnable() 
    {
        text.color = primaryColor;
        game = FindObjectOfType<Game>();
    }

    public void ClickButton()
    {
        game.RestartGame();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        text.color = primaryColor;
    }

    public void OnPointerClick(PointerEventData data)
    {
    }
}
