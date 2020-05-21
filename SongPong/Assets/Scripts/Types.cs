using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Types
{
    public enum Direction
    {
        positive, negative
    }

    public enum BallTypes
    {
        simple, bounce
    }
    
    // which paddle is this (P1 is left-side in x-axis mode)
    public enum Paddles
    {
        P1,P2
    }
}
