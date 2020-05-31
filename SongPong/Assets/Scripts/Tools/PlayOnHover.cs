using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayOnHover : MonoBehaviour
{
    private SongController songController;
    bool songPlaying;
    private InputMaster input;
    private Vector2 mousePos;

    void OnEnable()
    { 
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Awake()
    {
        songController = GameObject.Find("SongController").GetComponent<SongController>();
        input = new InputMaster();
        input.NoteListener.MousePos.performed += mov => mousePos = mov.ReadValue<Vector2>();
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
        var view = Camera.main.ScreenToViewportPoint(mousePos);
        var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        return !isOutside;
    }
}
