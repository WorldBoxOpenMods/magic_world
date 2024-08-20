
using System;
using ai.behaviours;
using ai.behaviours.conditions;
using magic_world;

namespace magic_world;
class mw_job
{
    public static void init()
    {
        var Master = AssetManager.job_actor.add(new ActorJob
        {
            id = "Master"
        });
        // Master.addCondition(new CondCheckTeacher(), true);
        Master.addTask("magic_tower");
        Master.addTask("end_job");
        
        BehaviourTaskActor magic_tower = new()
        {
            id = "magic_tower",
            force_item_sprite = "tool_pickaxe"
        };
        magic_tower.addBeh(new BehCityFindBuilding("magic_tower"));
        magic_tower.addBeh(new BehGoToBuildingTarget(false));
        magic_tower.addBeh(new BehStayInBuildingTarget(100f, 150f));
        magic_tower.addBeh(new BehCheckJoinMagesGroup());
        magic_tower.addBeh(new BehMasterApprentice());
        magic_tower.addBeh(new BehExitBuilding());
        magic_tower.addBeh(new BehRestartTask());
        AssetManager.tasks_actor.add(magic_tower);
        AssetManager.citizen_job_library.add(new CitizenJobAsset
        {
            id = "Master",
            common_job = false,
            ok_for_king = false,
            ok_for_leader = false,
            unit_job_default ="Master"
        });
    }
}