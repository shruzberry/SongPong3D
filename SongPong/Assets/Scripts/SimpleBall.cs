using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBall : Ball
{

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * INITIALIZE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * UPDATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void UpdateBall()
    {
        if(transform.position.y < -screenBounds.y){
           handleMiss();
        }
    }

    public void FixedUpdateBall()
    {
        velocityY += gravity * Time.fixedDeltaTime;

        Vector2 newPos = new Vector2(velocityX * Time.deltaTime, velocityY * Time.deltaTime);
        rb.MovePosition((Vector2)transform.position + newPos);
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * STATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public void handleDrop()
    {
        spawnTime = Time.time;
    }

    protected override void handleMiss()
    {
        print("Missed Ball");
        Destroy(this.gameObject);
    }

    protected override void handleCatch()
    {
        print("Collide");
        isFinished = true;
    }

 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * GETTERS
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/
    public float getSize(){return size;}
    public float getHitTime(){
       return notes[0].getHitTime();
    }
    public int getSpawnColumn(){return notes[0].getColumn();}
    public bool checkIsFinished(){return isFinished;}
    public bool checkMissed(){return missed;}

}
