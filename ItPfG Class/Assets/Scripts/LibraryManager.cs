using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public TileThing TileP;
    public List<SpawnableEntry> AllWTs;
    Dictionary<WorldThing.Types,WorldThing> ThingDict = new Dictionary<WorldThing.Types, WorldThing>();
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
        WorldThing prefab = (ThingDict.ContainsKey(t) ? ThingDict[t] : null);
//        if (ThingDict.ContainsKey(t))
//            prefab = ThingDict[t];
//        else
//            prefab = null;
        if (prefab == null)
            return null;
        WorldThing r = Instantiate(prefab).GetComponent<WorldThing>();
        r.Setup(tile);
        return r;
    }

    public MonsterType GetRandomMonster()
    {
        return Monsters[Random.Range(0, Monsters.Count)];
    }
}

[System.Serializable]
public struct SpawnableEntry
{
    public WorldThing.Types A;
    public WorldThing B;
}

