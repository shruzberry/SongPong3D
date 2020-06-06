using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateIndicator : MonoBehaviour
{
    private PlayOnHover song;
    public Sprite playing;
    public Sprite paused;

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        song = FindObjectOfType<PlayOnHover>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(song.songPlaying)
        {
            image.sprite = playing;
        }
        else if(!song.songPlaying)
        {
            image.sprite = paused;
        }
    }
}
