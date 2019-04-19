using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerThing : WorldThing
{
    
    public bool HasKey = false;
    
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TileThing door = God.GSM.GetThings(Types.MagicDoor)[0].Location;
            if (door == Location)
                SceneManager.LoadScene("Game");
            else
                God.GSM.StartCoroutine(WalkToTarget(door));
        }
    }

    public IEnumerator WalkToTarget(TileThing targ)
    {
        bool auto = false;
        float timer = 0.2f;
        //First, we find a path...
        int safety = 999;
        List<PathTile> open = new List<PathTile>(){new PathTile(Location,null,targ)};
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
            if (bTile.Tile == targ)
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
                        PathTile pt = new PathTile(nei, bTile, targ);
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

            
            while (!Input.GetKeyUp(KeyCode.Space) && (!auto || timer > 0))
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                    auto = true;
                timer -= Time.deltaTime;
                yield return null;
            }

            timer = 0.2f;
            yield return null;
        }
        List<TileThing> path = new List<TileThing>();
        while (current != null && current.Tile != Location){
            current.Tile.SetDemoInfo(current,false,true);
            while (!Input.GetKeyUp(KeyCode.Space) && (!auto || timer > 0))
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                    auto = true;
                timer -= Time.deltaTime;
                yield return null;
            }

            timer = 0.2f;
            yield return null;
            //We add it to the start because we're tracing the path backwards
            path.Insert(0,current.Tile); 
            current = current.CameFrom;
            
        }
        
        Debug.Log("PATH: " + path.Count);
        if (!path.Contains(targ))
        {
            God.GSM.SetText("COULDN'T FIND EXIT");
            yield break;
        }

        float walkSpeed = 0.2f;
        //If it's a real long path speed up your walking a bit...
        for (int n = path.Count / 10; n > 0; n--)
            walkSpeed *= 0.75f;
        //Then we take it, step by step
        foreach (TileThing t in path)
        {
            if (!t.CanEnter() || !Location.Neighbors().Contains(t))
            {
                God.GSM.SetText("INVALID MOVE");
                yield break;
            }
            Move(t);
            if (path.Count - path.IndexOf(t) < 30)
                walkSpeed = 0.2f;
            yield return new WaitForSeconds(walkSpeed);
        }
    }
}
