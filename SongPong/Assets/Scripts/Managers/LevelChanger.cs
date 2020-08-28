using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Load game into the Song scene for a specific SongData
 */
public class LevelChanger : MonoSingleton<LevelChanger>
{
    //_____ SETTINGS ____________________
    private bool firstLevelLoaded = true; // check if launch directly into the Song scene

    //_____ REFERENCES __________________
    private Game game;
    private SongController songController;
    
    //_____ COMPONENTS __________________
    public Animator animator;
    public SongData songData;
    public Canvas canvas;

    //_____ ATTRIBUTES __________________
    private int levelToLoad;

    //_____ STATE  ______________________

    //_____ EVENTS _______________________


/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* INITIALIZE
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void Awake()
    {
        base.Awake();
    
        animator = this.GetComponent<Animator>();
        game = FindObjectOfType<Game>(); 

        // Find the return to menu button in the Song scene
        if(SceneManager.GetActiveScene().buildIndex == 1) 
        {
            FindObjectOfType<MenuButton>().onMenuButtonClicked += ReturnToMenu;
        }
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // If launched directly into Song scene, bypassing Menu
        if(firstLevelLoaded)
        {
            CheckForNonMenuPlay();
            firstLevelLoaded = false;
            return;
        }

        PopulateGame();
        CheckForSongController();
        
        animator.SetTrigger("FadeIn");
    }

    private void CheckForNonMenuPlay()
    {
        // if we start the game in song scene
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            game.InitializeEditor();
        }
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        if(songController!= null) songController.onSongEnd -= ReturnToMenu;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* CHANGE LEVELS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void ReturnToMenu()
    {
        FadeToLevel(0);
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    private void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void PopulateGame()
    {
        game = FindObjectOfType<Game>();
        
        if(game != null)
        {
            game.Initialize(songData);
        }
    }
 
    private void CheckForSongController()
    {
        songController = FindObjectOfType<SongController>();
        if (songController != null)
        {
            songController.onSceneEnd += ReturnToMenu;
        }
    }

    public void SetSong(SongData sd)
    {
        songData = sd;
    }
}
