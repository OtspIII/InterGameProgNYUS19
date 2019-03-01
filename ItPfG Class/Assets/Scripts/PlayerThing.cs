using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThing : WorldThing
{
    public bool HasKey = false;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //If I hit an arrow key, move in that direction
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1,0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1,0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(0,1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0,-1);
        }
    }
}
