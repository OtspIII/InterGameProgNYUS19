using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Pathfinder
{
    public static List<TileThing> Pathfind(TileThing start, TileThing end){
        int safety = 999;
        List<PathTile> open = new List<PathTile>(){new PathTile(start,null,end)};
        Dictionary<TileThing,PathTile> closed = new Dictionary<TileThing, PathTile>();
        PathTile current = null;
        while (open.Count > 0 && safety > 0)
        {
            safety--;
            float best = 999;
            PathTile bTile = null;
            foreach (PathTile t in open)
                if (t.Value < best)
                {
                    best = t.Value;
                    bTile = t;
                }

            open.Remove(bTile);
            bTile.FindValue(false);
            if (bTile.Tile == end)
            {
                current = bTile;
                break;
            }

            //Just after you find your bTile
            foreach (TileThing nei in bTile.Tile.Neighbors())
            {
                if (nei.CanEnter())
                {
                    if (!closed.ContainsKey(nei))
                    {
                        PathTile pt = new PathTile(nei, bTile, end);
                        open.Add(pt);
                        closed.Add(nei, pt);
                        continue;
                    }

                    if (open.Contains(closed[nei]) && closed[nei].FromStart > bTile.FromStart + 1)
                    {
                        closed[nei].FromStart = bTile.FromStart + 1;
                        closed[nei].CameFrom = bTile;
                        closed[nei].FindValue(true);
                    }
                }
            }
        }
        List<TileThing> path = new List<TileThing>();
        while (current != null && current.Tile != start){
            //We add it to the start because we're tracing the path backwards
            path.Insert(0,current.Tile); 
            current = current.CameFrom;
        }
        return path;
    }
}

public class PathTile
{
    public TileThing Tile;
    public PathTile CameFrom;
    public float FromStart;
    public float FromEnd;
    
    public float Value;

    public PathTile(TileThing t, PathTile cf, TileThing dest){
        Tile = t; CameFrom = cf;
        FromStart = CameFrom != null ? CameFrom.FromStart + 1 : 0;
        FromEnd = Mathf.Abs(Tile.X - dest.X) + Mathf.Abs(Tile.Y - dest.Y);
        FindValue(true);
    }

    public void FindValue(bool open){
        Value = FromStart + FromEnd;
        Tile.SetDemoInfo(this,open,false);
    }
}