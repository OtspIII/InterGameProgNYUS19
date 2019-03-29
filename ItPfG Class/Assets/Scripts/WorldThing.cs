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
    public bool IsPlayer = false;

    public delegate void Bumped(WorldThing b);
    public Bumped GetBumped;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (!IsPlayer) //only runs for the player
            return;
        //If I hit an arrow key, move in that direction
        if (IM.Pressed(Inputs.Left))
        {
            Move(-1,0);
        }
        else if (IM.Pressed(Inputs.Right))
        {
            Move(1,0);
        }
        else if (IM.Pressed(Inputs.Up))
        {
            Move(0,1);
        }
        else if (IM.Pressed(Inputs.Down))
        {
            Move(0,-1);
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
                GetBumped = MonsterBump;
                break;
            case Types.Player:
                IsPlayer = true;
                break;
            case Types.ScoreThing:
                GetBumped = ScoreBump;
                break;
            case Types.RedKey:
                GetBumped = KeyBump;
                break;
            case Types.MagicDoor:
                GetBumped = DoorBump;
                break;
        }
    }

    //When I'm destroyed make sure I destroy myself safely
    public void Despawn()
    {
        if (Location != null)
            LeaveTile(Location);
        Destroy(gameObject);
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
            target.Contents.GetBumped(this);
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
    
    public void DoorBump(WorldThing bumper)
    {
        //If you enter the door and you have the key, reload the scene
        if (bumper.Type == Types.Player && bumper.HasKey)
        {
            TileThing loc = Location;
            Location.Contents = null;
            bumper.SetLocation(loc);
            SceneManager.LoadScene("Game");
        }
    }
    
    public void KeyBump(WorldThing bumper)
    {
        //If you touch this, you get a key! And the key goes on top of you
        if (bumper.Type == Types.Player)
        {
            TileThing loc = Location;
            bumper.HasKey = true;
            LeaveTile(Location);
            transform.SetParent(bumper.transform);
            transform.localPosition = new Vector3(0.25f,0.25f,-0.1f);
            transform.localScale = new Vector3(0.5f,0.5f,1);
            bumper.SetLocation(loc);
        }
    }
    
    public void MonsterBump(WorldThing bumper)
    {
        //Bump into a monster and kill it but take damage
        if (bumper.Type == Types.Player)
        {
            God.GSM.TakeDamage(Species.Damage);
            Despawn();
        }
    }
    
    public void ScoreBump(WorldThing bumper)
    {
        //If you stand on the score thing you get a point
        if (bumper.Type == Types.Player)
        {
            TileThing loc = Location;
            Despawn();
            God.GSM.ChangeScore(1);
            bumper.SetLocation(loc);
        }
    }

}
