using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterThing : WorldThing
{
    protected override void OnStart()
    {
        base.OnStart();
    }

    public override bool GetBumped(WorldThing bumper)
    {
        //Bump into a monster and you die
        if (bumper.Type == Types.Player)
        {
            bumper.Despawn();
            //GSM.SetText("You Died");
            return false;
        }

        return base.GetBumped(bumper);
    }

    

//    public List<MTypes> MonsterTypes = new List<MTypes> { MTypes.Skeleton, MTypes.Demon, MTypes.Dragon, MTypes.Bear,
//        MTypes.Goblin, MTypes.Werewolf, MTypes.GiantSpider };
//
//    public enum MTypes
//    {
//        None=0,
//        Skeleton=1,
//        Demon=2,
//        Dragon=3,
//        Bear=4,
//        Goblin=5,
//        Werewolf=6,
//        GiantSpider=7
//    }

//This is just here as a reminder/shortcut for me
//    Sprite[] res = Resources.LoadAll<Sprite> ("Characters");
//        foreach (Sprite s in res) {
//        MonsterThing.MTypes t = (MonsterThing.MTypes)System.Enum.Parse(typeof(MonsterThing.MTypes), s.name);
//        Monsters.Add (t, s);
//    }

}
