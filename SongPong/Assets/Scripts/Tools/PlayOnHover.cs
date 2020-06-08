﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayOnHover : MonoBehaviour
{
    private SongController song;
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
        song = GameObject.FindObjectOfType<SongController>();
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
        if(song.isPlaying && !isMouseOverGame()) //fullWindow.Contains(Event.current.mousePosition)
        {
            song.Pause();
            Time.timeScale = 0.0f;
        }
        if(!song.isPlaying && isMouseOverGame()) //!fullWindow.Contains(Event.current.mousePosition)
        {
            song.Play();
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
