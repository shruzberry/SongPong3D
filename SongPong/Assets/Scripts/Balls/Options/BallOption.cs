using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BallOption
{
    public string opt_name;
    public float value;

    public BallOption(float value, string name)
    {
        this.value = value;
        this.opt_name = name;
    }
}

