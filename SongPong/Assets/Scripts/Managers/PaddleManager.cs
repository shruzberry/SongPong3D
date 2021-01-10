using UnityEngine;

/**
 * The Manager for paddles, spawns, sets attributes, etc.
 */
public class PaddleManager : MonoBehaviour
{
    //_____ SETTINGS ____________________
    [Header("LEDs")]
    public float beatsPerFlash;
    [Header("Paddle")]
    public float speed = 12.0f;
    public int numColumnsPaddleWidth = 3;

    //_____ REFERENCES __________________
    private Game game;
    private Track track;

    //_____ COMPONENTS __________________
    public GameObject paddle1;
    public GameObject paddle2;

    //_____ ATTRIBUTES __________________
    public Vector3 paddleAxis;
    private float bound; // bounds
    public Vector2 bounds;
    public float yValue; // the y-height above the track the paddles will appear
    public float distFromTrack; // the z-position of the paddle

    //_____ STATE  ______________________
    //_____ OTHER _______________________

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Initialize(Game game, Track track)
    {
        // REFERENCES
        this.game = game;
        this.track = track;
        
        float trackWidth = track.columnWidth;
        float paddleSize = trackWidth * numColumnsPaddleWidth;

        Paddle pm2 = paddle2.GetComponent<Paddle>();

        pm2.transform.localScale = new Vector3(paddleSize, paddleSize, paddleSize);

        // VARIABLES
        paddleAxis = new Vector3(1,0,0);
        bound = track.GetRight() - track.padding;
        bounds = new Vector2(-bound + pm2.radius, bound - pm2.radius);
        distFromTrack = track.GetBottom() + 2;
        yValue = track.gameYValue;

        // PADDLE 2
        // moves along x axis (horizontal)
        pm2.Initialize(this);
        
        // PADDLE 1
        // Disabled
        paddle1.SetActive(false);
    }
}
