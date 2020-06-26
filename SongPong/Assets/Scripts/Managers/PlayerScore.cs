/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=

________ DEFENITION ________
Class Name: PlayerScore.cs
Purpose: Subscribes to ball events and modifies a UI based on public parameters

________ USAGE ________
* Attach to gameobject
* Select player to subscribe to

________ PUBLIC ________
+ int Score: The score of the player
+ (enum)Paddles player: The paddle that the Player score is listening to

+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using TMPro;

[ExecuteInEditMode]
public class PlayerScore : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public Paddles player;
    public int score = 0;

    private BallDropper ballDropper;
    private TextMeshProUGUI scoreText;
    private List<Ball> activeBalls;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnEnable()
    {
        ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        ballDropper.onBallSpawned += AddBallListener;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        checkPlayerSelect();
        scoreText.text = "" + score;
    }
    
    void OnDisable()
    {
        ballDropper.onBallSpawned -= AddBallListener;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PRIVATE FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    private void checkPlayerSelect()
    {
        switch (player)
        {
            case Paddles.P1:
                //scoreText.alignment = TextAnchor.LowerLeft;
                break;
            case Paddles.P2:
                //scoreText.alignment = TextAnchor.LowerRight;
                break;
        }
    }

    private void AddBallListener(Ball ball)
    {
        ball.onBallCaught += Score; //can i have parameters?
    }

    private void Score(Ball ball)
    {
        score += 1;
    }
}
