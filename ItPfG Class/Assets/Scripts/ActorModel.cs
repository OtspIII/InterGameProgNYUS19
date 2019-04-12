using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorModel
{
    [System.NonSerialized]public ActorView View;

    public Guid ID;
    public Point Location = null;
    public ThingTypes Type;
    public MonsterType.Types Species;
    public bool HasKey = false;
    
    public List<Traits> TraitList = new List<Traits>();
    [NonSerialized]public Dictionary<EventType,List<Trait>> Listeners = new Dictionary<EventType, List<Trait>>();
    
    //When I first spawn a world thing I need to run setup on it so that it does basic stuff like place itself
    public ActorModel(TileModel start,ThingTypes thingType)
    {
        ID = Guid.NewGuid();
        Type = thingType;
        SetLocation(start);
        if (!ModelManager.AllThings.ContainsKey(ID))
            ModelManager.AllThings.Add(ID,this);
        switch (thingType)
        {
            case ThingTypes.Skeleton:
                Species = God.Library.GetRandomMonster().Type;
                AddTrait(Traits.Monster);
                break;
            case ThingTypes.Player:
                AddTrait(Traits.Player);
                break;
            case ThingTypes.ScoreThing:
                AddTrait(Traits.Score);
                break;
            case ThingTypes.RedKey:
                AddTrait(Traits.Key);
                break;
            case ThingTypes.MagicDoor:
                AddTrait(Traits.Door);
                break;
        }
    }
    
    //When I'm destroyed make sure I destroy myself safely
    public void Despawn()
    {
        if (GetLocation() != null)
            LeaveTile(GetLocation());
        God.C.AddAction(new VanishAction(this));
        ModelManager.AllThings.Remove(ID);
    }
    
    //Move to a position relative to your current location
    public void Move(int x, int y)
    {
        //Neighbor() asks the tile what the tile is relative to them with an x any offset
        TileModel target = GetLocation().Neighbor(x, y);
        Move(target);
        if (target == null || (target.GetContents() != null && target.GetContents() != this))
            God.C.AddAction(new BumpAction(this,Location.x + ((float)x)/2f,Location.y + ((float)y)/2f));
    }

    public TileModel GetLocation()
    {
        return ModelManager.GetTile(Location);
    }
    
    //If I try to move to a tile, run the bump code on any object in the area and if none stop me move there
    public void Move(TileModel target)
    {
        if (target == null)
            return;
        if (target.GetContents() != null)
        {
            EventMsg bumpMsg = new EventMsg(EventType.GetBumped,this);
            target.GetContents().TakeMsg(bumpMsg);
        }
        else
            SetLocation(target);
    }
    
    

    public void AddTrait(Traits tr)
    {
        TraitList.Add(tr);
        Install(God.Library.TraitDict[tr]);
        
    }

    public void Install(Trait t)
    {
        foreach (EventType e in t.ListenFor)
        {
            if (!Listeners.ContainsKey(e))
                Listeners.Add(e,new List<Trait>());
            Listeners[e].Add(t);
        }
    }

    public List<Trait> GetTraits()
    {
        List<Trait> r = new List<Trait>();
        foreach(Traits t in TraitList)
            r.Add(God.Library.TraitDict[t]);
        return r;
    }
    
    public void TakeMsg(EventMsg msg)
    {
        if (!Listeners.ContainsKey(msg.Type))
            return;
        foreach(Trait t in Listeners[msg.Type])
            t.TakeMsg(this,msg);
    }

    public override string ToString()
    {
        EventMsg nam = new EventMsg(EventType.GetName,null);
        nam.Text = "WT: ";
        TakeMsg(nam);
        return nam.Text;
    }
    
    //This code is responsible for making a WT safely leave its old tile and join its new tile
    //Tiles keep track of what objects are in them
    //Also physically move to the tile
    //Note that this does no checks to see if a square is a valid place to go--that's in the Move function
    public void SetLocation(TileModel tile)
    {
        if (GetLocation() != null)
            LeaveTile(GetLocation());
        Location = new Point(tile.X,tile.Y);
        if (GetLocation().GetContents() != null && GetLocation().GetContents() != this)
            Debug.Log("I just orphaned a world thing at location " + tile.X + " / " + tile.Y);
        GetLocation().Contents = ID;
        if (View != null) 
            God.C.AddAction(new MoveAction(this,tile));
    }

    //When you leave a tile remove yourself from its contents
    public void LeaveTile(TileModel tile)
    {
        if (tile.GetContents() == this)
            tile.Contents =  Guid.Empty;
    }

    public void OnLoad()
    {
        Listeners = new Dictionary<EventType, List<Trait>>();
     foreach(Trait t in GetTraits())
         Install(t);
    }
}

public enum ThingTypes
{
    None=0,
    Player=1,
    Wall=2,
    Skeleton=3,
    ScoreThing=4,
    MagicDoor=5,
    RedKey=6
}

public class VanishAction : GameAction
{
    public ActorModel Who;
    
    public VanishAction(ActorModel who)
    {
        Who = who;
    }

    public override IEnumerator Run()
    {
        Who.View.gameObject.SetActive(false);
        yield return null;
    }
}

public class MoveAction : GameAction
{
    public ActorModel Who;
    public TileModel Where;
    
    public MoveAction(ActorModel who, TileModel where)
    {
        Who = who;
        Where = where;
    }

    public override IEnumerator Run()
    {
        Who.View.transform.SetParent(Where.View.transform);
        float timer = 0;
        Vector3 startPos = Who.View.transform.localPosition;
        Vector3 endPos = new Vector3(0,0,-0.1f);
        while (timer < 1)
        {
            timer += Time.deltaTime / 0.1f;
            float t = God.Ease (timer, true);
            Who.View.transform.localPosition = Vector3.Lerp(startPos,endPos,t);
            yield return null;
        }
        Who.View.transform.localPosition = endPos;
    }
}