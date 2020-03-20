using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{

    // COLUMNS
    private int[] ballCols;
    private bool showColumns = true;
    private static int NUM_COL = 16;
    public GameObject simpleBall;

    void Awake() {
    }
    void Start()
    {
        print("BD START");
        calcColumns();
        Instantiate(simpleBall, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        drawColumns();
    }

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
