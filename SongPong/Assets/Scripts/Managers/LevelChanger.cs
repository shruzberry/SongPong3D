using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoSingleton<LevelChanger>
{
    public Animator animator;
    private int levelToLoad;

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
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToLevel(int levelIndex)
    {
        print("made it to fade trigger");
        levelToLoad = levelIndex;
        if (animator.gameObject.activeSelf)
        {
            print("doing it");
            animator.SetTrigger("FadeOut");
        }
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
