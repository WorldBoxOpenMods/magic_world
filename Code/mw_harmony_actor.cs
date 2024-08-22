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
using System.Globalization;
using System.Runtime.Remoting;

namespace magic_world
{
	class mw_harmony_actor
	{
		private static List<Actor> _force_temp_actor_list = new List<Actor>();
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "newCreature")]
		public static void newCreature(Actor __instance)
		{
			if (__instance.Any() && __instance.asset.unit && !Main.Actor_Magic.ContainsKey(__instance) && __instance.stats[mw_S.magic_power] == 0 && Toolbox.randomChance(0.9f))
			{
				__instance.MagicBegining();
			}
		}
		[HarmonyPostfix]
		[HarmonyPatch(typeof(ActorBase), "updateStats")]
		public static void updateStats(ActorBase __instance)
		{
			Actor actor = (Actor)__instance;
			if (!actor.Any()) { return; }
			BaseStats stats = new BaseStats();
			if (Main.Actor_Magic.ContainsKey(actor))
			{
				foreach (magic action in Main.Actor_Magic[actor])
				{
					stats.mergeStats(action.stats);
				}

			}
			actor.data.get(mw_S.magic_power, out int result, 0);
			actor.stats[mw_S.magic_power] = (float)result;
			// if (stats.stats_list != null && stats.stats_list.Any())
			// {
			// 	if (actor.stats.mods_list != null)
			// 	{
			// 		for (int i = 0; i < actor.stats.mods_list.Count; i++)
			// 		{
			// 			BaseStatsContainer stats2 = actor.stats.mods_list[i];
			// 			BaseStatsContainer stats3 = actor.stats.getContainer(stats2.getAsset().main_stat_to_mod);
			// 			if (stats3 != null)
			// 			{
			// 				stats3.value /= 1f + stats2.value;
			// 			}
			// 		}
			// 	}
			// 	actor.stats.mergeStats(stats);
			// }
			actor.stats.mergeStats(stats);

			if (actor.data.health > actor.getMaxHealth())
			{
				actor.data.health = actor.getMaxHealth();
			}
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(CityBehProduceUnit), "produceNewCitizen")]
		public static bool produceNewCitizen(CityBehProduceUnit __instance, Building pBuilding, City pCity, ref bool __result)
		{
			if (__instance._possibleParents.Count == 0)
			{
				__result = false;
				return true;
			}

			Actor actor = __instance._possibleParents.Pop();
			if (actor == null)
			{
				__result = false;
				return true;
			}

			if (!Toolbox.randomChance(actor.stats[S.fertility]))
			{
				__result = false;
				return true;
			}

			Actor actor2 = null;

			if (__instance._possibleParents.Count > 0)
			{
				actor2 = __instance._possibleParents.Pop<Actor>();
			}
			if (!actor2.Any() && actor.stats == null && actor.stats[mw_S.magic_power] < 80)
			{
				return true;
			}
			ResourceAsset foodItem = pCity.getFoodItem(null);
			pCity.eatFoodItem(foodItem.id);
			pCity.status.housingFree--;
			pCity.data.born++;
			if (pCity.kingdom != null)
			{
				pCity.kingdom.data.born++;
			}
			ActorAsset asset = actor.asset;
			ActorData actorData = new()
			{
				created_time = BehaviourActionBase<City>.world.getCreationTime(),
				cityID = pCity.data.id,
				id = BehaviourActionBase<City>.world.mapStats.getNextId("unit"),
				asset_id = asset.id
			};
			ActorBase.generateCivUnit(actor.asset, actorData, actor.race);
			actorData.generateTraits(asset, actor.race);
			actorData.inheritTraits(actor.data.traits);
			actorData.hunger = asset.maxHunger / 2;
			actor.data.makeChild(BehaviourActionBase<City>.world.getCurWorldTime());
			if (actor2 != null)
			{
				actorData.inheritTraits(actor2.data.traits);
				actor2.data.makeChild(BehaviourActionBase<City>.world.getCurWorldTime());
			}
			Clan clan = CityBehProduceUnit.checkGreatClan(actor, actor2);
			actorData.skin = ActorTool.getBabyColor(actor, actor2);
			actorData.skin_set = actor.data.skin_set;
			Culture babyCulture = CityBehProduceUnit.getBabyCulture(actor, actor2);
			if (babyCulture != null)
			{
				actorData.culture = babyCulture.data.id;
				actorData.level = babyCulture.getBornLevel();
			}
			Actor pActor = pCity.spawnPopPoint(actorData, actor.currentTile); ;
			if (clan != null)
			{

				clan.addUnit(pActor);
			}
			int MPFromParent = (int)(actor.stats[mw_S.magic_power] * 0.2);

			// 使用TryGetValue优化字典访问
			if (!Main.Actor_Magic.ContainsKey(pActor))
			{
				pActor.MagicBegining();
			}
			int pResult2 = 0;
			actor.data.get(mw_S.magic_power, out int pResult, 0);
			actor2?.data.get(mw_S.magic_power, out pResult2, 0);
			int MP = (int)((pResult + pResult2) / 2 * 0.7);
			int newMP = MP + MPFromParent;

			// 添加限制条件避免数值过大
			pActor.data.change(mw_S.magic_power, Math.Min(newMP, (int)(pActor.stats[mw_S.magic_power] * 1.5))); // 限制最大增加至150%原MP
			__result = true;
			return false;
		}
		[HarmonyPostfix]
		[HarmonyPatch(typeof(WindowCreatureInfo), "OnEnable")]
		public static void OnEnable(WindowCreatureInfo __instance)
		{
			if (!Config.selectedUnit.Any())
			{
				return;
			}
			__instance.actor.data.get("currentMagicPower", out int CMP, 0);
			Debug.Log($"magic_power:{__instance.actor.stats[mw_S.magic_power]}  CMP:{CMP}");
			Main.Spirit = __instance.actor;

			return;
		}
		// [HarmonyPostfix]
		// [HarmonyPatch(typeof(Actor), "tryToAttack")]
		// public static void tryToAttack(Actor __instance, BaseSimObject pTarget, bool pDoChecks, ref bool __result)
		// {
		// 	if (!pTarget.a.Any())
		// 	{
		// 		return;
		// 	}
		// 	if (!__instance.Any())
		// 	{
		// 		return;
		// 	}

		// 	if (pDoChecks)
		// 	{
		// 		if (__instance.s_attackType == WeaponType.Melee && pTarget.zPosition.y > 0f)
		// 		{
		// 			return;
		// 		}
		// 		if (__instance.isInLiquid() && !__instance.asset.oceanCreature)
		// 		{
		// 			return;
		// 		}
		// 		if (!__instance.isAttackReady())
		// 		{
		// 			return;
		// 		}
		// 		if (!__instance.isInAttackRange(pTarget))
		// 		{
		// 			return;
		// 		}
		// 	}

		// 	return;
		// }
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "updateAge")]
		public static void updateAge(Actor __instance)
		{
			if (!__instance.Any()) { Main.Actor_Magic.Remove(__instance); return; }
			if (!__instance.asset.unit) { return; }
			__instance.data.get("ResearchStatus", out string RS, "idle");
			if (RS == "idle" && __instance.stats["intelligence"] > 5f && Toolbox.randomChance(0.3f))
			{
				__instance.StartResearch();
			}
			// __instance.data.get("IsMaster", out bool __result, false);
			// if (__instance.city.Any() && __result)
			// {
			// 	__instance.city.data.change("MagesNum", 1);
			// }
		}
		private static void UpdateMagicPower(Actor actor, float pElapsed)
		{
			actor.data.get("currentMagicPower", out int currentMagicPower, 0);
			actor.data.get("MP_time", out float mpTime, 0f);

			if (mpTime > 0f)
			{
				actor.data.set("MP_time", mpTime - pElapsed);
			}

			int maxMagicPower = (int)actor.stats[mw_S.magic_power];
			if (currentMagicPower < maxMagicPower && mpTime < pElapsed)
			{
				actor.data.set("MP_time", pElapsed + 5f);
				actor.data.set("currentMagicPower", currentMagicPower + 1);
			}
		}
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "updateParallelChecks")]
		public static void updateParallelChecks(Actor __instance, float pElapsed)
		{
			if (paused)
			{
				return;
			}
			if (!__instance.Any()) { Main.Actor_Magic.Remove(__instance); return; }
			if (!__instance.asset.unit) { return; }
			float world_time = (float)World.world.getCurWorldTime();
			UpdateMagicPower(__instance, world_time);
			// 研究状态更新
			__instance.data.get("ResearchStatus", out string researchStatus, "idle");
			__instance.data.get("ResearchTime", out float researchTime, 0f);

			if (researchStatus == "in progress" && researchTime < world_time)
			{
				if (__instance.stats["intelligence"] > 9f && __instance.stats[mw_S.magic_power] > 100 && Toolbox.randomChance(0.5f))
				{
					__instance.GetNewMagic();
					if (Toolbox.randomChance(0.3f))
					{
						__instance.GetMoreMP();
					}
				}
				else if (Toolbox.randomChance(0.5f))
				{
					__instance.GetMoreMP();
				}
				__instance.data.set("ResearchStatus", "idle");
			}

		}
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "newKillAction")]
		public static void newKillAction(Actor pDeadUnit, Kingdom pPrevKingdom)
		{
			if (Main.Actor_Magic.ContainsKey(pDeadUnit))
			{
				Main.Actor_Magic.Remove(pDeadUnit);
			}
		}
		// [HarmonyPrefix]
		// [HarmonyPatch(typeof(ActorBase), "nextJobActor")]
		// public static bool nextJobActor(ActorBase pActor, ref string __result)
		// {
		// 	if (!pActor.a.Any())
		// 	{
		// 		return false;
		// 	}
		// 	pActor.data.get("IsMaster", out bool falg, false);
		// 	if (falg)
		// 	{
		// 		__result = "Master";
		// 		return false;
		// 	}
		// 	return true;
		// }
		[HarmonyPrefix]
		[HarmonyPatch(typeof(ActorTool), "findNewBuildingTarget")]
		public static bool findNewBuildingTarget(Actor pActor, string pType, ref Building __result)
		{
			if (pType == "magic_tower")
			{
				if (pActor.city.hasBuildingType("magic_tower", true))
				{
					Building buildingType = pActor.city.getBuildingType("magic_tower", true, false);
					if (buildingType.currentTile.isSameIsland(pActor.currentTile))
					{
						ActorTool.possible_buildings.Add(buildingType);
					}
				}
			}
			else
			{
				return true;
			}

			if (ActorTool.possible_buildings.Count == 0)
			{
				__result = null;
				return false;
			}
			Building random = ActorTool.possible_buildings.GetRandom<Building>();
			__result = random;
			return false;
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(City), "getCitizenJob")]
		public static bool getCitizenJob(City __instance, Actor pActor)
		{
			if (!pActor.Any())
			{
				return false;
			}
			pActor.data.get("IsMaster", out bool falg, false);
			if (falg)
			{
				__instance.checkCitizenJob(AssetManager.citizen_job_library.get("Master"), __instance, pActor);
				return false;
			}
			return true;
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(Tornado), "tornadoAction")]
		public static bool tornadoAction(Actor pActor, WorldTile pTile = null)
		{
			Tornado_applyForce(pActor.currentTile, 10, 0.5f, false, false, 0, new string[] { pActor.kingdom.id }, pActor, null);
			float num = pActor.stats[S.scale];
			if (pActor.a.asset.flag_tornado)
			{
				num = pActor.a.actor_scale;
			}
			BrushData brushData = Brush.get((int)(num * 6f), "circ_");
			for (int i = 0; i < brushData.pos.Length; i++)
			{
				int num2 = pActor.currentTile.pos.x + brushData.pos[i].x;
				int num3 = pActor.currentTile.pos.y + brushData.pos[i].y;
				if (num2 >= 0 && num2 < MapBox.width && num3 >= 0 && num3 < MapBox.height)
				{
					WorldTile tileSimple = World.world.GetTileSimple(num2, num3);
					if (tileSimple.Type.ocean)
					{
						if (!pActor.asset.dieOnGround)
						{
							MapAction.removeLiquid(tileSimple);
						}
						if (Toolbox.randomChance(0.15f))
						{
							Tornado.spawnBurst(tileSimple, "rain", num);
						}
					}
					if (tileSimple.top_type != null || tileSimple.Type.life)
					{
						MapAction.decreaseTile(tileSimple, "flash");
					}
					if (tileSimple.Type.lava)
					{
						LavaHelper.removeLava(tileSimple);
						Tornado.spawnBurst(tileSimple, "lava", num);
					}
					if (tileSimple.building != null && tileSimple.building.asset.canBeDamagedByTornado)
					{
						tileSimple.building.getHit(1f, true, AttackType.Other, null, true, false);
					}
					if (tileSimple.isTemporaryFrozen())
					{
						tileSimple.unfreeze(10);
					}
					if (tileSimple.isOnFire())
					{
						tileSimple.stopFire();
					}
				}
			}
			return false;
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(BehTornadoGoToTileTarget), "execute")]
		public static bool execute(Actor pActor, ref BehResult __result)
		{
			if (pActor == null)
			{
				__result = BehResult.Stop;
				return false;
			}
			pActor.data.get("IsRotate", out bool pResult, false);
			if (!pResult)
			{
				return true;
			}
			int num;
			pActor.data.get("moveTimer", out num, 0);
			pActor.data.set("moveTimer", -(num + 4));

			// 定义绕圈的点（这里以当前位置为圆心）
			WorldTile centerTile = pActor.currentTile;
			if (centerTile == null)
			{
				__result = BehResult.Stop;
				return false;
			}
			// 计算围绕的轨迹点
			float radius = 5f; // 圆的半径，这里设置为5
			int numPoints = 3; // 增加轨迹点数量
			List<WorldTile> pathTiles = new();
			for (int i = 0; i < numPoints; i++)
			{
				float angle = 360f / numPoints * i;
				float circleX = centerTile.posV3.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
				float circleY = centerTile.posV3.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
				WorldTile circleTile = World.world.GetTile((int)circleX, (int)circleY);
				if (circleTile != null)
				{ pathTiles.Add(circleTile); }
			}
			// 移动到计算的轨迹点
			if (ActorMove.goToCurved(pActor, pathTiles.ToArray()) == ExecuteEvent.False)
			{
				__result = BehResult.Stop;
				return false;
			}

			__result = BehResult.Continue;
			return false;
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "getHit")]
		public static void getHit(Actor __instance, float pDamage, bool pFlash = true, AttackType pAttackType = AttackType.Other, BaseSimObject pAttacker = null, bool pSkipIfShake = true, bool pMetallicWeapon = false)
		{
			// 统一空引用检查
			if (!__instance.Any() || __instance.asset == null || pAttacker == null || pAttacker.a == null)
			{
				return;
			}

			// 跳过震动效果检查
			if (pSkipIfShake && __instance.shake_active)
			{
				return;
			}

			// 如果生命值小于等于0，直接返回
			if (__instance.data.health <= 0)
			{
				return;
			}
			WorldTile pTile = World.world.GetTile((int)__instance.currentPosition.x, (int)__instance.currentPosition.y);
			__instance.data.get("currentMagicPower", out int CMP, 0);
			if (CMP == 0 || !Main.Actor_Magic.ContainsKey(__instance)) { return; }
			int MaxMP = (int)__instance.stats[mw_S.magic_power];
			if (CMP > MaxMP * 0.6)
			{
				// 确保Main.Actor_Magic[__instance]有足够的元素可以进行访问
				if (Main.Actor_Magic[__instance].Count > 1)
				{
					for (int i = 1; i < Main.Actor_Magic[__instance].Count; i++)
					{
						var magicAction = Main.Actor_Magic[__instance][i];
						if (magicAction == null) continue; // 确保每个元素不为空

						float change = magicAction.stats[mw_S.change];
						if (magicAction.getHitAction == null || !Toolbox.randomChance(magicAction.stats[mw_S.change]))
						{
							continue;
						}
						magicAction.getHitAction(__instance, __instance.attackTarget, magicAction, pTile);
						Debug.Log(magicAction.id);
					}
				}
			}
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(Actor), "addStatusEffect")]
		public static bool addStatusEffect(Actor __instance, string pID, float pOverrideTimer = -1f)
		{
			if (!__instance.Any())
			{
				return false;
			}
			if (__instance.a.asset.unit && Main.Actor_Magic.ContainsKey(__instance.a))
			{
				List<magic> magicList = Main.Actor_Magic[__instance.a];
				magic magic1 = magicList.FirstOrDefault(m => m.id == "FireControl");
				if (magic1 != null && pID == "burning" && Toolbox.randomChance(magic1.stats[mw_S.change]))
				{
					return false;
				}
				magic1 = magicList.FirstOrDefault(m => m.id == "IceControl");
				if (magic1 != null && pID == "frozen" && Toolbox.randomChance(magic1.stats[mw_S.change]))
				{
					return false;
				}

			}

			return true;
		}
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "isInAttackRange")]
		public static void isInAttackRange(Actor __instance, BaseSimObject pObject)
		{
			if (!__instance.Any() && __instance.isInLiquid() && !__instance.asset.oceanCreature)
			{
				return;
			}
			BaseSimObject attackTarget = pObject;
			bool flag = Toolbox.DistVec3(__instance.currentPosition, pObject.currentPosition) < __instance.stats[S.range] + pObject.stats[S.size] + 20f;
			bool flag2 = __instance.isAttackReady();
			if (flag && flag2)
			{

				WorldTile pTile = World.world.GetTile((int)__instance.currentPosition.x, (int)__instance.currentPosition.y);
				__instance.data.get("currentMagicPower", out int CMP, 0);
				if (CMP == 0 || !Main.Actor_Magic.ContainsKey(__instance)) { return; }
				int MaxMP = (int)__instance.stats[mw_S.magic_power];
				if (CMP > MaxMP * 0.6)
				{
					// 确保Main.Actor_Magic[__instance]有足够的元素可以进行访问
					if (Main.Actor_Magic[__instance].Count > 1)
					{
						for (int i = 1; i < Main.Actor_Magic[__instance].Count; i++)
						{
							var magicAction = Main.Actor_Magic[__instance][i];
							if (magicAction == null) continue; // 确保每个元素不为空

							float change = magicAction.stats[mw_S.change];
							if (magicAction.attackAction == null || !Toolbox.randomChance(magicAction.stats[mw_S.change]))
							{
								continue;
							}
							magicAction.attackAction(__instance, __instance.attackTarget, magicAction, pTile);
							Debug.Log(magicAction.id);
						}
					}
				}
			}
		}
		// [HarmonyPrefix]
		// [HarmonyPatch(typeof(ActorBase), "goTo")]
		// public static bool goTo(ActorBase __instance, WorldTile pTile, ref ExecuteEvent __result, bool pPathOnWater = false, bool pWalkOnBlocks = false)
		// {
		// 	if (__instance.a.asset.unit && Main.Actor_Magic.ContainsKey(__instance.a)&&ActorMove.goTo((Actor)__instance, pTile, pPathOnWater, pWalkOnBlocks)==ExecuteEvent.True)
		// 	{
		// 		List<magic> magicList = Main.Actor_Magic[__instance.a];
		// 		if (magicList.FirstOrDefault(m => m.id == "delivery") != null)
		// 		{
		// 			// string text = __instance.asset.effect_teleport;
		// 			// if (string.IsNullOrEmpty(text))
		// 			// {
		// 			// 	text = "fx_teleport_blue";
		// 			// }
		// 			// EffectsLibrary.spawnAt(text, __instance.currentPosition, __instance.stats[S.scale]);
		// 			// BaseEffect baseEffect = EffectsLibrary.spawnAt(text, pTile.posV3, __instance.stats[S.scale]);
		// 			// if (baseEffect != null)
		// 			// {
		// 			// 	baseEffect.spriteAnimation.setFrameIndex(9);
		// 			// }
		// 			__instance.cancelAllBeh(null);
		// 			__instance.spawnOn(pTile, 0f);
		// 			__result=ExecuteEvent.True;
		// 			return false;
		// 		}

		// 	}

		// 	__instance.setTileTarget(pTile);
		// 	__result = ActorMove.goTo((Actor)__instance, pTile, pPathOnWater, pWalkOnBlocks);
		// 	return true;
		// }
		[HarmonyPrefix]
		[HarmonyPatch(typeof(Actor), "getHit")]
		public static bool GetHit(Actor __instance, float pDamage, bool pFlash = true, AttackType pAttackType = AttackType.Other, BaseSimObject pAttacker = null, bool pSkipIfShake = true, bool pMetallicWeapon = false)
		{
			__instance.attackedBy = null;
			if (!__instance.Any())
			{
				return false;
			}



			return true;
		}
		public static void Tornado_applyForce(WorldTile pTile, int pRad = 10, float pSpeedForce = 1.5f, bool pForceOut = true, bool useOnNature = false, int pDamage = 0, string[] pIgnoreKingdoms = null, BaseSimObject pByWho = null, TerraformOptions pOptions = null)
		{
			_force_temp_actor_list.Clear();
			Toolbox.fillListWithUnitsFromChunk(pTile.chunk, _force_temp_actor_list);
			for (int i = 0; i < pTile.chunk.neighboursAll.Count; i++)
			{
				Toolbox.fillListWithUnitsFromChunk(pTile.chunk.neighboursAll[i], _force_temp_actor_list);
			}
			if (pByWho != null && pByWho.isActor())
			{
				_force_temp_actor_list.Remove(pByWho.a);
			}
			float num = 1f;
			for (int j = 0; j < _force_temp_actor_list.Count; j++)
			{
				Actor actor = _force_temp_actor_list[j];
				if (pOptions == null || !actor.asset.very_high_flyer || pOptions.applies_to_high_flyers)
				{
					float num2 = Toolbox.DistVec2(actor.currentTile.pos, pTile.pos);
					bool flag = false;
					if ((useOnNature || !actor.race.nature) && num2 <= (float)pRad)
					{
						if (pIgnoreKingdoms != null)
						{

							foreach (string b in pIgnoreKingdoms)
							{
								Kingdom kingdom = actor.kingdom;
								if (((kingdom != null) ? kingdom.id : null) == b)
								{
									flag = true;
									// Debug.Log(actor.getName() + "pIgnoreKingdoms");
									return;
								}
							}
							if (flag)
							{
								goto IL_1FB;
							}
						}


					}
					if (actor.asset.unit && Main.Actor_Magic.ContainsKey(actor))
					{
						List<magic> magicList = Main.Actor_Magic[actor];
						if (magicList.FirstOrDefault(m => m.id == "WindControl") != null)
						{
							flag = true;
							// Debug.Log(actor.getName() + "WindControl");
							return;
						}
						if (flag)
						{
							goto IL_1FB;
						}
					}
					if (pByWho != null && pByWho.kingdom == actor.kingdom)
					{
						return;
					}
					if(flag)
					{
						Debug.Log("啊???");
						return;
					}
					// if (actor.asset.canBeHurtByPowers)
					// {
					// 	AttackType pType = AttackType.Other;
					// 	if (pOptions != null)
					// 	{
					// 		pType = pOptions.attackType;
					// 	}
					// 	actor.getHit((float)pDamage, true, pType, pByWho, true, false);
					// 	// Debug.Log(actor.getName() + "getHit");
					// }
					// float num3 = pSpeedForce - pSpeedForce * actor.stats[S.knockback_reduction];
					// if (num3 < 0f)
					// {
					// 	num3 = 0f;
					// }
					// if (num3 > 0f)
					// {
					// 	float angle = Toolbox.getAngle((float)actor.currentTile.x, (float)actor.currentTile.y, (float)pTile.x, (float)pTile.y);
					// 	float num4 = Mathf.Cos(angle) * num3 * num;
					// 	float num5 = Mathf.Sin(angle) * num3 * num;
					// 	if (pForceOut)
					// 	{
					// 		num4 *= -1f;
					// 		num5 *= -1f;
					// 	}
					// 	actor.addForce(num4, num5, num);
					// 	// Debug.Log(actor.getName() + "addForce");
					// }
				}
			}
		IL_1FB:Debug.Log("啊???");
		}
	}

}

