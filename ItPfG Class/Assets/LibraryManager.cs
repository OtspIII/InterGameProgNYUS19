using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public TileThing TileP;
    public List<SpawnableEntry> AllWTs;
    Dictionary<WorldThing.Types,WorldThing> ThingDict = new Dictionary<WorldThing.Types, WorldThing>();
    Dictionary<MonsterThing.MTypes,Sprite> Monsters = new Dictionary<MonsterThing.MTypes, Sprite>();

    void Awake()
    {
        God.Library = this;
        foreach (SpawnableEntry se in AllWTs)
            ThingDict.Add(se.A,se.B);
        Sprite[] res = Resources.LoadAll<Sprite> ("Characters");
        foreach (Sprite s in res) {
            MonsterThing.MTypes t = (MonsterThing.MTypes)System.Enum.Parse(typeof(MonsterThing.MTypes), s.name);
            Monsters.Add (t, s);
        }

    }
    
    public TileThing SpawnTile(int x, int y)
    {
        TileThing r = Instantiate(TileP).GetComponent<TileThing>();
        r.Setup(x,y);
        return r;
    }

    public WorldThing SpawnThing(WorldThing.Types t,TileThing tile)
    {
        if (!ThingDict.ContainsKey(t))
            return null;
        WorldThing r = Instantiate(ThingDict[t]).GetComponent<WorldThing>();
        r.Setup(tile);
        return r;
    }

    public Sprite GetMonster(MonsterThing.MTypes t)
    {
        if (!Monsters.ContainsKey(t))
            return null;
        return Monsters[t];
    }
}

[System.Serializable]
public struct SpawnableEntry
{
    public WorldThing.Types A;
    public WorldThing B;
}

