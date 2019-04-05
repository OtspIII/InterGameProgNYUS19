using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction
{
    public virtual IEnumerator Run()
    {
        yield return null;
    }
}

