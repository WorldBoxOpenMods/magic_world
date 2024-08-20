using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using System.Reflection;
using Newtonsoft.Json;
using System;
using NCMS;
using static Config;
using UnityEngine.Purchasing.MiniJSON;
using System.Collections;
using System.IO.Compression;
using System.Threading;
using System.Text;
using ai;
using ai.behaviours;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using magic_world.Utils;
using System.CodeDom;
using System.Security.Cryptography;
using System.Runtime.Remoting.Messaging;

namespace magic_world
{
    class mw_harmony_city
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "updateAge")]
        public static bool updateAge(City __instance)
        {
            if (__instance == null) return false;
            __instance.data.set("MagesNum", 0);
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CityBehBuild), "buildTick")]
        public static bool buildTick(City pCity, ref bool __result)
        {
            if (pCity == null)
            {
                __result = false;
                return false;
            }
            // 检查是否有正在建造的建筑
            if (pCity.tasks.fire > 0)
            {
                __result = false;
                return false;
            }

            // 寻找当前正在建造的建筑
            if (pCity.underConstructionBuilding == null)
            {
                foreach (Building building in pCity.buildings)
                {
                    if (building.isUnderConstruction())
                    {
                        pCity.underConstructionBuilding = building;
                        break;
                    }
                }
            }

            // 如果有正在建造的建筑，则返回
            if (pCity.underConstructionBuilding != null)
            {
                __result = false;
                return false;
            }

            // 计算可建造的建筑列表
            CityBehBuild.calcPossibleBuildings(pCity);

            // 在调试模式下重置调试信息
            if (DebugConfig.isOn(DebugOption.OverlayCity))
            {
                pCity._debug_last_possible_build_orders = string.Empty;
                pCity._debug_last_possible_build_orders_no_resources = string.Empty;
                pCity._debug_last_build_order_try = string.Empty;
            }

            // 如果没有可建造的建筑，则返回
            if (CityBehBuild._possible_buildings.Count == 0)
            {
                __result = false;
                return false;
            }

            // 随机选择一个建筑进行建造或升级
            BuildOrder random2 = CityBehBuild._possible_buildings.GetRandom<BuildOrder>();

            // 在调试模式下记录选择的建筑
            if (DebugConfig.isOn(DebugOption.OverlayCity))
            {
                foreach (BuildOrder buildOrder in CityBehBuild._possible_buildings)
                {
                    pCity._debug_last_possible_build_orders = pCity._debug_last_possible_build_orders + (buildOrder.upgrade ? "U-" : "") + buildOrder.id + "; ";
                }
                foreach (BuildOrder buildOrder2 in CityBehBuild._possible_buildings_no_resources)
                {
                    pCity._debug_last_possible_build_orders_no_resources = pCity._debug_last_possible_build_orders_no_resources + (buildOrder2.upgrade ? "U-" : "") + buildOrder2.id + "; ";
                }
                pCity._debug_last_build_order_try = (random2.upgrade ? "U-" : "") + random2.id;
            }

            // 清空可建造的建筑列表
            CityBehBuild._possible_buildings.Clear();
            CityBehBuild._possible_buildings_no_resources.Clear();

            // 如果选择的是升级建筑
            if (random2.upgrade)
            {

                // 查找可以升级的建筑，并进行升级
                BuildingContainer buildingContainerExact = pCity.getBuildingContainerExact(random2.getBuildingAsset(pCity, null).id);
                if (buildingContainerExact == null)
                {
                    __result = false;
                    return false;
                }
                Building random3 = buildingContainerExact.GetRandom();
                if (random3 == null)
                {
                    __result = false;
                    return false;
                }
                CityBehBuild.upgradeBuilding(random3, pCity);
                __result = true;
                return false;
            }
            else // 如果选择的是新建筑
            {
                // 尝试建造新建筑
                BuildingAsset buildingAsset = random2.getBuildingAsset(pCity, null);
                if (buildingAsset.id == "magic_tower_human_1")
                {
                    pCity.data.get("MagesNum", out int pResult, 0);
                    // if (pResult <= 20)
                    // {
                    //     return false;
                    // }
                    buildingAsset = AssetManager.buildings.get($"magic_tower_{pCity.race.id}_{Toolbox.randomInt(1, 10)}");
                    if(buildingAsset==null)
                    {
                        buildingAsset = AssetManager.buildings.get($"magic_tower_human_{Toolbox.randomInt(1, 10)}");
                    }
                    Building building2 = CityBehBuild.tryToBuild(pCity, buildingAsset);
                    if (building2 == null)
                    {
                        return true;
                    }
                    // 如果有建造加速的调试选项，则立即完成建造
                    if (DebugConfig.isOn(DebugOption.CityFastConstruction))
                    {
                        if (building2 != null)
                        {
                            building2.updateBuild(1000);
                        }
                        pCity.underConstructionBuilding = null;
                    }
                    // 如果文化技术允许，则在建造后进行道路建设
                    Culture culture = pCity.getCulture();
                    if (culture != null && culture.hasTech("building_roads") && building2 != null)
                    {
                        CityBehBuild.makeRoadsBuildings(pCity, building2);
                    }
                    __result = true;
                    return false;
                }
                return true;
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CityBehCheckCitizenTasks), "execute")]
        public static void execute(CityBehCheckCitizenTasks __instance, City pCity, ref BehResult __result)
        {
            pCity.data.get("MagesNum", out int pResult, 0);
            pCity.jobs.addToJob(AssetManager.citizen_job_library.get("Master"), 20);
            __result = BehResult.Continue;
            return;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "updateCityStatus")]
        public static bool updateCityStatus(City __instance)
        {
            if (__instance == null) { return false; }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "updateCitizens")]
        public static bool updateCitizens(City __instance)
        {
            if (__instance == null) { return false; }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CityBehCheckCulture), "recalcMainCulture")]
        public static bool recalcMainCulture(City __instance)
        {
            if (__instance == null) { return false; }
            return true;
        }
    }
}