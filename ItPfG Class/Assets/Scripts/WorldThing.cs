using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldThing : MonoBehaviour
{
    public TileThing Location;
    public WorldThing.Types Type;
    public SpriteRenderer Body;
    
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
        Body = transform.Find("Body").GetComponent<SpriteRenderer>();
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
    //Note that this does no checks to see if a square is a valid place to go--that's in the Move function
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

    //When the player (or something else) tries to enter a square containing this, run this function
    //The return bool is whether you let the player enter or not. On a true the player can enter, on a false they can't
    public virtual bool GetBumped(WorldThing bumper)
    {
        //By default let's just say that if you try to enter this square nothing happens and you can't get in
        return false;
    }

    public enum Types
    {
        None=0,
        Player=1,
        Wall=2,
        Skeleton=3,
        ScoreThing=4,
        MagicDoor=5,
        RedKey=6
    }

}
