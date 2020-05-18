using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public int id;
    public float hitTime; // hit 
    public int column; // which spawn to start at

    public Note(int id, float hitTime, int column)
    {
        this.id = id;
        this.hitTime = hitTime;
        this.column = column;
    }

    public Note(Note other)
    {
        this.id = other.id;
        this.hitTime = other.hitTime;
        this.column = other.column;
    }

    public int getId(){return id;}
    public float getHitTime(){return hitTime;}
    public int getColumn(){return column;}

}
