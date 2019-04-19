using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterThing : WorldThing
{
    public MonsterType Species;
    
    protected override void OnStart()
    {
        base.OnStart();
        Species = God.Library.GetRandomMonster();
        Body.sprite = Species.S;
    }

    public override void GetBumped(WorldThing bumper)
    {
        //Bump into a monster and kill it but take damage
        if (bumper.Type == Types.Player)
        {
            
            God.GSM.TakeDamage(Species.Damage);
            Despawn();
        }
    }
}
