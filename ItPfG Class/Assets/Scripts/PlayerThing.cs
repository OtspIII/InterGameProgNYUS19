using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThing : WorldThing
{
    public bool HasKey = false;

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //If I hit an arrow key, move in that direction
        if (IM.Pressed(Inputs.Left))
        {
            Move(-1,0);
        }
        else if (IM.Pressed(Inputs.Right))
        {
            Move(1,0);
        }
        else if (IM.Pressed(Inputs.Up))
        {
            Move(0,1);
        }
        else if (IM.Pressed(Inputs.Down))
        {
            Move(0,-1);
        }
    }
}
