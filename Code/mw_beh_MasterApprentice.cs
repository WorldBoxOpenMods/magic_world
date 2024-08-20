using ai.behaviours;
using magic_world.Utils;
using UnityEngine;

namespace magic_world;

public class BehMasterApprentice : BehaviourActionActor
{
    public override BehResult execute(Actor pActor)
    {
        if (!pActor.Any()) { return BehResult.Stop; }
        pActor.data.get("IsTeacher", out bool falg, false);
        if (!falg || pActor.insideBuilding == null || !pActor.is_inside_building)
        {
            return BehResult.Stop;
        }
        if (Main.MagesGroup.ContainsKey(pActor.insideBuilding))
        {
            if (MWTools.FindTeather(pActor,Main.MagesGroup[pActor.insideBuilding]) != null)
            {
                Debug.Log("拜师");
            }
        }
        return BehResult.Continue;
    }
}
