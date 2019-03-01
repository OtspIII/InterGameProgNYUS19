using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyThing : WorldThing
{
    public override bool GetBumped(WorldThing bumper)
    {
        //If you enter the door and you have the key, reload the scene
        if (bumper.Type == Types.Player)
        {
            ((PlayerThing) bumper).HasKey = true;
            LeaveTile(Location);
            transform.SetParent(bumper.transform);
            transform.localPosition = new Vector3(0,0,-0.1f); 
            return true;
        }

        return true;
    }
}
