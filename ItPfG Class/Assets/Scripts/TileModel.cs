using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileModel
{
    public Guid ID;
    [NonSerialized]public TileView View;
    //What world things are in me?
    public Guid Contents;
    
    //What are my map coordinates?
    public int X;
    public int Y;
    
    public TileModel(int x, int y)
    {
        ID = Guid.NewGuid();
        X = x;
        Y = y;
        //Register yourself with the GSM when you spawn
        ModelManager.AllTiles.Add(ID,this);
        if (!ModelManager.Tiles.ContainsKey(X))
            ModelManager.Tiles.Add(X,new Dictionary<int, TileModel>());
        ModelManager.Tiles[X].Add(Y,this);
    }

    public ActorModel GetContents()
    {
        return ModelManager.GetActor(Contents);
    }

    public void OnLoad()
    {
        if (!ModelManager.Tiles.ContainsKey(X))
            ModelManager.Tiles.Add(X,new Dictionary<int, TileModel>());
        ModelManager.Tiles[X].Add(Y,this);
    }
    
    //If I get told an x and y value I'll see what tile is that many units away from me
    public TileModel Neighbor(int x, int y)
    {
        return ModelManager.GetTile(X + x,Y + y);
    }
}
