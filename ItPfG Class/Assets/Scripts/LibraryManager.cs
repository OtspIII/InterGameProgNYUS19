using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public TileView TileP;
    public List<SpawnableEntry> AllWTs;
    Dictionary<ThingTypes,Sprite> ThingDict = new Dictionary<ThingTypes, Sprite>();
    public ActorView Thing;
    public List<MonsterType> Monsters;
    public Dictionary<Traits,Trait> TraitDict = new Dictionary<Traits, Trait>();

    void Awake()
    {
        God.Library = this;
        foreach (SpawnableEntry se in AllWTs)
            ThingDict.Add(se.A,se.B);
        new KeyTrait().Setup();
        new DoorTrait().Setup();
        new MonsterTrait().Setup();
        new ScoreTrait().Setup();
        new PlayerTrait().Setup();
    }
    
    public TileView SpawnTile(TileModel t)
    {
        TileView r = Instantiate(TileP).GetComponent<TileView>();
        r.Setup(t);
        return r;
    }

    public ActorView SpawnThing(ActorModel m)
    {
        ActorView r = Instantiate(Thing).GetComponent<ActorView>();
        r.Setup(m);
        return r;
    }

    public MonsterType GetRandomMonster()
    {
        return Monsters[Random.Range(0, Monsters.Count)];
    }
    
    public MonsterType GetMonster(MonsterType.Types t)
    {
        foreach(MonsterType m in Monsters)
            if (m.Type == t)
                return m;
        return null;
    }

    public Sprite GetSprite(ThingTypes t)
    {
        return ThingDict[t];
    }
}

[System.Serializable]
public struct SpawnableEntry
{
    public ThingTypes A;
    public Sprite B;
}

