using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreThing : WorldThing
{
    public override bool GetBumped(WorldThing bumper)
    {
        //If you stand on the score thing you get a point
        if (bumper.Type == Types.Player)
        {
            Despawn();
            //GSM.ChangeScore(1);
            return false;
        }
        return false;
    }
}
