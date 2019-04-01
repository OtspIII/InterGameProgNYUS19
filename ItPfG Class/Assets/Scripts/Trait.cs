using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trait
{
    public List<EventType> ListenFor = new List<EventType>();

    public virtual void TakeMsg(WorldThing who, EventMsg msg)
    {
        
    }
}

public class EventMsg
{
    public EventType Type;
    public WorldThing Source;
    public int Amount;
    public Inputs Dir;
    public string Text = "";

    public EventMsg(EventType type,WorldThing source,int amt=0,Inputs dir=Inputs.None)
    {
        Type = type;
        Source = source;
        Amount = amt;
        Dir = dir;
    }
    public EventMsg(Inputs dir)
    {
        Type = EventType.PlayerInput;
        Source = null;
        Amount = 0;
        Dir = dir;
    }
}

public enum EventType
{
    None,
    GetBumped,
    TakeDmg,
    PlayerInput,
    GetName
}

public class KeyTrait : Trait
{
    public KeyTrait()
    {
        ListenFor.Add(EventType.GetBumped);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(WorldThing who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.GetBumped:
                WorldThing bumper = msg.Source;
                if (bumper.Type == WorldThing.Types.Player)
                {
                    TileThing loc = who.Location;
                    bumper.HasKey = true;
                    who.LeaveTile(who.Location);
                    who.transform.SetParent(bumper.transform);
                    who.transform.localPosition = new Vector3(0.25f,0.25f,-0.1f);
                    who.transform.localScale = new Vector3(0.5f,0.5f,1);
                    bumper.SetLocation(loc);
                    who.gameObject.SetActive(true);
                }
                return;
            case EventType.GetName:
                msg.Text += " KEY";
                return;
        }
        
    }
}

public class DoorTrait : Trait
 {
     public DoorTrait()
     {
         ListenFor.Add(EventType.GetBumped);
         ListenFor.Add(EventType.GetName);
     }
 
     public override void TakeMsg(WorldThing who, EventMsg msg)
     {
         switch (msg.Type)
         {
             case EventType.GetBumped:
                 WorldThing bumper = msg.Source;
                 if (bumper.HasKey)
                 {
                     TileThing loc = who.Location;
                     who.Location.Contents = null;
                     bumper.SetLocation(loc);
                     SceneManager.LoadScene("Game");
                 }
                 return;
             case EventType.GetName:
                 msg.Text += " DOOR";
                 return;
         }
         
     }
 }
 
public class MonsterTrait : Trait
{
    public MonsterTrait()
    {
        ListenFor.Add(EventType.GetBumped);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(WorldThing who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.GetBumped:
                msg.Source.TakeMsg(new EventMsg(EventType.TakeDmg,who,who.Species.Damage));
                who.Despawn();
                return;
            case EventType.GetName:
                msg.Text += " " + who.Species.Type;
                return;
        }
        
    }
}

public class ScoreTrait : Trait
{
    public ScoreTrait()
    {
        ListenFor.Add(EventType.GetBumped);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(WorldThing who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.GetBumped:
                WorldThing bumper = msg.Source;
                if (bumper.Type == WorldThing.Types.Player)
                {
                    TileThing loc = who.Location;
                    who.Despawn();
                    God.GSM.ChangeScore(1);
                    bumper.SetLocation(loc);
                }
                return;
            case EventType.GetName:
                msg.Text += " SCORETHING";
                return;
        }
        
    }
}

public class PlayerTrait : Trait
{
    public PlayerTrait()
    {
        ListenFor.Add(EventType.TakeDmg);
        ListenFor.Add(EventType.PlayerInput);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(WorldThing who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.TakeDmg:
                int amount = msg.Amount;
                God.GSM.TakeDamage(amount);
                return;
            case EventType.PlayerInput:
                if (msg.Dir == Inputs.Left)
                {
                    who.Move(-1,0);
                }
                else if (msg.Dir == Inputs.Right)
                {
                    who.Move(1,0);
                }
                else if (msg.Dir == Inputs.Up)
                {
                    who.Move(0,1);
                }
                else if (msg.Dir == Inputs.Down)
                {
                    who.Move(0,-1);
                }
                return;
            case EventType.GetName:
                msg.Text += " PLAYER";
                return;
        }
        
    }
}