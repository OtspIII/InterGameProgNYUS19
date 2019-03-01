using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldThing : MonoBehaviour
{
    public TileThing Location;
    
    //I put all my Start/Update code in virtual functions so they can be messed with more easily by children
    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    //When I first spawn a world thing I need to run setup on it so that it does basic stuff like place itself
    public virtual void Setup(TileThing start)
    {
        SetLocation(start);
    }

    //When I'm destroyed make sure I destroy myself safely
    public virtual void Despawn()
    {
        if (Location != null)
            LeaveTile(Location);
        Destroy(gameObject);
    }
    
    //This code is responsible for making a WT safely leave its old tile and join its new tile
    //Tiles keep track of what objects are in them
    //Also physically move to the tile
    public void SetLocation(TileThing tile)
    {
        if (Location != null)
            LeaveTile(Location);
        Location = tile;
        if (!Location.Contents.Contains(this))
            Location.Contents.Add(this);
        transform.SetParent(Location.transform);
        transform.localPosition = Vector3.zero;
    }

    //When you leave a tile remove yourself from its contents
    public void LeaveTile(TileThing tile)
    {
        tile.Contents.Remove(this);
    }

    

}
