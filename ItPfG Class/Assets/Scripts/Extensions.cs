using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{

    public static void Rotate2D(this Transform t,float rot)
    {
        t.localRotation = Quaternion.Euler(new Vector3(0,0,rot));
    } 
    
}
