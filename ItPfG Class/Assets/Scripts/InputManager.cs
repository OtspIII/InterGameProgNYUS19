using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IM
{

    public static bool CanAct = true;
    
    public static bool Pressed(Inputs i)
    {
        if (!IM.CanAct)
            return false;
        
        switch (i)
        {
            case Inputs.Up:
                return Input.GetKeyDown(KeyCode.UpArrow);
            case Inputs.Left:
                return Input.GetKeyDown(KeyCode.LeftArrow);
            case Inputs.Right:
                return Input.GetKeyDown(KeyCode.RightArrow);
            case Inputs.Down:
                return Input.GetKeyDown(KeyCode.DownArrow);
        }

        return false;
    }
}

public enum Inputs
{
    None=0,
    Up=1,
    Right=2,
    Down=3,
    Left=4
}