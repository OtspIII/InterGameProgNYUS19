using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreThing : WorldThing
{
    public override IEnumerator GetBumped(WorldThing bumper)
    {
        //If you stand on the score thing you get a point
        if (bumper.Type == Types.Player)
        {
            TileThing loc = Location;
            Despawn();
            God.GSM.ChangeScore(1);
            yield return StartCoroutine(bumper.Walk(loc));//This is going to throw an error eventually!
        }
    }
}
