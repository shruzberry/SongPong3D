using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateIndicator : MonoBehaviour
{
    private SongController song;
    public Sprite playing;
    public Sprite paused;

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        song = FindObjectOfType<SongController>();
        image = GetComponent<Image>();
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
}
