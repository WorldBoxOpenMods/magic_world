using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using magic_world;
using System;
using UnityEngine.Events;
using ai;
using magic_world.Code;


namespace magic_world.Utils
{
    //感谢寒海提供的技术支持
    public static class MWTools
    {
        public static bool HasMagic(this Actor a, string id)
        {
            if (!a.Any()) return false;
            if (a.asset.unit && Main.Actor_Magic.ContainsKey(a))
            {
                List<magic> magicList = Main.Actor_Magic[a];
                magic magic1 = magicList.FirstOrDefault(m => m.id == id);
                if (magic1 != null)
                {
                    return true;
                }
            }
            return false;
        }
        public static void MagicBegining(this Actor a)
        {
            if (!a.Any()) return;

            // List<magic> magicList = new List<magic>();
            // magic magic = new magic();

            if (Toolbox.randomChance(0.1f))
            {
                a.data.intelligence += Toolbox.randomInt(0, 3);
            }

            if (Toolbox.randomChance(0.5f))
            {
                int minPower = (int)(10 + a.stats["intelligence"] * 5);
                int maxPower = (int)(120 + a.stats["intelligence"] * 5);
                a.data.change(mw_S.magic_power, Toolbox.randomInt(minPower, maxPower));
            }

            else if (Toolbox.randomChance(0.1f))
            {
                int minPower = (int)(15 + a.stats["intelligence"] * 7);
                int maxPower = (int)(125 + a.stats["intelligence"] * 7);
                a.data.change(mw_S.magic_power, Toolbox.randomInt(minPower, maxPower));
            }
            a.data.set("element", (int)GetElements(a));
            // magic.element = GetElements(a);
            // magic.a = a;
            // magicList.Add(magic);

            // Main.Actor_Magic.TryAdd(a, magicList);
        }
        private static readonly element[] AllElements = Enum.GetValues(typeof(element)) as element[];

        public static element GetElements(Actor a)
        {
            System.Random random = new();
            element selectedElement = AllElements[random.Next(0, AllElements.Length)];

            switch (a.asset.race)
            {
                case "orc":
                    selectedElement = GetElementByChance(new Dictionary<element, double> { { element.nature, 0.4 } }, selectedElement);
                    break;
                case "human":
                    selectedElement = GetElementByChance(new Dictionary<element, double> { { element.fire, 0.5 }, { element.nature, 0.4 }, { element.ice, 0.2 } }, selectedElement);
                    break;
                case "elf":
                    selectedElement = GetElementByChance(new Dictionary<element, double> { { element.nature, 0.5 }, { element.ice, 0.3 } }, selectedElement);
                    break;
                case "dwarf":
                    selectedElement = GetElementByChance(new Dictionary<element, double> { { element.nature, 0.3 }, { element.fire, 0.1 } }, selectedElement);
                    break;
            }
            if (selectedElement == element.nature)
            {
                a.data.set("camp", SK.nature);
            }
            return selectedElement;
        }

        private static element GetElementByChance(Dictionary<element, double> chances, element defaultElement)
        {
            double roll = new System.Random().NextDouble();
            double cumulative = 0.0;
            foreach (var chance in chances)
            {
                cumulative += chance.Value;
                if (roll < cumulative)
                { return chance.Key; }
            }
            return defaultElement;
        }
        public static void StartResearch(this Actor a)
        {
            if (!a.Any()) return;

            float world_time = (float)World.world.getCurWorldTime();
            a.data.set("ResearchTime", (float)(world_time + 180f)); // 将研究时间调整为180秒
            a.data.set("ResearchStatus", "in progress");
            // Debug.Log("开始研究");
        }
        public static void GetMoreMP(this Actor a)
        {
            a.data.get(mw_S.magic_power, out int num, 0);
            if (Toolbox.randomChance(0.6f) && a.stats[S.intelligence] > 3f)
            {
                a.data.set(mw_S.magic_power, num + Toolbox.randomInt(1, 3));
            }
            else if (Toolbox.randomChance(0.45f) && a.stats[S.intelligence] > 10f)
            {
                a.data.set(mw_S.magic_power, num + Toolbox.randomInt(3, 5));
            }
            else if (Toolbox.randomChance(0.45f) && a.stats[S.intelligence] > 20f)
            {
                a.data.set(mw_S.magic_power, num + Toolbox.randomInt(7, 15));
            }

            if (Toolbox.randomChance(0.1f))
            {
                a.data.intelligence += Toolbox.randomInt(0, 2);
            }
            else if (Toolbox.randomChance(0.1f))
            {
                a.data.intelligence += Toolbox.randomInt(1, 5);
            }
        }
        public static void GetNewMagic(this Actor a)
        {
            if (!a.Any()) return;
            a.data.favorite = true;
            a.data.set("ResearchStatus", "idle");
            if (a.stats["intelligence"] > 20f && Toolbox.randomChance(0.1f))
            {
                a.StartResearch();
            }
            a.data.set("IsMaster", true);
            System.Random random = new();
            Dictionary<element, List<KeyValuePair<string, magic>>> magicDictionary = new Dictionary<element, List<KeyValuePair<string, magic>>>
            {
                { element.fire, Main.FireMagic.ToList() },
                { element.nature, Main.NatureMagic.ToList() },
                { element.ice, Main.IceMagic.ToList() },
                {element.dark,Main.DarkMagic.ToList()},
                { element.light, Main.IceMagic.ToList() }
            };
            a.data.get("element", out int Element, 0);
            element currentElement = (element)Element;
            List<KeyValuePair<string, magic>> magicList = magicDictionary[currentElement];
            KeyValuePair<string, magic> randomElement = magicList[random.Next(magicList.Count)];
            magic value = randomElement.Value;
            if (!Main.Actor_Magic.ContainsKey(a))
            {
                List<magic>list=new();
                Main.Actor_Magic.TryAdd(a, list);
            }
            else if (Main.Actor_Magic[a].Any(m => m.id == value.id))
            {
                magic Magic = Main.Actor_Magic[a].First(m => m.id == value.id);
                NewEntry(Magic);
            }
            else
            {
                value.a = a;
                Main.Actor_Magic[a].Add(value);
            }
            Debug.Log($"{a.getName()} GetNewMagic!");
            // else if(Main.MagicAction.ContainsKey(a))
            // {
            //     // List<magic> magicList = Main.Actor_Magic[a];
            //     // magic magic = new();
            //     Main.MagicAction.Add(a, delegate (Actor actor, magic action)
            //     {

            //     });
            // }
        }
        public static void NewEntry(magic Magic)
        {
            #region 添加词条
            if (Magic.projectile)
            {
                if (Toolbox.randomChance(0.07f))
                {
                    Magic.stats[mw_S.incidence] += Toolbox.randomInt(1, 3);
                }
                else if (Toolbox.randomChance(0.08f))
                {
                    Magic.stats[mw_S.projectiles] += Toolbox.randomInt(1, 2);
                }
            }
            if (Magic.type == magic_type.element_effect)
            {
                if (Toolbox.randomChance(0.08f))
                {
                    Magic.stats[mw_S.element_effect_extension] += Toolbox.randomFloat(2f, 5f);
                }
                else if (Toolbox.randomChance(0.1f))
                {
                    Magic.stats[mw_S.additional_damage] += Toolbox.randomFloat(2f, 3f);
                }

            }
            else if (Magic.type == magic_type.element_effect)
            {
                if (Toolbox.randomChance(0.08f))
                {
                    Magic.stats[mw_S.element_effect_extension] += Toolbox.randomFloat(2f, 5f);
                }
                else if (Toolbox.randomChance(0.1f))
                {
                    Magic.stats[mw_S.additional_damage] += Toolbox.randomFloat(2f, 3f);
                }

            }
            else if (Magic.type == magic_type.controlElements)
            {
                if (Toolbox.randomChance(0.1f))
                {
                    Magic.stats[mw_S.change] += Toolbox.randomFloat((float)0.01, (float)0.04);
                }
            }
            if (Magic.Enmeybuff)
            {
                if (Toolbox.randomChance(0.04f))
                {
                    Magic.stats[mw_S.target] += Toolbox.randomInt(1, 2);
                }
                if (Toolbox.randomChance(0.04f))
                {
                    Magic.stats[mw_S.additional_damage] += Toolbox.randomFloat(3f, 10f);
                }
            }
            if (Magic.Selfbuff)
            {
                if (Toolbox.randomChance(0.04f))
                {
                    Magic.stats[mw_S.target] += Toolbox.randomInt(1, 2);
                }
                if (Toolbox.randomChance(0.04f))
                {
                    Magic.stats[mw_S.additional_damage] += Toolbox.randomFloat(3f, 10f);
                }
            }
            #endregion
        }
        public static bool checkCMP(Actor a, int num)
        {
            a.data.get("currentMagicPower", out int currentMagicPower, 0);
            if (currentMagicPower <= 0)
            {
                return false;
            }
            a.data.set("currentMagicPower", num);
            return true;
        }
        public static void copyUnitToOtherUnit(Actor p1, Actor p2)
        {
            p1.spriteRenderer.enabled = false;
            p2.currentPosition = p1.currentPosition;
            p2.transform.position = p1.transform.position;
            p2.curAngle = p1.transform.localEulerAngles;
            p2.transform.localEulerAngles = p2.curAngle;
            p2.data.setName(p1.getName() + "的暗影分身");
            p2.data.age_overgrowth = p1.data.age_overgrowth;
            p2.data.favorite = p1.data.favorite;
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
        public static List<WorldTile> GetSurroundingTiles(ActorTileTarget pTarget, MapChunk pChunk, int findNum)
        {
            List<WorldTile> result = new();

            List<MapChunk> listPool = new(pChunk.neighboursAll.Count + 1)
                {
                    pChunk
                };
            listPool.AddRange(pChunk.neighboursAll);

            foreach (MapChunk mapChunk in listPool)
            {
                foreach (WorldTile tTile in mapChunk.tiles)
                {
                    switch (pTarget)
                    {
                        case ActorTileTarget.RandomTNT:
                            if (tTile.Type.explodable)
                            {
                                result.Add(tTile);
                            }
                            break;
                        case ActorTileTarget.RandomBurnableTile:
                            if (tTile.Type.burnable)
                            {
                                result.Add(tTile);
                            }
                            break;
                        case ActorTileTarget.RandomTileWithUnits:
                            // Assuming tTile.doUnits() checks and adds tiles with units
                            tTile.doUnits(delegate (Actor pActor)
                            {
                                result.Add(tTile);
                            });
                            break;
                        case ActorTileTarget.RandomTileWithCivStructures:
                            if (tTile.building != null && tTile.building.city != null)
                            {
                                result.Add(tTile);
                            }
                            if (tTile.Type.burnable && tTile.zone.city != null)
                            {
                                result.Add(tTile);
                            }
                            break;
                    }

                    // 如果已经找到3个符合条件的 WorldTile，则返回结果
                    if (result.Count >= findNum)
                    {
                        return result;
                    }
                }
            }

            // 如果没有找到符合条件的 WorldTile，则返回空列表
            return result;
        }
        public static Actor FindTeather(this BaseSimObject obj, List<Actor> actors = null)
        {
            if (obj == null) { return null; }
            if (!obj.isAlive()) { return null; }
            float num = obj.a.stats[mw_S.magic_power];
            Actor actor = null;
            if (actors == null)
            {
                return null;
            }
            var actors2 = actors.OrderBy(x => Guid.NewGuid()).ToList();
            foreach (Actor a in actors2)
            {
                if (a != obj.a && a.Any())
                {
                    float num2 = a.stats[mw_S.magic_power];
                    if (actor == null || num2 < num)
                    {
                        actor = a;
                        num = num2;
                    }
                }
            }
            return actor;
        }
    }
}