using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private double gameTime;
    // Start is called before the first frame update
    void Start()
    {
        gameTime = Time.time;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameTime += Time.deltaTime;
    }

    public double getGameTime(){
        return gameTime;
    }
}
