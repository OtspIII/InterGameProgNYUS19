using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyThing : WorldThing
{
    public override IEnumerator GetBumped(WorldThing bumper)
    {
        //If you touch this, you get a key! And the key goes on top of you
        if (bumper.Type == Types.Player)
        {
            TileThing loc = Location;
            ((PlayerThing) bumper).HasKey = true;
            LeaveTile(Location);
            transform.SetParent(bumper.transform);
            transform.localPosition = new Vector3(0.25f,0.25f,-0.1f);
            transform.localScale = new Vector3(0.5f,0.5f,1);
            yield return God.GSM.StartCoroutine(bumper.Walk(loc));
        }
    }
}
