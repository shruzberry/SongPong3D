using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public enum Axis {x,y}; //marking for deletion. has been migrated to types.cs

public class AxisManager : MonoBehaviour
{
    //_______EVENT_______________
    public delegate void OnGameAxisChange();
    public event OnGameAxisChange onGameAxisChange;

    //_______AXIS________________
    public Axis gameAxis = Axis.y;
    public Axis GameAxis
    {
        get{return gameAxis;}
        set
        {
            if(gameAxis == value) return;
            gameAxis = value;
            if(onGameAxisChange != null) onGameAxisChange();
        }
    }

    private void OnValidate() 
    {
        Debug.Log("TesT");
    }
}
