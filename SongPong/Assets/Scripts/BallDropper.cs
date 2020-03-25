using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    // REFERENCES
    public Song song;
    private Paddle paddle;
    public GameObject simpleBall;

    // CONSTANTS
    public float dropSpeed = -1.0f;
    public float gravity = -3;

    // SPAWN
    private int ballCounter = 0; // how many balls are in this song
    private int dropIndex = 0; // keeps track of which ball to drop next
    private Vector2 screenBounds;

    public float ballDropY = 200;
    private float ballRadius;
    private float paddleY;

    // TIME
    private float dropTime;


    // COLUMNS
    private float[] ballCols; // x-positions of each column in world coordinates
    private bool showColumns = true; // if true, show columns
    private static int NUM_COL = 16; // number of ball columns

    void Awake() {
        paddle = GameObject.Find("Paddle").GetComponent<Paddle>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        spawnTestBall();
    }
    void Start()
    {
        dropTime = calcDropTime();

        calcColumns();
    }

    // Update is called once per frame
    private bool first = true;
    void Update()
    {
        drawColumns();

        // Test spawning ball
        /*
        if(first){
            spawnSimpleBall(1);
            first = false;
        }
        */
        checkDrop();
    }
    public void checkDrop() 
    {
        float nextHitTime = song.getNextHitTime();

        if(!song.finishedNotes && nextHitTime < Time.time){
            spawnSimpleBall(5);
            if(song.hasNextNote()){
                song.nextNote();
            }
        }

	}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SPAWN
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void spawnSimpleBall(int column)
    {
        Vector3 spawnPos = new Vector3(ballCols[column-1], ballDropY, 0);
        spawnPos.z = 0;

        GameObject ball = Instantiate(simpleBall, spawnPos, Quaternion.identity);
        SimpleBall ballScript = ball.GetComponent<SimpleBall>();
        ball.transform.parent = transform; // set BallDropper gameobject to parent
        ballScript.setBallDropSpeed(dropSpeed);
        ballScript.setBallAcceleration(gravity);

        ballCounter++;
    }

    public void spawnTestBall()
    {
        Vector3 spawnPos = new Vector3((-screenBounds.x * 2), (-screenBounds.y * 2), 0);
        GameObject simp = Instantiate(simpleBall, spawnPos, Quaternion.identity);
        SimpleBall simpScript = simp.GetComponent<SimpleBall>();
        ballRadius = simpScript.getBallSize() / 2;
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * CALCULATIONS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private float calcDropTime()
    {
        // Get Paddle Info
        paddleY = paddle.getPaddleYAxis();
        float paddleHeightHalf = paddle.getPaddleHeight() / 2;
        
        // Ball Info
        ballDropY = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height + 200)).y;
        
        // Determine Delta Y
        float deltaY = paddleY - ballDropY + ballRadius + paddleHeightHalf;
        print("(Confirmed Working) DISTANCE TO FALL: " + deltaY + " world units");

        // Calculate delta T
        // using physics equation dy = v0t + 1/2at^2 solved for time in the form
        // t = (-v0 +- sqrt(v0^2 + 2ady)) / a
        float determinant = (Mathf.Pow(dropSpeed, 2) + (2 * gravity * deltaY));
        float time = (-dropSpeed - Mathf.Sqrt(determinant)) / gravity;

        print("Expected Ball Drop Time: " + time + " sec.");
        return time;
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * COLUMNS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void calcColumns(){
        ballCols = new float[NUM_COL+1]; // need n+1 lines to make n columns
        int width = Screen.width;
        int screenPadding = (int)(width * 0.1); // padding is 10% of screen width on each side (20% total)
		int effScreenW = width - 2 * screenPadding; // the screen width minus the padding
	    //print("ScreenW: " + width + "screenPadding: " + screenPadding + " EffScreenWidth: " + effScreenW);

		int colStep = effScreenW / NUM_COL; // amount of x to move per column

		for(int i = 0; i < NUM_COL+1; i++) {
			ballCols[i] = Camera.main.ScreenToWorldPoint(new Vector3(colStep * i + screenPadding,0,0)).x;
			//print("Column " + i + " is located at x = " + ballCols[i]);
		}

    }

    void drawColumns(){
        int z = 1;
        if (ballCols != null && showColumns)
        {
            // Draw a white line over each column
            for(int i = 0; i < ballCols.Length; i++){
                Vector3 top = new Vector3(ballCols[i], screenBounds.y, z);
                Vector3 bot = new Vector3(ballCols[i],-screenBounds.y, z);
                Debug.DrawLine(top,bot, Color.white,1,false);
            }
        }
    }
}
