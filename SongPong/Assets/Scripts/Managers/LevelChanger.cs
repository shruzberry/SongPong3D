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
        CheckForNonMenuPlay();
    }

    private void Update() 
    {
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
        AxisManager axisManager = GameObject.Find("Game").GetComponent<AxisManager>();
        axisManager.gameAxis = song.axis;
        
        //PaddleManager paddleManager = GameObject.Find("PaddleManager").GetComponent<PaddleManager>();
        //paddleManager.Enable();

        songLoaded = true;
        this.gameObject.SetActive(false);
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();
        songController.LoadSong(song);

        BallDropper ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        ballDropper.Activate();  
    }

    private void CheckForNonMenuPlay()
    {
        // if we start the game in song scene
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            //PaddleManager paddleManager = GameObject.Find("PaddleManager").GetComponent<PaddleManager>();
            //paddleManager.Enable();
        }
    }
}
