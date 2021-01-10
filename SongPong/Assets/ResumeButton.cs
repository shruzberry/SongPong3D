using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ResumeGame);
    }

    private void ResumeGame()
    {
        FindObjectOfType<Game>().ResumeGame();
    }
}
