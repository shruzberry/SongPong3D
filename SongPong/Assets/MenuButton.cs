using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void OnMenuButtonClicked();
    public event OnMenuButtonClicked onMenuButtonClicked;

    public Color primaryColor;
    public Color hoverColor;

    public TextMeshProUGUI text;

    private void OnEnable() 
    {
        text.color = primaryColor;
    }

    public void ClickButton()
    {
        if(onMenuButtonClicked != null) onMenuButtonClicked();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        text.color = primaryColor;
    }
}
