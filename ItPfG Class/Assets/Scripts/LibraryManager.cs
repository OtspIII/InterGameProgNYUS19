using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public TileThing TileP;
    public List<SpawnableEntry> AllWTs;
    Dictionary<WorldThing.Types,Sprite> ThingDict = new Dictionary<WorldThing.Types, Sprite>();
    public WorldThing Thing;
    public List<MonsterType> Monsters;

    void Awake()
    {
        God.Library = this;
        foreach (SpawnableEntry se in AllWTs)
            ThingDict.Add(se.A,se.B);
    }
    
    public TileThing SpawnTile(int x, int y)
    {
        TileThing r = Instantiate(TileP).GetComponent<TileThing>();
        r.Setup(x,y);
        return r;
    }

    public WorldThing SpawnThing(WorldThing.Types t,TileThing tile)
    {
        WorldThing r = Instantiate(Thing).GetComponent<WorldThing>();
        r.Setup(tile,t);
        return r;
    }

    public MonsterType GetRandomMonster()
    {
        return Monsters[Random.Range(0, Monsters.Count)];
    }

    public Sprite GetSprite(WorldThing.Types t)
    {
        return ThingDict[t];
    }
}

[System.Serializable]
public struct SpawnableEntry
{
    public WorldThing.Types A;
    public Sprite B;
}

