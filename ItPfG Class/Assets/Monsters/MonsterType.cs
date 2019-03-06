using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="MonsterTypes" ,menuName="Monster Type")]
public class MonsterType : ScriptableObject
{
    public Types Type;
    public Sprite S;
    public int Damage;
    
    public enum Types
    {
        None=0,
        Skeleton=1,
        Demon=2,
        Dragon=3,
        Bear=4,
        Goblin=5,
        Werewolf=6,
        GiantSpider=7
    }
}
