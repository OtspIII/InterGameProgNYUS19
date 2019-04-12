using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Point{
    public int x;public int y;
  
    public Point (int X, int Y){x = X;y = Y;}
  
    public static Point operator +(Point c1, Point c2)
    {return new Point(c1.x + c2.x, c1.y + c2.y);}
  
    public static Point operator -(Point c1, Point c2)
    {return new Point(c1.x - c2.x, c1.y - c2.y);}

    public static bool operator ==(Point c1, Point c2)
    {
        if (c1 is null && c2 is null)
            return true;
        if (c1 is null || c2 is null)
            return false;
        return c1.x == c2.x && c1.y == c2.y;
    }
  
    public static bool operator !=(Point c1, Point c2)
    {return c1.x != c2.x || c1.y != c2.y;}
}

