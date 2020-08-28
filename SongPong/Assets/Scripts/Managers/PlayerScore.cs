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
    private Game game;
    private BallDropper ballDropper;
    private SongController songController;
    public Paddles player;

    //_____ COMPONENTS __________________
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
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
        game = FindObjectOfType<Game>();
        game.onGameRestart += ResetScore;
        game.onGameEnd += SetFinalScore;

        ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        songController = GameObject.Find("SongController").GetComponent<SongController>();
        ballDropper.onBallSpawned += AddBallListener;
        UpdateScoreText();
    }

    private void AddBallListener(Ball ball)
    {
        ball.onBallCaught += Score; //can i have parameters?
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    
    void OnDisable()
    {
        ballDropper.onBallSpawned -= AddBallListener;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* UPDATE
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void UpdateScoreText()
    {
        scoreText.text = "" + score.ToString("0000000");
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* DISPLAY
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void SetFinalScore()
    {
        finalScoreText.text = score.ToString("0000000");
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* CALCULATIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void Score(Ball ball)
    {
        score += 50;
        UpdateScoreText();
    }
}
