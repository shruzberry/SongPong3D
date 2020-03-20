using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBall : MonoBehaviour
{
    // ATTRIBUTES
    private int ballNum;

    // POSITION
    private double startPositionX;

    // BOUNCE
    private int numBouncesLeft;
    private double spawnTime;

    void Awake(){

    }
    void Start()
    {
        print("SIMPLE BALL START");
    }

    void Update()
    {
    }
}
