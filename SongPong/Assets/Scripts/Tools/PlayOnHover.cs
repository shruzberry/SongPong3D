using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnHover : MonoBehaviour
{
    private SongController songController;
    bool songPlaying;

    void Awake()
    {
        songController = GameObject.Find("SongController").GetComponent<SongController>(); 
    }

    void Update()
    {
        if(Application.isPlaying)
            CheckMouseHover();
    }

    void CheckMouseHover()
    {
        if(songPlaying && !isMouseOverGame()) //fullWindow.Contains(Event.current.mousePosition)
        {
            songController.Pause();
            songPlaying = false;
            Time.timeScale = 0.0f;
        }
        if(!songPlaying && isMouseOverGame()) //!fullWindow.Contains(Event.current.mousePosition)
        {
            songController.Play();
            songPlaying = true;
            Time.timeScale = 1.0f;
        }    
    }

    bool isMouseOverGame()
    {
        var view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        //return (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x == Screen.width - 1 || Input.mousePosition.y == Screen.height - 1);
        return !isOutside;
    }
}
