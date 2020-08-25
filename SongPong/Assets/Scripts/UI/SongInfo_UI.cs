using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongInfo_UI : MonoBehaviour
{
    public TextMeshProUGUI songName;
    private Game game;

    void Start()
    {
        game = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
