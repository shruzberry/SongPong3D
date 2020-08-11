using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    //_______REFERENCES__________________
    private Game game;

    //_______COMPONENTS__________________
    private MeshRenderer mesh_renderer;
    public GameObject columnPrefab;

    //_______ATTRIBUTES__________________
    [Range(1,16)]
    public int NUM_COL = 16; // number of ball columns
    public float padding;
    public float paddingFallAxis;

    //_______SIZING______________________
    private Vector3 position;
    private Vector3 size;
    private float width;
    private float height;
    private float bottom;
    private float top;
    private float left;
    private float right;
    private float middle;

    //_______SPAWN INFO__________________
    public float gameYValue;
    [Tooltip("Determines what height the ball spawns are from the top of the screen")]
    public float[] ballCols; // x- or z-positions of each column in world coordinates
    public float ballHeightMod = 0;
    public float columnWidth;

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void Initialize(Game game)
    {
        this.game = game;

        mesh_renderer = GetComponentInChildren<MeshRenderer>();
        position = transform.position;
        size = mesh_renderer.bounds.size;
        width = size.x;
        height = size.z;
        top = transform.position.z + (height / 2);
        bottom = transform.position.z - (height / 2);
        left = transform.position.x - (width / 2);
        right = transform.position.x + (width / 2);
        middle = transform.position.z;

        CalcColumns();
        SpawnColumns();
    }

/*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * COLUMNS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    private void CalcColumns()
    {
        ballCols = new float[NUM_COL+1]; // need n+1 lines to make n column

        float trackLeft = transform.position.x - (GetWidth() / 2);
        float effScreenW = GetWidth() - (2 * padding); // the screen width minus the padding

        // STEP for each column
        columnWidth = effScreenW / NUM_COL; // amount of x to move per column

        for(int i = 0; i < NUM_COL+1; i++)
        {
            ballCols[i] = trackLeft + (columnWidth * i + padding);
        }
    }

    private void SpawnColumns()
    {
        SpawnColumn(ballCols[0] - (columnWidth));
        foreach(float position in ballCols)
        {
            SpawnColumn(position);
        }
    }

    private void SpawnColumn(float position)
    {
        Column col = Instantiate(columnPrefab).GetComponent<Column>();
        col.Initialize(position + (columnWidth / 2));
        col.transform.parent = this.transform;
    }

    public int GetNearestColumn(Vector2 screenPos, bool debug = false)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        int nearestColumn = 0;
        float minDist = float.MaxValue;

        float compare;
        if(game.gameType == GameType.OnePlayer){compare = worldPos.x;}
        else if(game.gameType == GameType.TwoPlayer){compare = worldPos.z;}
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

    private float ConvertToUnits(Camera cam, float p)
    {
        float ortho = cam.orthographicSize;
        float pixelH = cam.pixelHeight;

        return (p * ortho * 2) / pixelH;
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public Vector2 GetBoundsFallAxis()
    {
        float bottom = position.z - (height/2) + paddingFallAxis;
        float top = position.z + (height/2) - paddingFallAxis;
        return new Vector2(bottom, top);
    }
    
    public float GetWidth()
    {
        return width;
    }

    public float GetHeight()
    {
        return height;
    }

    public float GetBottom()
    {
        return bottom;
    }

    public float GetTop()
    {
        return top;
    }

    public float GetLeft()
    {
        return left;
    }

    public float GetRight()
    {
        return right;
    }

    public float GetMiddle()
    {
        return middle;
    }

    public Vector3 GetSize()
    {
        return size;
    }

}
