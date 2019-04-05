using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class God
{
    public static GSM GSM;
    public static LibraryManager Library;
    public static Controller C;
    
    public static float Ease(float t, bool inout)
    {
        if (inout)
            return t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
        return (--t) * t * t + 1;
    }
}