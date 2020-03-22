using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    // REFERENCES
    public GameTimer gt;
    public Song song;
    public GameObject simpleBall;

    // SPAWN
    private int ballCounter = 0; // how many balls are in this song
    private int dropIndex = 0; // keeps track of which ball to drop next

    // COLUMNS
    private int[] ballCols; // x-positions of each column in screen coordinates
    private bool showColumns = true; // if true, show columns
    private static int NUM_COL = 16; // number of ball columns

    void Awake() {
        gt = gameObject.GetComponent<GameTimer>();
        song = gameObject.GetComponent<Song>();
    }
    void Start()
    {
        calcColumns();
    }

    // Update is called once per frame
    private bool first = true;
    void Update()
    {
        drawColumns();

        // Test spawning ball
        if(first){
            spawnSimpleBall(1);
            first = false;
        }

        checkDrop();
    }

private bool test = true;
    public void checkDrop() {
		double currentTime = gt.getGameTime();
        if(test){
            song.readNote(0);
            test = false;
        }

        // get the current line of the note map

        // check if spawn time < current time

        // spawn ball based on type

	}

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * SPAWN
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void spawnSimpleBall(int column){

        Vector3 spawnPos = new Vector3(ballCols[column-1], 100, 1);

        Instantiate(simpleBall, Camera.main.ScreenToWorldPoint(spawnPos), Quaternion.identity);
        ballCounter++;
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 *
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/


/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * COLUMNS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void calcColumns(){
        ballCols = new int[NUM_COL+1]; // need n+1 lines to make n columns
        int width = Screen.width;
        int screenPadding = (int)(width * 0.1); // padding is 10% of screen width on each side (20% total)
		int effScreenW = width - 2 * screenPadding; // the screen width minus the padding
	    //print("ScreenW: " + width + "screenPadding: " + screenPadding + " EffScreenWidth: " + effScreenW);

		int colStep = effScreenW / NUM_COL; // amount of x to move per column

		for(int i = 0; i < NUM_COL+1; i++) {
			ballCols[i] = colStep * i + screenPadding;
			//print("Column " + i + " is located at x = " + ballCols[i]);
		}

    }

    void drawColumns(){
        int height = Screen.height;
        int z = 1;
        if (ballCols != null && showColumns)
        {
            // Draw a white line over each column
            for(int i = 0; i < ballCols.Length; i++){
                Vector3 top = Camera.main.ScreenToWorldPoint(new Vector3(ballCols[i], height, z));
                Vector3 bot = Camera.main.ScreenToWorldPoint(new Vector3(ballCols[i],0, z));
                Debug.DrawLine(top,bot, Color.white);
            }
        }
    }
}
