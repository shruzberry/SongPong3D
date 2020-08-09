using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLabel
{
    public int numNotes;
    public float y;
    public float yMax;
    public float line_h = 20;
    public float padding = 35;
    public float height;

    public BallLabel(BallData data, float y)
    {
        this.y = y;
        height = line_h + (numNotes * line_h);
        height += padding + (padding * numNotes);
        yMax = y + height;
    }

    public void RenderLabel()
    {
    }
}
