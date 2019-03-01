using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterThing : WorldThing
{
    protected override void OnStart()
    {
        base.OnStart();
        MTypes species = MonsterTypes[Random.Range(0, MonsterTypes.Count)];
        Body.sprite = God.Library.GetMonster(species);
    }

    public override bool GetBumped(WorldThing bumper)
    {
        //Bump into a monster and you die
        if (bumper.Type == Types.Player)
        {
            bumper.Despawn();
            God.GSM.SetText("You Died");
            return false;
        }

        return base.GetBumped(bumper);
    }

    

    public List<MTypes> MonsterTypes = new List<MTypes> { MTypes.Skeleton, MTypes.Demon, MTypes.Dragon, MTypes.Bear,
        MTypes.Goblin, MTypes.Werewolf, MTypes.GiantSpider };

    public enum MTypes
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
