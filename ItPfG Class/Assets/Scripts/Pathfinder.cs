using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Pathfinder
{
    public static List<TileThing> Pathfind(TileThing start, TileThing end)
    {
        List<TileThing> path = new List<TileThing>();
        int safety = 999;
        while (!path.Contains(end) && safety > 0)
        {
            TileThing current = path.Count > 0 ? path[path.Count - 1] : start;
            safety--;
            //Here's a drunken walk pathfinding method. Just walk randomly until you find the target. It is. . .not good
            List<TileThing> adj = new List<TileThing>();
            foreach(TileThing nei in current.Neighbors())
                if (nei.CanEnter())
                    adj.Add(nei);
            path.Add(adj[Random.Range(0,adj.Count)]);
        }
        return path;
    }
}