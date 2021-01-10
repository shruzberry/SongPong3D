using UnityEngine;
using UnityEngine.UI;

public class GameStateIndicator : MonoBehaviour
{
    private SongController song;
    public Sprite playing;
    public Sprite paused;
    private Vector2 mousePos;

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        song = FindObjectOfType<SongController>();
        image = GetComponent<Image>();
        image.sprite = playing;
    }

    // Update is called once per frame
    void Update()
    {
        if(song.isPlaying)
        {
            image.sprite = playing;
        }
        else if(!song.isPlaying)
        {
            image.sprite = paused;
        }
    }

    bool isMouseOverGame()
    {
        var view = Camera.main.ScreenToViewportPoint(mousePos);
        var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        return !isOutside;
    }
}
