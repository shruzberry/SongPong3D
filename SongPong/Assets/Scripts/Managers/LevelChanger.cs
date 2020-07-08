/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=

________ DEFENITION ________
Class Name: Level Changer
Purpose: Loads the game into the Song scene set up for a specific song

__________ USAGE ___________
* Attach to GameObject
* Load in songData
* Transition to Song scene

_________ PUBLIC ___________
+ FadeToNextLevel(): Fade to next scene index
    - Song should index 1
+ FadeToLevel(int levelIndex): Fade to specific scene
+ SetSong(SongData sd): Set song data to be loaded in
    - Call before you transition scene

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoSingleton<LevelChanger>
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // REFERENCES
    public Animator animator;
    public SongData songData;
    private Game game;
    private SongController songController;

    public delegate void OnGameLoaded();
    public event OnGameLoaded onGameLoaded;

    private int levelToLoad;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void Awake()
    {
        base.Awake();

        animator = this.GetComponent<Animator>();
        game = FindObjectOfType<Game>(); 
    }

    public void Start()
    {
        CheckForNonMenuPlay();
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        bool nonMenu = CheckForNonMenuPlay();
        if(!nonMenu) PopulateGame();
        CheckForSongController();
        animator.SetTrigger("FadeIn");
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        if (songController != null)
            songController.onSongEnd -= ReturnToMenu;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void SetSong(SongData sd)
    {
        songData = sd;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private bool CheckForNonMenuPlay()
    {
        // if we start the game in song scene
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            bool success = game.InitializeEditor();

            if(success)
            {
                if(onGameLoaded != null)
                {
                    onGameLoaded();
                }
                return true;
            }
        }
        return false;
    }

    private void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

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
            songController.onSongEnd += ReturnToMenu;
        }
    }

    private void ReturnToMenu()
    {
        Invoke("FadeToMenu", 2);
    }

    private void FadeToMenu() {FadeToLevel(0);}
}
