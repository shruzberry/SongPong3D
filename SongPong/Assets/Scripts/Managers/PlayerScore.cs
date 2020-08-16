using System.Collections.Generic;
using UnityEngine;
using Types;
using TMPro;

/**
 * When a ball is caught, this class gives the player points
 * Each ball has a different method of scoring.
 */
[ExecuteInEditMode]
public class PlayerScore : MonoBehaviour
{
    //_____ SETTINGS ____________________
    //_____ REFERENCES __________________
    private BallDropper ballDropper;
    private SongController songController;
    public Paddles player;

    //_____ COMPONENTS __________________
    private TextMeshProUGUI scoreText;
    private List<Ball> activeBalls;

    //_____ ATTRIBUTES __________________
    public int score = 0;

    //_____ STATE  ______________________
    //_____ OTHER _______________________

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* INITIALIZE
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnEnable()
    {
        ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        songController = GameObject.Find("SongController").GetComponent<SongController>();
        ballDropper.onBallSpawned += AddBallListener;
        songController.onSongEnd += FlashEndScore;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    void Awake()
    {
    }

    private void AddBallListener(Ball ball)
    {
        ball.onBallCaught += Score; //can i have parameters?
    }
    
    void OnDisable()
    {
        ballDropper.onBallSpawned -= AddBallListener;
        songController.onSongEnd -= FlashEndScore;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* UPDATE
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void Update()
    {
        scoreText.text = "" + score;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* DISPLAY
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void FlashEndScore()
    {
        Transform t = GetComponent<Transform>();

        Vector3 position = t.position;
        t.position = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);

        scoreText.alignment = TextAlignmentOptions.Center;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* CALCULATIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Score(Ball ball)
    {
        score += 1;
    }
}
