using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreThing : WorldThing
{
    public override bool GetBumped(WorldThing bumper)
    {
        //If you enter the door, you load the new scene
        if (bumper.Type == Types.Player)
        {
            Despawn();
            //GSM.ChangeScore(1);
            return false;
        }

        return true;
    }
}
