using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    private int id;
    private float hitTime;
    private int column;

    public Note(int id, float hitTime, int column){
        this.id = id;
        this.hitTime = hitTime;
        this.column = column;
    }

    public int getId(){return id;}
    public float getHitTime(){return hitTime;}
    public int getColumn(){return column;}
}
