using UnityEngine;

/**
 * Automatically pauses the game when the player moves the mouse outside of the editor window
 */
public class PlayOnHover : MonoBehaviour
{
    public float gameSpeed = 1.0f;
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
        song = FindObjectOfType<SongController>();
        input = new InputMaster();
        input.NoteListener.MousePos.performed += mov => mousePos = mov.ReadValue<Vector2>();
    }

    void Update()
    {
        if(Application.isPlaying && song.isLoaded)
            CheckMouseHover();
    }

    void CheckMouseHover()
    {
        if(song.isPlaying && song.hasStarted && !isMouseOverGame()) //fullWindow.Contains(Event.current.mousePosition)
        {
            song.Pause();
            Time.timeScale = 0.0f;
        }
        if(!song.isPlaying && song.hasStarted && isMouseOverGame()) //!fullWindow.Contains(Event.current.mousePosition)
        {
            song.Play();
            Time.timeScale = gameSpeed;
        }
    }

    bool isMouseOverGame()
    {
        var view = Camera.main.ScreenToViewportPoint(mousePos);
        var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        return !isOutside;
    }
}
