using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * A toggleable tool that listens for clicks on the screen and adds them to a list as notes
 */
public class NoteListener : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    //_____ SETTINGS ____________________
    //_____ REFERENCES __________________
    public Track track;
    public Keyboard keyboard;
    private InputMaster input;
    private SongController sc;
    private BallDropper bd;

    //_____ COMPONENTS __________________
    public List<NoteData> data;

    //_____ ATTRIBUTES __________________
    Vector2 mousePos;

    //_____ STATE  ______________________
    private bool active = true;

    //_____ OTHER _______________________

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
        track = FindObjectOfType<Track>();
        keyboard = InputSystem.GetDevice<Keyboard>();
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
        if(keyboard.spaceKey.wasPressedThisFrame && active)
        {
            /*
            NoteData nd = new NoteData();
            
            nd.hitPosition = track.GetNearestColumn(mousePos);
            nd.hitBeat = (int) sc.GetSongTimeBeats();
            nd.noteDirection = Direction.positive;

            data.Add(nd);
            */
           Debug.Log("CLICK: " + sc.GetSongTimeBeats());
        }
    }
}