using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using magic_world.Utils;

namespace magic_world
{
    class mw_harmony_projectiles
	{
		public static List<Projectile> CattailStab = new List<Projectile>();
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EffectsLibrary), "spawnProjectile")]
		public static bool spawnProjectile(ref string pAssetID)
		{
			if (string.IsNullOrEmpty(pAssetID)) { pAssetID = "arrow"; }
			return true;
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(Projectile), "start")]
		public static bool start_Prefix_Projectile(Projectile __instance, Vector3 pStart, Vector3 pTarget, string pAssetID, float pTargetZ = 0f)
		{
			if (!__instance.created) { __instance.create(); }
			__instance.asset = AssetManager.projectiles.get(pAssetID);
			if (__instance.asset == null) { __instance.destroy(); return false; }
			return true;
		}
		
		
		// [HarmonyPrefix]
		// [HarmonyPatch(typeof(Projectile), "checkHit")]
		// public static bool checkHit_Prefix(Projectile __instance, Vector3 pHitVector)
		// {
		// 	if (__instance == null)
		// 	{
		// 		return false;
		// 	}
		// 	if (__instance.byWho != null && __instance.byWho.isAlive())
		// 	{
		// 		if (__instance.asset.parabolic)
		// 		{
		// 			Vector3 pos1 = __instance.m_transform.position;
		// 			WorldTile tile = World.world.GetTile((int)pHitVector.x, (int)pHitVector.y);
		// 			if (tile != null && Main.UmbrellaLeafTile.ContainsKey(tile))
		// 			{
		// 				Actor NGA = Main.UmbrellaLeafTile[tile];
		// 				if (NGA.Any() && __instance.byWho.kingdom != null && NGA.kingdom != null && NGA.kingdom.isEnemy(__instance.byWho.kingdom))
		// 				{
		// 					Main.NewAction("UmbrellaLeafProtect", NGA, 0.2f);
		// 					float angle = Toolbox.randomFloat(3.141593f, -3.141593f);
		// 					__instance.start(pos1, pos1 + new Vector3(Toolbox.randomFloat(10f, 20f) * Mathf.Cos(angle), Toolbox.randomFloat(10f, 20f) * Mathf.Sin(angle)), __instance.asset.id);
		// 					return false;
		// 				}
		// 			}
		// 		}
		// 		AttackData pData = new AttackData(__instance.byWho, World.world.GetTile((int)__instance.currentPosition.x,
		// 		(int)__instance.currentPosition.y), pHitVector, __instance.targetObject, AttackType.Weapon, false, false, false);
		// 		if (Main.AttackData_Projectile.ContainsKey(pData))
		// 		{
		// 			Main.AttackData_Projectile[pData] = __instance;
		// 		}
		// 		else { Main.AttackData_Projectile.Add(pData, __instance); }
		// 	}
		// 	return true;
		// }
		// [HarmonyPostfix]
		// [HarmonyPatch(typeof(Projectile), "checkHit")]
		// public static void checkHit_Postfix(Projectile __instance, Vector3 pHitVector)
		// {
		// 	if (__instance.byWho != null && __instance.byWho.isAlive())
		// 	{
		// 		AttackData pData = new AttackData(__instance.byWho, World.world.GetTile((int)__instance.currentPosition.x,
		// 		(int)__instance.currentPosition.y), pHitVector, __instance.targetObject, AttackType.Weapon, false, false, false);
		// 		if (Main.AttackData_Projectile.ContainsKey(pData)) { Main.AttackData_Projectile.Remove(pData); }
		// 	}
		// }
		[HarmonyPostfix]
		[HarmonyPatch(typeof(Projectile), "start")]
		public static void start_Postfix_Projectile(Projectile __instance)
		{
			if (__instance != null && !Main.MWProjectile.Contains(__instance)&&Main.projectiles.Contains(__instance.asset.id))
			{
				Main.MWProjectile.Add(__instance);
			}
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(Projectile), "update")]
		public static bool update_Projectile_Prefix(Projectile __instance, float pElapsed)
		{
			if (__instance == null) { return false; }
			if (__instance.asset == null)
			{
				__instance.destroy();
				return false;
			}
			
			if (CattailStab.Contains(__instance)  && __instance.gameObject.activeSelf)
			{
				if (__instance.controller.asset.draw_light_area)
				{
					Vector2 position = __instance.transform.position;
					position.x += __instance.controller.asset.draw_light_area_offset_x;
					position.y += __instance.controller.asset.draw_light_area_offset_y;
					World.world.stackEffects.light_blobs.Add(new LightBlobData
					{
						position = position,
						radius = __instance.controller.asset.draw_light_size
					});
				}
				if (!World.world._isPaused && DebugConfig.isOn(DebugOption.PauseEffects) && __instance.attachedToActor != null)
				{
					__instance.updateAttached();
				}
				if (__instance.spriteAnimation != null)
				{
					__instance.spriteAnimation.update(pElapsed);
				}
				if (__instance.callbackOnFrame != -1 && __instance.spriteAnimation.currentFrameIndex == __instance.callbackOnFrame)
				{
					__instance.callback();
					__instance.clear();
				}
				if (__instance.curScale < __instance.targetScale)
				{
					__instance.curScale += pElapsed * 0.2f;
					if (__instance.curScale > __instance.targetScale)
					{
						__instance.curScale = __instance.targetScale;
					}
					__instance.m_transform.localScale = new Vector3(__instance.curScale, __instance.curScale);
				}
				if (__instance.asset.draw_light_area)
				{
					Vector2 position2 = __instance.m_transform.position;
					position2.x += __instance.asset.draw_light_area_offset_x;
					position2.y += __instance.asset.draw_light_area_offset_y;
					World.world.stackEffects.light_blobs.Add(new LightBlobData
					{
						position = position2,
						radius = __instance.asset.draw_light_size
					});
				}
				return false;
			}
			else if (__instance.byWho != null && __instance.byWho.isActor() &&__instance.IsID("photosphere"))
			{
				CattailStab.Add(__instance);
				return false;
			}
			// if (__instance.asset.parabolic && __instance.byWho != null && __instance.byWho.isAlive())
			// {
			// 	Vector3 pos1 = __instance.m_transform.position;
			// 	WorldTile tile = World.world.GetTile((int)pos1.x, (int)pos1.y);
			// 	if (tile != null && Main.UmbrellaLeafTile.ContainsKey(tile))
			// 	{
			// 		Actor NGA = Main.UmbrellaLeafTile[tile];
			// 		if (NGA.Any() && __instance.byWho.kingdom != null && NGA.kingdom != null && NGA.kingdom.isEnemy(__instance.byWho.kingdom))
			// 		{
			// 			Main.NewAction("UmbrellaLeafProtect", NGA, 0.2f);
			// 			float angle = Toolbox.randomFloat(3.141593f, -3.141593f);
			// 			__instance.start(pos1, pos1 + new Vector3(Toolbox.randomFloat(10f, 20f) * Mathf.Cos(angle), Toolbox.randomFloat(10f, 20f) * Mathf.Sin(angle)), __instance.asset.id);
			// 			return false;
			// 		}
			// 	}
			// }
			return true;
		}
		
	// 	[HarmonyPrefix]
	// 	[HarmonyPatch(typeof(CombatActionLibrary), "attackRangeAction")]
	// 	public static bool attackRangeAction(AttackData pData, CombatActionLibrary __instance)
	// 	{
	// 		Actor a = pData.initiator.a;
	// 		BaseSimObject target = pData.target;
	// 		Vector3 pos1 = a.currentPosition;
	// 		Vector3 pos2 = target.currentPosition;
	// 		string projectile = pData.initiator.a.getWeaponAsset().projectile;
	// 		if (projectile == "niblet")
	// 		{
	// 			if (Toolbox.randomChance(0.25f) || pvz_laws.toggleBools["Butterpult"])
	// 			{
	// 				bool maize2 = false;
	// 				float maizeGL = 0.004f;
	// 				projectile = "butter";
	// 				if (pvz_laws.toggleBools["Butterpult"])
	// 				{
	// 					maizeGL = 0.01f;
	// 				}
	// 				if (Toolbox.randomChance(maizeGL))
	// 				{
	// 					maize2 = true;
	// 					projectile = "maize2";
	// 				}
	// 				foreach (Projectile pro in a.NewProjectile(target, Main.None, Main.None, projectile))
	// 				{
	// 					if (pro != null)
	// 					{
	// 						if (maize2)
	// 						{
	// 							pro.stats[S.damage] = a.stats[S.damage] * 240;
	// 						}
	// 						else
	// 						{
	// 							pro.stats[S.damage] = a.stats[S.damage] * 2;
	// 						}
	// 					}
	// 				}
	// 				return false;
	// 			}
	// 		}
	// 		bool flag1 = a.hasAction("Attack") && a.IsID("Gatlingpea");
	// 		bool flag2 = a.hasAction("FireGatling_PlantFood");
	// 		if (a.IsID("Threepeater") || flag1 || flag2 || a.hasAction("SnowGatling_PlantFood"))
	// 		{
	// 			string pid = "null";
	// 			float damage = 0f;
	// 			if (flag1)
	// 			{
	// 				if (Toolbox.randomChance(0.1f))
	// 				{
	// 					pid = "firepea";
	// 					damage += a.stats[S.damage];
	// 				}
	// 				else if (Toolbox.randomChance(0.05f))
	// 				{
	// 					pid = "firepea2";
	// 					damage += a.stats[S.damage] * 2f;
	// 				}
	// 			}
	// 			if (flag2)
	// 			{
	// 				if (Toolbox.randomChance(0.1f))
	// 				{
	// 					pid = "firepea2";
	// 					damage += a.stats[S.damage] / 2f;
	// 				}
	// 				else if (Toolbox.randomChance(0.05f))
	// 				{
	// 					pid = "firepea3";
	// 					damage += a.stats[S.damage];
	// 				}
	// 			}
	// 			foreach (Projectile pro in a.NewProjectile(target, Main.None, Main.None, pid))
	// 			{
	// 				if (pro != null)
	// 				{
	// 					pro.stats[S.damage] += damage;
	// 					Vector3 pos3 = pro.vecTarget;
	// 					float pAngle = Toolbox.getAngle(pos1.x, pos1.y, pos2.x, pos2.y);
	// 					bool XorY = (pAngle >= -3.141593f && pAngle <= -2.356194f || pAngle >= 2.356194f && pAngle <= 3.141593f)
	// 					|| (pAngle >= -0.7853982f && pAngle <= 0f || pAngle >= 0f && pAngle <= 0.7853982f);
	// 					float random1 = Toolbox.randomFloat(1f, 2f);
	// 					float random2 = Toolbox.randomFloat(1f, 2f);
	// 					pvz_action pf = a.getAction("Threepeater_PlantFood");
	// 					if (pf != null)
	// 					{
	// 						float num = pf.stats[S.range];
	// 						random1 = num;
	// 						random2 = num;
	// 					}
	// 					Vector3 vector1 = new Vector3(pos3.x + random1, pos3.y);
	// 					Vector3 vector2 = new Vector3(pos3.x - random2, pos3.y);
	// 					if (XorY)
	// 					{
	// 						vector1 = new Vector3(pos3.x, pos3.y + random1);
	// 						vector2 = new Vector3(pos3.x, pos3.y - random2);
	// 					}
	// 					if (a.HXPlant() && pf == null)
	// 					{
	// 						WorldTile tile1 = World.world.GetTile((int)pos1.x, (int)pos1.y);
	// 						WorldTile tile2 = World.world.GetTile((int)pos2.x, (int)pos2.y);
	// 						if (tile1 != null && tile2 != null)
	// 						{
	// 							if (tile1.tile_down != null && tile1.tile_down.y == tile2.y)
	// 							{
	// 								if (XorY)
	// 								{
	// 									vector1 = new Vector3(pos3.x, pos3.y + random1);
	// 									vector2 = new Vector3(pos3.x, pos3.y + random2 * 2);
	// 								}
	// 								else
	// 								{
	// 									vector1 = new Vector3(pos3.x + random1, pos3.y);
	// 									vector2 = new Vector3(pos3.x + random2 * 2, pos3.y);
	// 								}
	// 							}
	// 							if (tile1.tile_up != null && tile1.tile_up.y == tile2.y)
	// 							{
	// 								if (XorY)
	// 								{
	// 									vector1 = new Vector3(pos3.x, pos3.y - random1);
	// 									vector2 = new Vector3(pos3.x, pos3.y - random2 * 2);
	// 								}
	// 								else
	// 								{
	// 									vector1 = new Vector3(pos3.x - random1, pos3.y);
	// 									vector2 = new Vector3(pos3.x - random2 * 2, pos3.y);
	// 								}
	// 							}
	// 						}
	// 					}
	// 					if (flag1)
	// 					{
	// 						if (Toolbox.randomChance(0.1f))
	// 						{
	// 							pid = "firepea";
	// 							damage += a.stats[S.damage];
	// 						}
	// 						else if (Toolbox.randomChance(0.05f))
	// 						{
	// 							pid = "firepea2";
	// 							damage += a.stats[S.damage] * 2;
	// 						}
	// 					}
	// 					if (flag2)
	// 					{
	// 						if (Toolbox.randomChance(0.1f))
	// 						{
	// 							pid = "firepea2";
	// 							damage += a.stats[S.damage] / 2f;
	// 						}
	// 						else if (Toolbox.randomChance(0.05f))
	// 						{
	// 							pid = "firepea3";
	// 							damage += a.stats[S.damage];
	// 						}
	// 					}
	// 					foreach (Projectile pro2 in a.NewProjectile(target, Main.None, vector1, pid))
	// 					{
	// 						if (pro2 != null)
	// 						{
	// 							pro2.stats[S.damage] += damage;
	// 						}
	// 					}
	// 					if (flag1)
	// 					{
	// 						if (Toolbox.randomChance(0.1f))
	// 						{
	// 							pid = "firepea";
	// 							damage += a.stats[S.damage];
	// 						}
	// 						else if (Toolbox.randomChance(0.05f))
	// 						{
	// 							pid = "firepea2";
	// 							damage += a.stats[S.damage] * 2;
	// 						}
	// 					}
	// 					if (flag2)
	// 					{
	// 						if (Toolbox.randomChance(0.1f))
	// 						{
	// 							pid = "firepea2";
	// 							damage += a.stats[S.damage] / 2f;
	// 						}
	// 						else if (Toolbox.randomChance(0.05f))
	// 						{
	// 							pid = "firepea3";
	// 							damage += a.stats[S.damage];
	// 						}
	// 					}
	// 					foreach (Projectile pro2 in a.NewProjectile(target, Main.None, vector2, pid))
	// 					{
	// 						if (pro2 != null)
	// 						{
	// 							pro2.stats[S.damage] += damage;
	// 						}
	// 					}
	// 				}
	// 			}
	// 			return false;
	// 		}
	// 		if (a.IsID("SplitPea"))
	// 		{
	// 			foreach (Projectile pro in a.NewProjectile(target, Main.None, Main.None))
	// 			{
	// 				if (pro != null)
	// 				{
	// 					for (int i = 0; i < 2; i++)
	// 					{
	// 						Vector3 pos3 = pro.vecTarget;
	// 						Vector3 vector1 = pos3.AddAngle(pos1, 180f);
	// 						vector1.x += Toolbox.randomFloat(-(target.stats[S.size] + 1f), target.stats[S.size] + 1f);
	// 						vector1.y += Toolbox.randomFloat(-target.stats[S.size], target.stats[S.size]);
	// 						a.NewProjectile(target, Main.None, vector1);
	// 					}
	// 				}
	// 			}
	// 			return false;
	// 		}
	// 		if (a.IsID("MagnetShroom"))
	// 		{
	// 			foreach (Projectile pro in a.NewProjectile(target, Main.None, Main.None))
	// 			{
	// 				if (pro != null)
	// 				{
	// 					Vector3 pos3 = pro.vecTarget;
	// 					pro.vecTarget = pro.vecStart;
	// 					pro.vecStart = pos3;
	// 					pro.byWho = null;
	// 				}
	// 			}
	// 			return false;
	// 		}
	// 		if (a.IsID("Starfruit"))
	// 		{
	// 			float angle = 0f;
	// 			bool flag = a.hasAction("Attack");
	// 			foreach (Projectile pro in a.NewProjectile(target, Main.None, Main.None, "null", 5))
	// 			{
	// 				if (pro != null)
	// 				{
	// 					pro.vecTarget = pro.vecTarget.AddAngle(pos1, angle);
	// 					pro.vecStart = pro.vecStart.AddAngle(pos1, angle);
	// 					angle += 72f;
	// 					if (flag)
	// 					{
	// 						pro.curScale *= 1.8f;
	// 						pro.targetScale *= 1.8f;
	// 						pro.m_transform.localScale = new Vector3(pro.curScale, pro.curScale);
	// 					}
	// 				}
	// 			}
	// 			return false;
	// 		}
	// 		if (a.IsID("Rafflesia") && a.hasAction("Attack"))
	// 		{
	// 			foreach (Projectile pro in a.NewProjectile(target, Main.None, Main.None))
	// 			{
	// 				if (pro != null)
	// 				{
	// 					pro.speed *= 10f;
	// 					pro.timeToTarget = (Toolbox.DistVec3(pro.vecStart, pro.vecTarget) + pro.targetZ) / pro.speed;
	// 				}
	// 			}
	// 			return false;
	// 		}
	// 		if (a.IsID("ScaredyShroom") && a.hasAction("ScaredyShroom_PlantFood"))
	// 		{
	// 			foreach (Projectile pro in a.NewProjectile(target, Main.None, Main.None))
	// 			{
	// 				if (pro != null)
	// 				{
	// 					float size = Mathf.Sqrt(a.stats[S.damage] / 30f) / 40f;
	// 					if (size > 0.075f)
	// 					{
	// 						size = 0.075f;
	// 					}
	// 					pro.curScale = size;
	// 					pro.targetScale = size;
	// 					pro.m_transform.localScale = new Vector3(size, size);
	// 				}
	// 			}
	// 			return false;
	// 		}
	// 		if (a.IsPlant() && pvz_laws.toggleBools["RandomProjectile"])
	// 		{
	// 			a.NewProjectile(target, Main.None, Main.None);
	// 			return false;
	// 		}
	// 		return true;
	// 	}
	}
}