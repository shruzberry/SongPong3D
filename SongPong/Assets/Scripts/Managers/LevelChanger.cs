using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoSingleton<LevelChanger>
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public Animator animator;
    public SongData songData;

    private int levelToLoad;

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
        songData = sd;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void InitSong()
    {
        // PADDLES
        PaddleManager paddleManager = FindObjectOfType<PaddleManager>();
        paddleManager.Activate();

        // SONG
        SongController song = FindObjectOfType<SongController>();
        song.LoadSong(this.songData);
        song.JumpToStart();

        // BALL DROPPER
        BallDropper ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        ballDropper.Activate();
        ballDropper.ballMapName = song.songName;

        song.Play();
    }

    private void CheckForNonMenuPlay()
    {
        // if we start the game in song scene
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            InitSong();
        }
    }
}
