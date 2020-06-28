using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceHeight : BallOption
{
    public BounceHeight(float bh)
    {
        name = "bounce_height";
        value = bh;
    }

    public void SetValue(float v)
    {
        value = v;
    }
}
