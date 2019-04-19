using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorThing : WorldThing
{
    public override void GetBumped(WorldThing bumper)
    {
        //If you enter the door and you have the key, reload the scene
        if (bumper.Type == Types.Player && (!GameSettings.NeedKey || ((PlayerThing)bumper).HasKey))
        {
            TileThing loc = Location;
            //Normally you can't walk into a tile with a thing in it, but since this is the win animation
                //I make an override
            Location.Contents = null;
            bumper.SetLocation(loc);
            
        }
    }
    
    public override bool CanEnter()
    {
        return true;
    }
}
