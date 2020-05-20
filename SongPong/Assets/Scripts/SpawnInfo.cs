using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnInfo : MonoBehaviour
{
    private Vector2 screenBounds;

    private Axis gameAxis;
    private AxisManager axisManager;

    //_______OPTIONS_____________
    [Range(0.0f,0.5f)]
    public float paddingXAxis = 0.1f;
    private float defaultPaddingXAxis = 0.1f;

    [Range(0.0f, 0.5f)]
    public float paddingYAxis = 0.1f;
    private float defaultPaddingYAxis = 0.1f;

    [Range(1,32)]
    public int NUM_COL = 16; // number of ball columns
    private float defaultNumCol = 16;

    //_______SPAWN_______________
    [Tooltip("Determines what height the ball spawns are from the top of the screen")]
    public float ballHeightMod = 0;
    private float ballDropHeight; // height (x or y) that balls spawn from

    //_______DEBUG_______________
    public bool showColumns = true; // if true, show columns

    // COLUMNS
    private float[] ballCols; // x- or y-positions of each column in world coordinates

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INIT
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    // Start is called before the first frame update
    private void Awake() 
    {
        axisManager = FindObjectOfType<AxisManager>();
        gameAxis = axisManager.GameAxis;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        ballDropHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height + ballHeightMod)).y;
        calcColumns();
    }

    private void Update() 
    {
        UpdateAxis();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * COLUMNS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void UpdateAxis()
    {
        // update padding x axis
        if(paddingXAxis != defaultPaddingXAxis)
        {
            calcColumns();
            defaultPaddingXAxis = paddingXAxis;
        }
        // update padding y axis
        if(paddingYAxis != defaultPaddingYAxis)
        {
            calcColumns();
            defaultPaddingYAxis = paddingYAxis;
        }
        // update columns
        if(defaultNumCol != NUM_COL)
        {
            calcColumns();
            defaultNumCol = NUM_COL;
        }
    }

    private void calcColumns()
    {
        ballCols = new float[NUM_COL+1]; // need n+1 lines to make n columns

        if(gameAxis == Axis.y)
        {
            int width = Screen.width;
            int screenPadding = (int)(width * paddingYAxis);
		    int effScreenW = width - 2 * screenPadding; // the screen width minus the padding

		    int colStep = effScreenW / NUM_COL; // amount of x to move per column

            for(int i = 0; i < NUM_COL+1; i++)
            {
			    ballCols[i] = Camera.main.ScreenToWorldPoint(new Vector3(colStep * i + screenPadding,0,0)).x;
		    }
        }
        else if(gameAxis == Axis.x)
        {
            int height = Screen.height;
            int screenPadding = (int)(height * paddingXAxis);
		    int effScreenH = height - 2 * screenPadding; // the screen width minus the padding

		    int colStep = effScreenH / NUM_COL; // amount of y to move per column
 
            for(int i = 0; i < NUM_COL+1; i++)
            {
			    ballCols[i] = Camera.main.ScreenToWorldPoint(new Vector3(0,colStep * i + screenPadding,0)).y;
		    }
        }

    }

    private void OnDrawGizmos()
    {
        if(ballCols != null && showColumns)
        {
            if(gameAxis == Axis.y)
            {
                foreach(float x in ballCols)
                {
                    // Draw Columns
                    Gizmos.color = Color.red;
                    Vector3 top = new Vector3(x, screenBounds.y, 0);
                    Vector3 bot = new Vector3(x, -screenBounds.y, 0);
                    Gizmos.DrawLine(top, bot);
                    
                    // Draw Spawn Locations
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(new Vector3(x, ballDropHeight, 0), 0.2f);
                }
            }
            else if(gameAxis == Axis.x)
            {
                foreach(float y in ballCols)
                {
                    // Draw Columns
                    Gizmos.color = Color.red;
                    Vector3 left = new Vector3(-screenBounds.x, y, 0);
                    Vector3 right = new Vector3(screenBounds.x, y, 0);
                    Gizmos.DrawLine(left, right);

                    // Draw SpawnLocations
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(new Vector3(0, y, 0), 0.2f);
                }
            }
        }
    }

    public int GetNearestColumn(Vector2 screenPos, bool debug = false)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        int nearestColumn = 0;
        float minDist = float.MaxValue;

        float compare;
        if(gameAxis == Axis.y){compare = worldPos.x;}
        else if(gameAxis == Axis.x){compare = worldPos.y;}
        else{return 0;}

        for(int f = 0; f < ballCols.Length; f++)
        {
            float delta = Mathf.Abs(ballCols[f] - compare);
            if(delta < minDist)
            {
                minDist = delta;
                nearestColumn = f;
            }
        }
        if(debug){print("NEAREST COLUMN: " + nearestColumn);}

        return nearestColumn;
    }

    public Vector2 GetSpawnLocation(int spawnNum)
    {
        if(gameAxis == Axis.y)
        {
            return new Vector2(ballCols[spawnNum], ballDropHeight);
        }
        else if(gameAxis == Axis.x)
        {
            return new Vector2(0, ballCols[spawnNum]);
        }

        return new Vector2(999f,999f);
    }
}
