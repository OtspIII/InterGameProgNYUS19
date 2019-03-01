using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorThing : WorldThing
{
    public override bool GetBumped(WorldThing bumper)
    {
        //If you enter the door and you have the key, reload the scene
        if (bumper.Type == Types.Player && ((PlayerThing)bumper).HasKey)
        {
            SceneManager.LoadScene("Game");
            return true;
        }
        return base.GetBumped(bumper);
    }
}
