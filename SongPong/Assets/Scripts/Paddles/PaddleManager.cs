using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{
    //__________REFERENCES____________
    private Game game;
    private Track track;

    //__________AXIS__________________
    [Header("AXIS")]    
    [Range(0,45)]
    [Tooltip("The percent(%) away from the edge of screen the paddle's axis is.")]
    public int padding; // percent

    //__________PADDLE________________
    [Header("PADDLE")]
    public float speed = 12.0f;

    private float yValue; // the y-height above the track the paddles will appear
    private float bound; // bounds
    private float distFromTrack; // the z-position of the paddle

    //__________PADDLE________________
    [Header("PADDLES")]
    public GameObject paddle1;
    public GameObject paddle2;

    //__________LEDS__________________
    [Header("LEDs")]
    public float beatsPerFlash;

    public void Activate()
    {
        // REFERENCES
        game = FindObjectOfType<Game>();
        track = FindObjectOfType<Track>();

        distFromTrack = track.GetBottom() + 2;
        yValue = track.gameYValue;

        // PADDLE 2
        // moves along x axis (horizontal)
        PaddleMover pm2 = paddle2.GetComponent<PaddleMover>();
        pm2.Initialize(this);
        pm2.speed = speed;
        pm2.paddleAxis = new Vector3(1,0,0);
        pm2.distFromTrack = distFromTrack;
        pm2.yValue = yValue;
        bound = track.GetRight() - track.padding;
        pm2.bounds = new Vector2(-bound + pm2.radius, bound - pm2.radius);
        
        // PADDLE 1
        // Disabled
        paddle1.SetActive(false);
    }
}
