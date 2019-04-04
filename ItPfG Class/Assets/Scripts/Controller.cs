using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    void Update()
    {
        if (IM.Pressed(Inputs.Left))
        {
            MsgAll(new EventMsg(Inputs.Left));
        }
        else if (IM.Pressed(Inputs.Right))
        {
            MsgAll(new EventMsg(Inputs.Right));
        }
        else if (IM.Pressed(Inputs.Up))
        {
            MsgAll(new EventMsg(Inputs.Up));
        }
        else if (IM.Pressed(Inputs.Down))
        {
            MsgAll(new EventMsg(Inputs.Down));
        }
    }

    public void MsgAll(EventMsg msg)
    {
        foreach(ActorModel am in ModelManager.AllThings.ToArray())
            am.TakeMsg(msg);
        God.GSM.UpdateText();
    }
}
