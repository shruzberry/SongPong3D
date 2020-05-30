using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoSingleton<LevelChanger>
{
    public Animator animator;
    public SongData song;
    
    private int levelToLoad;
    private bool songLoaded = false;

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
        print("made to setSong: " + sd.name);
        song = sd;
    }

    private void SongInit()
    {
        songLoaded = true;
        this.gameObject.SetActive(false);
        SongController songController = GameObject.Find("SongController").GetComponent<SongController>();
        songController.LoadSong(song);
    }
}
