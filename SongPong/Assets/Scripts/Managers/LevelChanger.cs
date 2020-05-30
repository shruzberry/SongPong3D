using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoSingleton<LevelChanger>
{
    
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public Animator animator;
    public SongData song;
    
    private int levelToLoad;
    private bool songLoaded = false;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void Awake()
    {
        base.Awake();
        animator = this.GetComponent<Animator>();
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            FadeToNextLevel();
        }

        if(SceneManager.GetActiveScene().buildIndex == 1 && !songLoaded)
            SongInit();
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

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SetSong(SongData sd)
    {  
        song = sd;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void SongInit()
    {
        songLoaded = true;
        this.gameObject.SetActive(false);
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();
        songController.LoadSong(song);

        BallDropper ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        ballDropper.Activate();
    }
}
