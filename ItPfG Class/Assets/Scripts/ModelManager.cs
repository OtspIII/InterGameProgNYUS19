using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModelManager
{
    public static bool Setup = false;
    
    public static List<ActorModel> AllThings = new List<ActorModel>();
    public static List<TileModel> AllTiles = new List<TileModel>();
    public static Dictionary<int, Dictionary<int, TileModel>> Tiles = new Dictionary<int, Dictionary<int, TileModel>>();
    public static int Score;
    public static int HP = 10;
    
    public static void Init()
    {
        if (Setup)
            return;
        Setup = true;
    }
    
    public static void BuildWorld()
    {
        AllThings.Clear();
        AllTiles.Clear();
        Tiles.Clear();
        for (int x = -GameSettings.MapSizeX /2; x <= GameSettings.MapSizeX /2; x++)
        {
            for (int y = -GameSettings.MapSizeY /2; y <= GameSettings.MapSizeY /2; y++)
            {
                TileModel t = new TileModel(x,y);
                God.Library.SpawnTile(t);
            }
        }

        List<TileModel> openTiles = new List<TileModel>();
        openTiles.AddRange(AllTiles);
        foreach (ThingTypes t in GameSettings.MapContents)
        {
            if (openTiles.Count == 0)
                break;
            TileModel rand = openTiles[Random.Range(0, openTiles.Count)];
            openTiles.Remove(rand);
            ActorModel a = new ActorModel(rand,t);
            God.Library.SpawnThing(a);
        }
    }
    
    //Feed me coordinates and I'll tell you the tile that lives there
    public static TileModel GetTile(int x, int y)
    {
        if (!Tiles.ContainsKey(x) || !Tiles[x].ContainsKey(y))
            return null;
        return Tiles[x][y];
    }
    
    //I can get a list of all the TYPE of a thing to exist.
    //If I don't specify a type it just gives me all world things that exist period
    //This doesn't work yet, since I don't have a way to populate AllThings
    public static List<ActorModel> GetThings(ThingTypes thingType = ThingTypes.None)
    {
        List<ActorModel> r = new List<ActorModel>();
        foreach(ActorModel wt in AllThings)
            if (thingType == ThingTypes.None || wt.Type == thingType)
                r.Add(wt);
        return r;
    }
    
    public static List<T> GetThings<T>() where T:ActorModel
    {
        List<T> r = new List<T>();
        foreach(ActorModel wt in AllThings)
            if (wt is T)
                r.Add((T)wt);
        return r;
    }
    
    //This makes my points go up or down and also updates the text in the top left corner
    public static void ChangeScore(int amt)
    {
        Score += amt;
    }
    
    public static void TakeDamage(int amt)
    {
        HP -= amt;
        if (HP <= 0)
        {
            HP = 0;
            God.C.AddAction(new DeathAction(GetThings(ThingTypes.Player)[0]));
        }
    }
}
