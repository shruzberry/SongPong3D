using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisManager : MonoBehaviour
{
    //_______AXIS________________
    private Axis defaultAxis = Axis.y; // stores the axis set at the start so if the axis changes, it triggers recalculation
    public Axis gameAxis = Axis.y;
}
