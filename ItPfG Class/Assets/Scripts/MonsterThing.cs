using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterThing : WorldThing
{
    public override bool GetBumped(WorldThing bumper)
    {
        //Bump into a skeleton and you die
        if (bumper.Type == Types.Player)
        {
            bumper.Despawn();
            //GSM.SetText("You Died");
            return false;
        }

        return base.GetBumped(bumper);
    }

    public override void Setup(TileThing start)
    {
        base.Setup(start);
        
    }

//    public List<MTypes> MonsterTypes = new List<MTypes> { MTypes.Skeleton, MTypes.Demon, MTypes.Dragon, MTypes.Bear,
//        MTypes.Goblin, MTypes.Werewolf };
//
//    public enum MTypes
//    {
//        None=0,
//        Skeleton=1,
//        Demon=2,
//        Dragon=3,
//        Bear=4,
//        Goblin=5,
//        Werewolf=6
//    }

//This is just here as a reminder/shortcut for me
//    object[] res = Resources.LoadAll ("Characters", typeof(Sprite));
//        foreach (object o in res) {
//        Sprite s = (Sprite)o;
//        MonsterThing.MTypes t = (MonsterThing.MTypes)System.Enum.Parse(typeof(MonsterThing.MTypes), s.name);
//        Monsters.Add (t, s);
//    }
}
