using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ReplayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void OnReplayButtonClicked();
    public event OnReplayButtonClicked onReplayButtonClicked;

    public Color primaryColor;
    public Color hoverColor;

    public TextMeshProUGUI text;

    private void OnEnable() 
    {
        text.color = primaryColor;
    }

    public void ClickButton()
    {
        if(onReplayButtonClicked != null) onReplayButtonClicked();
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
