using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Trait
{
    public Traits Type;
    public List<EventType> ListenFor = new List<EventType>();

    public void Setup()
    {
        God.Library.TraitDict.Add(Type,this);
    }

    public abstract void TakeMsg(ActorModel who, EventMsg msg);

}

public enum Traits
{
    None=0,
    Key=1,
    Door=2,
    Monster=3,
    Score=4,
    Player=5,
}

public class EventMsg
{
    public EventType Type;
    public ActorModel Source;
    public int Amount;
    public Inputs Dir;
    public string Text = "";

    public EventMsg(EventType type,ActorModel source,int amt=0,Inputs dir=Inputs.None)
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
        Type = Traits.Key;
        ListenFor.Add(EventType.GetBumped);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(ActorModel who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.GetBumped:
                ActorModel bumper = msg.Source;
                if (bumper.Type == ThingTypes.Player)
                {
                    TileModel loc = who.GetLocation();
                    bumper.HasKey = true;
                    who.LeaveTile(who.GetLocation());
                    bumper.SetLocation(loc);
                    God.C.AddAction(new GainKeyAction(bumper,who));
                    
                }
                return;
            case EventType.GetName:
                msg.Text += " KEY";
                return;
        }
        
    }
}

public class GainKeyAction : GameAction
{
    public ActorModel Player;
    public ActorModel Key;

    public GainKeyAction(ActorModel player, ActorModel key)
    {
        Player = player;
        Key = key;
    }
    
    public override IEnumerator Run()
    {
        Key.View.transform.SetParent(Player.View.transform);
        Key.View.gameObject.SetActive(true);
        float timer = 0;
        Vector3 startPos = Key.View.transform.localPosition;
        Vector3 endPos = new Vector3(0.25f,0.25f,-0.1f);
        Vector3 startSize = Key.View.transform.localScale;
        Vector3 endSize = new Vector3(0.5f,0.5f,1);
        while (timer < 1)
        {
            timer += Time.deltaTime / 0.2f;
            float t = God.Ease (timer, true);
            Key.View.transform.localPosition = Vector3.Lerp(startPos,endPos,t);
            Key.View.transform.localScale = Vector3.Lerp(startSize,endSize,t);
            yield return null;
        }
        Key.View.transform.localPosition = endPos;
        Key.View.transform.localScale = endSize;
    }
}

public class DoorTrait : Trait
 {
     public DoorTrait()
     {
         Type = Traits.Door;
         ListenFor.Add(EventType.GetBumped);
         ListenFor.Add(EventType.GetName);
     }
 
     public override void TakeMsg(ActorModel who, EventMsg msg)
     {
         switch (msg.Type)
         {
             case EventType.GetBumped:
                 ActorModel bumper = msg.Source;
                 if (bumper.HasKey)
                 {
                     God.C.AddAction(new UseDoorAction(bumper,who));
                     ModelManager.SaveGame();
                 }
                 return;
             case EventType.GetName:
                 msg.Text += " DOOR";
                 return;
         }
         
     }
 }

public class UseDoorAction : GameAction
{
    public ActorModel Player;
    public ActorModel Door;

    public UseDoorAction(ActorModel player, ActorModel door)
    {
        Player = player;
        Door = door;
    }
    
    public override IEnumerator Run()
    {
        Player.View.transform.SetParent(Door.View.transform);
        float timer = 0;
        Vector3 startPos = Player.View.transform.localPosition;
        Vector3 endPos = new Vector3(0,0,-0.1f);
        Vector3 startSize = Player.View.transform.localScale;
        Vector3 endSize = new Vector3(0,0,1);
        while (timer < 1)
        {
            timer += Time.deltaTime / 0.5f;
            float t = God.Ease (timer, true);
            Player.View.transform.localPosition = Vector3.Lerp(startPos,endPos,t);
            Player.View.transform.localScale = Vector3.Lerp(startSize,endSize,t);
            Player.View.transform.Rotate(0,0,10);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Game");
    }
}
 
public class MonsterTrait : Trait
{
    public MonsterTrait()
    {
        Type = Traits.Monster;
        ListenFor.Add(EventType.GetBumped);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(ActorModel who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.GetBumped:
                God.C.AddAction(new BumpAction(msg.Source,who.Location.x,who.Location.y));
                msg.Source.TakeMsg(new EventMsg(EventType.TakeDmg,who,God.Library.GetMonster(who.Species).Damage));
                who.Despawn();
                return;
            case EventType.GetName:
                msg.Text += " " + God.Library.GetMonster(who.Species).Type;
                return;
        }
        
    }
}

public class BumpAction : GameAction
{
    public ActorModel Player;
    public float DirX;
    public float DirY;

    public BumpAction(ActorModel player, float x, float y)
    {
        Player = player;
        DirX = x;
        DirY = y;
    }
    
    public override IEnumerator Run()
    {
        float timer = 0;
        Vector3 startPos = Player.View.transform.position;
        Vector3 endPos = new Vector3(DirX,DirY,-0.1f);
        while (timer < 1)
        {
            timer += Time.deltaTime / 0.1f;
            float t = Mathf.Sin(timer * Mathf.PI);
            Player.View.transform.position = Vector3.Lerp(startPos,endPos,t);
            yield return null;
        }
        Player.View.transform.localPosition = Vector3.zero;
        God.GSM.UpdateText();
    }
}

public class ScoreTrait : Trait
{
    public ScoreTrait()
    {
        Type = Traits.Score;
        ListenFor.Add(EventType.GetBumped);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(ActorModel who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.GetBumped:
                ActorModel bumper = msg.Source;
                if (bumper.Type == ThingTypes.Player)
                {
                    TileModel loc = who.GetLocation();
                    God.C.AddAction(new GetScoreAction(bumper,who));
                    who.Despawn();
                    ModelManager.ChangeScore(1);
                    bumper.SetLocation(loc);
                }
                return;
            case EventType.GetName:
                msg.Text += " SCORETHING";
                return;
        }
        
    }
}

public class GetScoreAction : GameAction
{
    public ActorModel Player;
    public ActorModel Score;

    public GetScoreAction(ActorModel player, ActorModel score)
    {
        Player = player;
        Score = score;
    }
    
    public override IEnumerator Run()
    {
        Score.View.transform.SetParent(Player.View.transform);
        float timer = 0;
        Vector3 startPos = Score.View.transform.localPosition;
        Vector3 endPos = new Vector3(0,0,-0.1f);
        Vector3 startSize = Score.View.transform.localScale;
        Vector3 endSize = new Vector3(0,0,1);
        while (timer < 1)
        {
            timer += Time.deltaTime / 0.2f;
            float t = God.Ease (timer, true);
            Score.View.transform.localPosition = Vector3.Lerp(startPos,endPos,t);
            Score.View.transform.localScale = Vector3.Lerp(startSize,endSize,t);
            Score.View.transform.Rotate(0,0,30);
            yield return null;
        }
        God.GSM.UpdateText();
    }
}

public class PlayerTrait : Trait
{
    public PlayerTrait()
    {
        Type = Traits.Player;
        ListenFor.Add(EventType.TakeDmg);
        ListenFor.Add(EventType.PlayerInput);
        ListenFor.Add(EventType.GetName);
    }

    public override void TakeMsg(ActorModel who, EventMsg msg)
    {
        switch (msg.Type)
        {
            case EventType.TakeDmg:
                int amount = msg.Amount;
                God.C.AddAction(new TakeDamageAction(who,amount));
                ModelManager.TakeDamage(amount);
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

public class TakeDamageAction : GameAction
{
    public ActorModel Player;
    public float Amount;

    public TakeDamageAction(ActorModel player, float amt)
    {
        Player = player;
        Amount = amt / 10;
    }
    
    public override IEnumerator Run()
    {
        float timer = 0;
        while (timer < Amount)
        {
            timer += Time.deltaTime;
            Player.View.transform.localPosition = new Vector3(Random.Range(-Amount,Amount),Random.Range(-Amount,Amount),0);
            yield return null;
        }
        Player.View.transform.localPosition = Vector3.zero;
        God.GSM.UpdateText();
    }
}

public class DeathAction : GameAction
{
    public ActorModel Who;
    
    public DeathAction(ActorModel who)
    {
        Who = who;
    }

    public override IEnumerator Run()
    {
        float timer = 0;
        float size = Who.View.transform.localScale.x;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            size *= 1.2f;
            Who.View.transform.localScale = new Vector3(size,size,1);
            Who.View.transform.Rotate(0,0,30);
            yield return null;
        }
        God.GSM.SetText("You Died");
    }
}