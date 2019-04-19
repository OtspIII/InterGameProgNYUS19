using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class TileThing : MonoBehaviour
{
    //What world things are in me?
    public WorldThing Contents;
    
    //What are my map coordinates?
    public int X;
    public int Y;

    public SpriteRenderer SR;
    public TextMesh From;
    public TextMesh To;
    public TextMesh Value;

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
    
    public List<TileThing> Neighbors()
    {
        List<TileThing> r = new List<TileThing>();
        List<Point> dirs = new List<Point>(){new Point(-1,0),new Point(1,0),new Point(0,1),new Point(0,-1)};
        foreach (Point d in dirs)
        {
            TileThing t = Neighbor(d.X, d.Y);
            if (t != null)
                r.Add(t);
        }
        return r;
    }

    public bool CanEnter()
    {
        if (Contents == null)
            return true;
        return Contents.CanEnter();
    }

    private void OnMouseDown()
    {
        if (Contents != null)
        {
            if (Contents.Type == WorldThing.Types.Wall || Contents.Type == WorldThing.Types.Skeleton)
                Contents.LeaveTile(this);
            return;
        }
        if (Random.Range(0,100f) >= 1)
            God.Library.SpawnThing(WorldThing.Types.Wall, this);
        else
            God.Library.SpawnThing(WorldThing.Types.Skeleton, this);
    }

    public void SetDemoInfo(PathTile pt, bool open,bool realPath)
    {
        From.text = pt.FromStart.ToString();
        To.text = pt.FromEnd.ToString();
        Value.text = pt.Value.ToString();
        Color c = Color.yellow;
        if (!open)
            c = Color.green;
        if (realPath)
            c = Color.blue;
        SR.color = c;
    }
}
