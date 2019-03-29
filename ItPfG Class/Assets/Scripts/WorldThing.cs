using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldThing : MonoBehaviour
{
    public TileThing Location;
    public Types Type;
    protected SpriteRenderer Body;
    public MonsterType Species;
    public bool HasKey = false;

    //The code for this is all the way at the bottom of this class
    public List<Trait> Traits = new List<Trait>();
    public Dictionary<EventType,List<Trait>> Listeners = new Dictionary<EventType, List<Trait>>();
    
    void Update()
    {
        if (IM.Pressed(Inputs.Left))
        {
            TakeMsg(new EventMsg(Inputs.Left));
        }
        else if (IM.Pressed(Inputs.Right))
        {
            TakeMsg(new EventMsg(Inputs.Right));
        }
        else if (IM.Pressed(Inputs.Up))
        {
            TakeMsg(new EventMsg(Inputs.Up));
        }
        else if (IM.Pressed(Inputs.Down))
        {
            TakeMsg(new EventMsg(Inputs.Down));
        }
    }

    //When I first spawn a world thing I need to run setup on it so that it does basic stuff like place itself
    public void Setup(TileThing start,Types type)
    {
        Body = transform.Find("Body").GetComponent<SpriteRenderer>();
        Type = type;
        gameObject.name = Type.ToString();
        SetLocation(start);
        if (!God.GSM.AllThings.Contains(this))
            God.GSM.AllThings.Add(this);
        Body.sprite = God.Library.GetSprite(type);
        switch (type)
        {
            case Types.Skeleton:
                Species = God.Library.GetRandomMonster();
                Body.sprite = Species.S;
                AddTrait(new MonsterTrait());
                break;
            case Types.Player:
                AddTrait(new PlayerTrait());
                break;
            case Types.ScoreThing:
                AddTrait(new ScoreTrait());
                break;
            case Types.RedKey:
                AddTrait(new KeyTrait());
                break;
            case Types.MagicDoor:
                AddTrait(new DoorTrait());
                break;
        }
    }

    //When I'm destroyed make sure I destroy myself safely
    public void Despawn()
    {
        if (Location != null)
            LeaveTile(Location);
        gameObject.SetActive(false);
        God.GSM.AllThings.Remove(this);
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
        if (Location.Contents != null && Location.Contents != this)
            Debug.Log("I just orphaned a world thing at location " + tile.X + " / " + tile.Y);
        Location.Contents = this;
        transform.SetParent(Location.transform);
        transform.localPosition = Vector3.zero;
    }

    //When you leave a tile remove yourself from its contents
    public void LeaveTile(TileThing tile)
    {
        if (tile.Contents == this)
            tile.Contents =  null;
        transform.position = new Vector3(999,999,-999);
    }

    //When the player (or something else) tries to enter a square containing this, run this function
    //The return bool is whether you let the player enter or not. On a true the player can enter, on a false they can't
//    public void GetBumped(WorldThing bumper)
//    {
//        //By default let's just say that if you try to enter this square nothing happens and you can't get in
//        
//    }

    //Move to a position relative to your current location
    public void Move(int x, int y)
    {
        //Neighbor() asks the tile what the tile is relative to them with an x any offset
        TileThing target = Location.Neighbor(x, y);
        Move(target);
    }
    
    //If I try to move to a tile, run the bump code on any object in the area and if none stop me move there
    public void Move(TileThing target)
    {
        if (target == null)
            return;
        if (target.Contents != null)
        {
            EventMsg bumpMsg = new EventMsg(EventType.GetBumped,this);
            target.Contents.TakeMsg(bumpMsg);
        }
        else
            SetLocation(target);
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

    public void AddTrait(Trait t)
    {
        Traits.Add(t);
        foreach (EventType e in t.ListenFor)
        {
            if (!Listeners.ContainsKey(e))
                Listeners.Add(e,new List<Trait>());
            Listeners[e].Add(t);
        }
    }

    public void TakeMsg(EventMsg msg)
    {
        if (!Listeners.ContainsKey(msg.Type))
            return;
        foreach(Trait t in Listeners[msg.Type])
            t.TakeMsg(this,msg);
    }
}
