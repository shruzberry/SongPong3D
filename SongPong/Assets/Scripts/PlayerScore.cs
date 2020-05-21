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

    private AxisManager axisManager;
    private Text score;

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* PUBLIC FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    void OnEnable()
    {
        axisManager = GameObject.Find("Game").GetComponent<AxisManager>();
        score = GetComponentInChildren<Text>();
    }

    void Update()
    {
        checkPlayerSelect();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
* RUNTIME FUNCTIONS
*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    void checkPlayerSelect()
    {
        switch (player)
        {
            case Paddles.P1:
                score.alignment = TextAnchor.LowerLeft;
                break;
            case Paddles.P2:
                score.alignment = TextAnchor.LowerRight;
                break;
        }
    }
}
