using HarmonyLib;
using NCMS.Utils;
using NeoModLoader.api;
using UnityEngine;
using System.Collections.Generic;
using ReflectionUtility;
using UnityEngine.UI;
using ai;
using NCMS;
using magic_world.Utils;
using UnityEngine.Events;
using magic_world.Code;
using System;
using System.IO;
using System.Data.Common;

namespace magic_world
{
	[ModEntry]
	class Main : BasicMod<Main>, ILocalizable
	{
		public static List<Actor> Heroic_Spirit = new();
		public static Actor Spirit = new();
		public static List<PowersTab> PVZPowersTabs = new List<PowersTab>();
		public static Dictionary<string, UnityAction<Actor, magic>> MWMagic = new();
		public static List<string> projectiles = new() { "arrow" };
		public static Dictionary<Actor, magic> Magic = new();
		public static Dictionary<string, magic> FireMagic = new();
		public static Dictionary<string, magic> IceMagic = new();
		public static Dictionary<string, magic> NatureMagic = new();
		public static Dictionary<string, magic> DarkMagic = new();
		public static Dictionary<string, magic> LightMagic = new();
		public static Dictionary<string, AttackAction> AttackMagic = new();
		public static Dictionary<Actor, List<magic>> Actor_Magic = new();
		public static Dictionary<Actor, List<magic>> Actor_Action_Magic = new();
		public static Dictionary<Actor, List<AttackAction>> Actor_AttackMagics = new();
		public static Dictionary<Actor, UnityAction<Actor, magic>> MagicAction = new();
		public static Dictionary<Building, List<Actor>> MagesGroup = new();
		public static List<Projectile> MWProjectile = new List<Projectile>();
		//本Mod由寒海特别赞助播出，请我们谢谢寒海！
		protected override void OnModLoad()
		{
			Dictionary<string, ScrollWindow> allWindows = (Dictionary<string, ScrollWindow>)Reflection.GetField(typeof(ScrollWindow), null, "allWindows");
			_ = Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
			allWindows["inspect_unit"].gameObject.SetActive(false);
			_ = Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "village");
			allWindows["village"].gameObject.SetActive(false);
			_ = Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "debug");
			allWindows["debug"].gameObject.SetActive(false);
			_ = Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "kingdom");
			allWindows["kingdom"].gameObject.SetActive(false);
			GameObject UIG = new GameObject("MW_UIG");
			var UIGRTF = UIG.AddComponent<RectTransform>();
			UIG.AddComponent<CanvasRenderer>();
			UIG.AddComponent<Image>().sprite = Resources.Load<Sprite>("ui/UIG");
			Windows.CreateNewWindow("MWHelper", "");
			GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/MWHelper/Background/Scroll View").SetActive(true);
			UIGRTF.sizeDelta = new Vector2(200, 30);
			translation();
			AddItem();
			mw_job.init();
			mw_effects.init();
			mw_projectile.init();
			mw_buildings.init();
			mw_traits.init();
			magicLibrary.init();
			pvz_ui.NewTab("magic_world", "ui/tab", 150f);
			mw_modder.init();
			mw_update_log.init();
			mw_magesWindow.init();
			mw_button.init();


			Harmony.CreateAndPatchAll(typeof(mw_harmony_actor));
			Harmony.CreateAndPatchAll(typeof(mw_harmony_city));
			Harmony.CreateAndPatchAll(typeof(mw_harmony_projectiles));
			Harmony.CreateAndPatchAll(typeof(Main));

			// NewUI.createBgWindowButtonLeft(
			// 	GameObject.Find($"Canvas Container Main/Canvas - Windows/windows/inspect_unit"),
			// 	posY: -20,
			// 	iconName: "default",
			// 	buttonName: "Join Yingling Hall",
			// 	buttonTitle: "Join Yingling Hall",
			// 	buttonDesc: "Join Yingling Hall",
			// 	call: AddSpirit
			// );
		}

		void Update()
		{
			if (Config.gameLoaded)
			{
				bool paused = World.world.isPaused();
				mw_update.update_magic(paused);
				mw_update.update_projectiles(paused);
			}
		}
		public void translation()
		{
			easyTranslate("cn", "magic_power", "魔力");
			easyTranslate("cz", "magic_power", "魔力");
			easyTranslate("en", "magic_power", "魔力");
			easyTranslate("cn", "currentMagicPower", "当前魔力");
			easyTranslate("cz", "currentMagicPower", "当前魔力");
			easyTranslate("en", "currentMagicPower", "当前魔力");
			easyTranslate("cz", mw_S.additional_damage, "额外伤害");
			easyTranslate("en", mw_S.additional_damage, "额外伤害");
			easyTranslate("en", mw_S.additional_damage, "额外伤害");
			easyTranslate("cn", mw_S.change, "释放概率");
			easyTranslate("cz", mw_S.change, "释放概率");
			easyTranslate("en", mw_S.change, "释放概率");
			easyTranslate("cz", mw_S.element_effect_extension, "元素效果延长");
			easyTranslate("en", mw_S.element_effect_extension, "元素效果延长");
			easyTranslate("cz", "tab_magic_world", "magic_world");
			easyTranslate("en", "tab_magic_world", "magic_world");
			easyTranslate("cz", "modder", "modder");
			easyTranslate("en", "modder", "modder");
			easyTranslate("cz", "mwbilibili", "bilibili");
			easyTranslate("en", "mwbilibili", "作者的bilibili账号");
			easyTranslate("cz", "mwqq", "加入qq群聊");
			easyTranslate("en", "mwqq", "加入qq群聊");
			easyTranslate("cz", "modderDescription", "modder");
			easyTranslate("en", "modderDescription", "modder");
			easyTranslate("cz", "tab_magic_world Description", "magic_world");
			easyTranslate("en", "tab_magic_world Description", "magic_world");
			easyTranslate("cz", "tab_magic_world Description2", "mod by 空星漫漫&洛德林");
			easyTranslate("en", "tab_magic_world Description2", "mod by 空星漫漫&洛德林");
			easyTranslate("cz", "mw_MContentText", "代码by空星漫漫\n\n贴图by洛德林\n\n特别感谢寒海这本Mod中提供的技术支持!!!\n\nqq群781471990");
			easyTranslate("en", "mw_MContentText", "代码by空星漫漫\n\n贴图by洛德林\n\n特别感谢寒海这本Mod中提供的技术支持!!!\n\nqq群781471990");
			ReadTranslationFromTextFile("MWUpdatelog1", $"Mods/{Mod.Info.Name}/更新日志.txt");
			for (int i = 2; i < 6; i++)
			{
				easyTranslate("cz", $"MWUpdatelog{i}", "别催了，在更（流泪）");
				easyTranslate("en", $"MWUpdatelog{i}", "别催了，在更（流泪）");
			}
		}
		public static void AddItem()
		{
			var grail = AssetManager.items.clone("grail", "ring");
			grail.id = "grail";
			grail.rarity = 100;
			grail.materials = List.Of<string>(new string[] { "gold" });
			grail.equipment_value = 9999;
			grail.mod_rank = 2;
			grail.quality = ItemQuality.Legendary;
			grail.base_stats[S.intelligence] = 10f;
			grail.base_stats[S.warfare] = 6;
			grail.base_stats[S.attack_speed] = 5;
			grail.base_stats[S.diplomacy] = 20;
			grail.name_templates = List.Of<string>(new string[]
			{
				"ring_name"
			});
			addItemSprite(grail.id, grail.materials[0]);
			AssetManager.items.add(grail);
		}
		public static void AddSpirit()
		{
			Spirit.data.set("Spirit", true);
			// Actor actor = World.world.units.createNewUnit(SA.ghost, Spirit.currentTile, 0f);
			Spirit.removeTrait("blessed");
			// ActorTool.copyUnitToOtherUnit(Spirit.a, actor);
			LogNewMessage(Spirit, "成为", Spirit, "英灵");
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(Actor), "killHimself")]
		public static void killHimself(Actor __instance, bool pDestroy = false, AttackType pType = AttackType.Other, bool pCountDeath = true, bool pLaunchCallbacks = true, bool pLogFavorite = true)
		{
			if (!__instance.isAlive() && !pDestroy)
			{
				return;
			}
			if (__instance.Any() && Spirit != null)
			{
				if (checkDamage(__instance.stats[S.damage]) != Rank.E)
				{
					Heroic_Spirit.Add(__instance);
					Debug.Log(checkDamage(__instance.stats[S.damage]).ToString());
				}
				Spirit.data.get("Spirit", out bool flag, false);
				if (flag) Heroic_Spirit.Add(__instance);
				Actor actor = World.world.units.createNewUnit(__instance.asset.id, Spirit.currentTile, 0f);
				actor.removeTrait("blessed");
				ActorTool.copyUnitToOtherUnit(Spirit.a, actor);
				LogNewMessage(Spirit, "召唤英灵", actor, "降临！");
				return;
			}
		}



		// [HarmonyPrefix]
		// [HarmonyPatch(typeof(BuildOrder), "getBuildingAsset")]
		// public static bool getBuildingAsset_Prefix(BuildingAsset __instance, ref BuildingAsset __result, City pCity, string pBuildingID = null)
		// {
		// 	if (string.IsNullOrEmpty(pBuildingID))
		// 	{
		// 		pBuildingID = __instance.id;
		// 	}
		// 	if (pCity.race.building_order_keys.ContainsKey(pBuildingID))
		// 	{
		// 		string pID = pCity.race.building_order_keys[pBuildingID];
		// 		__result = AssetManager.buildings.get(pID);
		// 		return true;
		// 	}
		// 	else
		// 	{
		// 		Race race = AssetManager.raceLibrary.get(S.human);
		// 		string pID = race.building_order_keys[pBuildingID];

		// 		__result = AssetManager.buildings.get(pID);
		// 		return false;
		// 	}
		// }

		public static void addItemSprite(string id, string material)
		{
			var dictItems = ActorAnimationLoader.dictItems;
			var sprite = Resources.Load<Sprite>("ui/Icons/items/w_" + id + "_" + material);
			dictItems.Add(sprite.name, sprite);
		}
		public static Rank checkDamage(float num)
		{
			if (num > 25) return Rank.D;
			else if (num > 50) return Rank.C;
			else if (num > 100) return Rank.B;
			else if (num > 200) return Rank.A;
			else if (num > 300) return Rank.CPlus;
			else if (num > 500) return Rank.BPlus;
			else if (num > 600) return Rank.CPlusPlus;
			else if (num > 5000) return Rank.EX;
			return Rank.E;
		}
		public static List<BaseSimObject> getObjectsInChunks(WorldTile pTile, int pRadius = 3, MapObjectType pObjectType = MapObjectType.All)
		{
			List<BaseSimObject> temp_map_objects = new List<BaseSimObject>();
			if (pTile == null)
			{
				return temp_map_objects;
			}
			temp_map_objects = checkChunk(temp_map_objects, pTile.chunk, pTile, pRadius, pObjectType);
			foreach (MapChunk MC in pTile.chunk.neighbours)
			{
				temp_map_objects = checkChunk(temp_map_objects, MC, pTile, pRadius, pObjectType);
			}
			return temp_map_objects;
		}
		public static List<BaseSimObject> checkChunk(List<BaseSimObject> temp_map_objects, MapChunk pChunk, WorldTile pTile, int pRadius, MapObjectType pObjectType = MapObjectType.All)
		{
			for (int i = 0; i < pChunk.k_list_objects.Count; i++)
			{
				Kingdom key = pChunk.k_list_objects[i];
				List<BaseSimObject> list = pChunk.k_dict_objects[key];
				for (int j = 0; j < list.Count; j++)
				{
					BaseSimObject baseSimObject = list[j];
					if (!(baseSimObject == null) && baseSimObject.isAlive() && !baseSimObject.object_destroyed)
					{
						if (baseSimObject.isActor())
						{
							if (pObjectType == MapObjectType.Building)
							{
								goto IL_84;
							}
						}
						else if (pObjectType == MapObjectType.Actor)
						{
							goto IL_84;
						}
						if (pRadius == 0 || Toolbox.DistTile(baseSimObject.currentTile, pTile) <= (float)pRadius)
						{
							temp_map_objects.Add(baseSimObject);
						}
					}
				IL_84:;
				}
			}
			return temp_map_objects;
		}
		public static void copyUnitToOtherUnit(Actor p1, Actor p2)
		{
			p1.spriteRenderer.enabled = false;
			p2.currentPosition = p1.currentPosition;
			p2.transform.position = p1.transform.position;
			p2.curAngle = p1.transform.localEulerAngles;
			p2.transform.localEulerAngles = p2.curAngle;
			p2.data.setName(p1.data.name);
			p2.data.created_time = p1.data.created_time;
			p2.data.age_overgrowth = p1.data.age_overgrowth;
			p2.data.kills = p1.data.kills;
			p2.data.children = p1.data.children;
			p2.data.favorite = p1.data.favorite;
			p2.takeItems(p1, p2.asset.take_items_ignore_range_weapons);
			for (int i = 0; i < p1.data.traits.Count; i++)
			{
				string text = p1.data.traits[i];
				if (!(text == "peaceful"))
				{
					p2.addTrait(text, false);
				}
			}
			p2.setStatsDirty();
			p2.setPosDirty();
			if (MoveCamera.inSpectatorMode() && MoveCamera.focusUnit == p1)
			{
				MoveCamera.focusUnit = p2;
			}
		}
		public static void easyTranslate(string pLanguage, string id, string name)
		{
			string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
			if (language != "en" && language != "cz")
			{
				language = "en";
			}
			if (pLanguage != language)
			{
				return;
			}
			Localization.addLocalization(id, name);
		}
		private static void ReadTranslationFromTextFile(string id, string filePath)
		{
			string text = "";
			using (StreamReader sr = new StreamReader(filePath))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					text += line + "\n";
				}
			}
			// Debug.Log("text:" + text);
			Localization.addLocalization(id, text);
		}
		public static void AddNewText(string message, Color color, Sprite icon = null)
		{
			GameObject gameObject = HistoryHud.instance.GetObject();
			gameObject.name = "HistoryItem " + (object)(HistoryHud.historyItems.Count + 1);
			gameObject.SetActive(true);

			gameObject.transform.Find("CText").GetComponent<Text>();
			gameObject.transform.SetParent(HistoryHud.contentGroup);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.localScale = Vector3.one;
			component.localPosition = Vector3.zero;
			component.SetLeft(0.0f);

			float top = (float)HistoryHud.instance.CallMethod("recalcPositions");

			component.SetTop(top);
			component.sizeDelta = new Vector2(component.sizeDelta.x, 15f);
			gameObject.GetComponent<HistoryHudItem>().targetBottom = top;

			gameObject.GetComponent<HistoryHudItem>().textField.color = color;
			gameObject.GetComponent<HistoryHudItem>().textField.text = message;
			HistoryHud.historyItems.Add(gameObject.GetComponent<HistoryHudItem>());
			HistoryHud.instance.recalc = true;

			if (icon != null)
			{
				gameObject.transform.Find("Icon").GetComponent<Image>().sprite = icon;
			}

			gameObject.SetActive(true);
		}

		public static void LogNewMessage(Kingdom pKingdom, string pMessage = "", string pMessage2 = "")
		{
			var kingdomColor = pKingdom.kingdomColor.getColorText();
			var Message = string.Concat(new string[]
			{
				pMessage,
				"  <color=",
				Toolbox.colorToHex(kingdomColor, true),
				">",
				pKingdom.name,
				"</color>  ",
				pMessage2
			});
			AddNewText(Message, Toolbox.color_log_neutral);
		}

		public static void LogNewMessage(Actor pActor, string pMessage, Actor pActor2, string pMessage2 = "")
		{
			var kingdomColor = pActor.kingdom.kingdomColor.getColorText();
			var Message = string.Concat(new string[]
			{
				"  <color=",
				Toolbox.colorToHex(kingdomColor, true),
				">",
				pActor.name,
				"</color>  ",
				pMessage,
				pActor2.name,
				pMessage2
			});
			AddNewText(Message, Toolbox.color_log_neutral);
		}
	}
}