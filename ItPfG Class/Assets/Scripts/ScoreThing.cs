using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreThing : WorldThing
{
    public override void GetBumped(WorldThing bumper)
    {
        //If you stand on the score thing you get a point
        if (bumper.Type == Types.Player)
        {
            TileThing loc = Location;
            Despawn();
            God.GSM.ChangeScore(1);
            bumper.SetLocation(loc);
        }
    }

    public override bool CanEnter()
    {
        return true;
    }
}
