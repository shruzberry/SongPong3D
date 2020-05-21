using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoSingleton<LevelChanger>
{
    private Animator animator;
    private int levelToLoad;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void Update() {
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
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
