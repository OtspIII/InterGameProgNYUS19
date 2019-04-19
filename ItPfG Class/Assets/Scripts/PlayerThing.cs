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
        //First, we find a path...
        List<TileThing> path = Pathfinder.Pathfind(Location,targ);
        
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
