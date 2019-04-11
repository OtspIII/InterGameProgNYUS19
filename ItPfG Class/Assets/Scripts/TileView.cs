using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public TileModel Model;

    public void Setup(TileModel m)
    {
        Model = m;
        m.View = this;
        transform.position = new Vector3(m.X,m.Y,0);
//        God.GSM.AllTiles.Add(this);
//        if (!God.GSM.Tiles.ContainsKey(m.X))
//            God.GSM.Tiles.Add(m.X,new Dictionary<int, TileView>());
//        God.GSM.Tiles[m.X].Add(m.Y,this);
    }
    
    
}
