/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
________ DEFENITION ________
Class Name: PlayerScore.cs
Purpose: Subscribes to ball events and modifies a UI based on public parameters

________ USAGE ________
* Attach to gameobject
* Select player to subscribe to

________ ATTRIBUTES ________
+ Axis axis

________ FUNCTIONS ________


+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* DEPENDENCIES
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Types;

[ExecuteInEditMode]
public class PlayerScore : MonoBehaviour
{
/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* MEMBERS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public Paddles player;
    public int score = 0;

    private AxisManager axisManager;
    private BallDropper ballDropper;
    private Text scoreText;
    private List<Ball> activeBalls;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnEnable()
    {
        axisManager = FindObjectOfType<AxisManager>();
        ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
        ballDropper.onBallSpawned += AddBallListener;
        scoreText = GetComponentInChildren<Text>();
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
                scoreText.alignment = TextAnchor.LowerLeft;
                break;
            case Paddles.P2:
                scoreText.alignment = TextAnchor.LowerRight;
                break;
        }
    }

    private void AddBallListener(Ball ball)
    {
        ball.onBallCaught += Score; //can i have parameters?
    }

    private void Score(Ball ball, Paddle paddle)
    {
        switch(player)
        {/*
            case Paddles.P1:
                if(ball.direction == Direction.negative)
                    score += 1;
                break;
            case Paddles.P2:
                if(ball.direction == Direction.positive)
                    score += 1;
                break;
        */}
    }
}
