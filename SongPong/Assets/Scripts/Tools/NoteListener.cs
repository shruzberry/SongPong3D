/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: NoteListener.cs
Purpose: A toggleable tool that listens for clicks on the screen and
adds them to a list as notes

________ USAGE ________
* Create an instance of NoteListener
* Call NoteListener.Enable();
* Click around the game view at different times
* Access NoteListener.data;

________ ATTRIBUTES ________
+ NoteData data

________ FUNCTIONS ________
+ Enable()
+ Disable()
+ Clear()
+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using UnityEngine.InputSystem;

public class NoteListener : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public List<NoteData> data;

    private bool active = true;
    private InputMaster input;
    private SongController sc;
    private BallDropper bd;

    Vector2 mousePos;
    
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Enable(){active = true;}

    public void Disable(){active = false;}

    public void Clear(){data.Clear();}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

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
        sc = FindObjectOfType<SongController>();
        bd = FindObjectOfType<BallDropper>();

        input = new InputMaster();
        input.NoteListener.MousePos.performed += mov => mousePos = mov.ReadValue<Vector2>();
    }
    
    void Update()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        if(kb.spaceKey.wasPressedThisFrame && active)
        {
            NoteData nd = new NoteData();
            
            nd.hitPosition = bd.GetNearestColumn(mousePos);
            nd.hitBeat = (int) sc.GetSongTimeBeats();
            nd.noteDirection = Direction.positive;

            data.Add(nd);
        }
    }
}