﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileThing : MonoBehaviour
{
    //What world things are in me?
    public List<WorldThing> Contents;
    
    //What are my map coordinates?
    public int X;
    public int Y;

    public void Setup(int x, int y)
    {
        X = x;
        Y = y;
        transform.position = new Vector3(X,Y,0);
        //Register yourself with the GSM when you spawn
        God.GSM.AllTiles.Add(this);
        if (!God.GSM.Tiles.ContainsKey(X))
            God.GSM.Tiles.Add(X,new Dictionary<int, TileThing>());
        God.GSM.Tiles[X].Add(Y,this);
    }

    //If I get told an x and y value I'll see what tile is that many units away from me
    public TileThing Neighbor(int x, int y)
    {
        return God.GSM.GetTile(X + x,Y + y);
    }
    
    
}
