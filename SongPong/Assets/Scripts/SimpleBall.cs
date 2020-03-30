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


 /*+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
 * STATE
 *+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=*/

    public override void handleDrop()
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

}
