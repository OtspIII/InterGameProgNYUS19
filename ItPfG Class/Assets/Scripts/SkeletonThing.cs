using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonThing : WorldThing
{
    public override bool GetBumped(WorldThing bumper)
    {
        //If you enter the door, you load the new scene
        if (bumper.Type == Types.Player)
        {
            bumper.Despawn();
            //GSM.SetText("You Died");
            return false;
        }
        return base.GetBumped(bumper);
    }
}
