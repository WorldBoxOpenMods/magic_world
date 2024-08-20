using System.Collections.Generic;
using ai.behaviours;
using UnityEngine;


namespace magic_world;
public class BehCheckJoinMagesGroup : BehaviourActionActor
{
    public override BehResult execute(Actor pActor)
    {
        if (pActor == null)
        {
            return BehResult.Stop;
        }
        pActor.data.get("IsMaster", out bool falg, false);
        if (!falg||pActor.insideBuilding==null||!pActor.is_inside_building)
        {
            return BehResult.Stop;
        }
        if(Main.MagesGroup.ContainsKey(pActor.insideBuilding))
        {
            Main.MagesGroup[pActor.insideBuilding].Add(pActor);
        }
        else
        {
            Main.MagesGroup.Add(pActor.insideBuilding,new List<Actor> {pActor});
        }
        return BehResult.Continue;
    }
}