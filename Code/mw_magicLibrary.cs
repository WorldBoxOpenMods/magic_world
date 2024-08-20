using HarmonyLib;
using ai.behaviours;
using NCMS.Utils;
using NeoModLoader.api;
using System;
using UnityEngine;
using System.Data;
using System.Collections.Generic;
using ReflectionUtility;
using UnityEngine.UI;
using ai;
using magic_world.Utils;
using magic_world;
using System.Linq;
using System.Globalization;
using magic_world.Code;

namespace magic_world;

class magicLibrary
{
    public static void init()
    {
        #region 火系法术
        magic magic = new()
        {
            id = "fire",
            projectile = true,
            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(actionFire)),
            element = element.fire,
        };
        magic.stats[mw_S.change] = 0.3f;
        magic.stats[mw_S.incidence] = 3;
        magic.stats[mw_S.projectiles] = 1;
        Main.projectiles.Add(magic.id);
        Main.FireMagic.Add("fire", magic);
        magic = new()
        {
            id = "rapid_red_teeth",
            projectile = true,
            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(action_rapid_red_teeth)),
            element = element.fire,
        };
        magic.stats[mw_S.change] = 0.2f;
        magic.stats[mw_S.incidence] = 4;
        magic.stats[mw_S.projectiles] = 1;
        Main.projectiles.Add(magic.id);
        Main.FireMagic.Add("rapid_red_teeth", magic);
        magic = new()
        {
            id = "FireControl",
            type = magic_type.controlElements,
            element = element.fire,
        };
        magic.stats[mw_S.change] = 0.95f;
        Main.FireMagic.Add("FireControl", magic);
        #endregion
        #region 冰系法术
        magic = new()
        {
            id = "icicle",
            projectile = true,
            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(actionIcicle)),
            element = element.ice
        };
        magic.stats[mw_S.projectiles] = 2;
        magic.stats[mw_S.change] = 0.2f;
        Main.projectiles.Add(magic.id);
        Main.IceMagic.Add("icicle", magic);
        magic = new()
        {
            id = "FreezingRay",
            projectile = true,
            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(actionFreezingRay)),
            element = element.ice
        };
        magic.stats[mw_S.projectiles] = 2;
        magic.stats[mw_S.change] = 0.2f;
        Main.projectiles.Add(magic.id);
        Main.IceMagic.Add("FreezingRay", magic);
        magic = new()
        {
            id = "snow",
            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(actionSnow)),
            element = element.ice
        };
        magic.stats[mw_S.incidence] = 2;
        magic.stats[mw_S.change] = 0.1f;
        Main.IceMagic.Add("snow", magic);
        magic = new()
        {
            id = "IceControl",
            type = magic_type.controlElements,
            element = element.ice,
        };
        magic.stats[mw_S.change] = 1f;
        Main.IceMagic.Add("IceControl", magic);
        #endregion
        #region 自然系法术
        magic = new()
        {
            id = "The Source of Life",
            type = magic_type.restore,
            getHitAction = (MW_GetHitAction)Delegate.Combine(new MW_GetHitAction(actionbloodRain)),
            element = element.nature
        };
        magic.stats[mw_S.target] = 10;
        magic.stats[mw_S.change] = 0.2f;
        magic = new()
        {
            id = "EarthBlessings",
            Selfbuff = true,
            getHitAction = (MW_GetHitAction)Delegate.Combine(new MW_GetHitAction(actionEarthBlessings)),
            element = element.nature
        };
        magic.stats[mw_S.target] = 5;
        magic.stats[mw_S.change] = 0.3f;
        Main.NatureMagic.Add("EarthBlessings", magic);
        magic = new()
        {
            id = "whirlwind",
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(actionwhirlwind)),
            element = element.nature
        };
        magic.stats[mw_S.change] = 0.1f;
        Main.NatureMagic.Add("whirlwind", magic);
        // magic = new()
        // {
        //     id = "WindControl",
        //     type = magic_type.controlElements,
        //     element = element.nature,
        // };
        // magic.stats[mw_S.change] = 0.8f;
        // Main.NatureMagic.Add("WindControl", magic);
        magic = new()
        {
            id = "root_wrapping",
            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(action_root_wrapping)),
            element = element.nature
        };
        magic.stats[mw_S.target] = 7;
        magic.stats[mw_S.change] = 0.1f;
        Main.NatureMagic.Add("root_wrapping", magic);
        #region 暗系法术
        #endregion
        magic = new()
        {
            id = "swallow",
            Enmeybuff = true,
            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(action_swallow)),
            element = element.dark
        };
        magic.stats[mw_S.change] = 0.1f;
        magic.stats[mw_S.target] = 3;
        Main.DarkMagic.Add("swallow", magic);
        magic = new()
        {
            id = "absorb",

            type = magic_type.element_effect,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(action_absorb)),
            element = element.dark
        };
        magic.stats[mw_S.target] = 4;
        magic.stats[mw_S.change] = 0.1f;
        Main.DarkMagic.Add("absorb", magic);
        magic = new()
        {
            id = "DarkBall",
            type = magic_type.element_effect,
            projectile = true,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(action_DarkBall)),
            element = element.dark
        };
        magic.stats[mw_S.projectiles] = 1;
        magic.stats[mw_S.change] = 0.2f;
        Main.projectiles.Add(magic.id);
        Main.DarkMagic.Add("DarkBall", magic);
        magic = new()
        {
            id = "dark_clone",
            getHitAction = (MW_GetHitAction)Delegate.Combine(new MW_GetHitAction(action_dark_clone)),
            element = element.dark
        };
        magic.stats[mw_S.change] = 0.1f;
        Main.DarkMagic.Add("dark_clone", magic);
        #endregion
        #region 光系法术
        magic = new()
        {
            id = "delivery",
            // attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(action_delivery)),
            element = element.light
        };
        magic.stats[mw_S.change] = 0.1f;
        Main.LightMagic.Add("delivery", magic);
        magic = new()
        {
            id = "photosphere",
            type = magic_type.element_effect,
            projectile = true,
            attackAction = (MW_AttackAction)Delegate.Combine(new MW_AttackAction(action_photosphere)),
            element = element.light
        };
        magic.stats[mw_S.projectiles] = 2;
        magic.stats[mw_S.change] = 0.1f;
        Main.projectiles.Add(magic.id);
        Main.LightMagic.Add("photosphere", magic);
        #endregion




    }
    #region 法术行为
    public static bool action_swallow(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (!pSelf.a.Any()) { return false; }
        if (pTarget != null)
        {
            pTile = pTarget.currentTile;
        }
        if (pTile == null)
        {
            return false;
        }
        MWTools.checkCMP(pSelf.a, 20);

        // //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "swallow");
        float world_time = (float)World.world.getCurWorldTime();
        foreach (Actor actor in Main.getObjectsInChunks(pSelf.a.currentTile, (int)Magic.stats[mw_S.target], MapObjectType.Actor).Cast<Actor>())
        {
            if (actor.Any() && actor.kingdom != null && pSelf.a.kingdom.isEnemy(actor.kingdom))
            {
                if (actor.isInAir() || actor.isFlying())
                {
                    if (Main.Actor_Magic.ContainsKey(actor) && Main.Actor_Magic[actor].FirstOrDefault(m => m.id == "swallow_effect") != null)
                    {
                        break;
                    }
                    actor.data.health -= (int)Magic.stats[mw_S.additional_damage];
                    float damage = actor.stats[S.damage] * 0.1f;

                    magic Magic2 = new magic
                    {
                        id = "swallow_effect",
                        a = actor,
                        hasInterval = true,
                        time = world_time + 5f + Magic.stats[mw_S.element_effect_extension],
                        current_time = world_time + 5f,
                        interval = 5f
                    };

                    Magic2.stats[S.damage] -= damage;
                    Main.Actor_Magic.TryGetValue(actor, out List<magic> actorMagic);


                    if (actorMagic == null)
                    {
                        Main.Actor_Magic[actor] = new List<magic> { Magic2 };
                    }
                    else
                    {
                        actorMagic.Add(Magic2);
                    }

                    Magic2.a = pSelf.a;
                    Magic2.stats[S.damage] = damage;
                    if (Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "swallow_effect") != null)
                    {
                        break;
                    }
                    Main.Actor_Magic[pSelf.a].Add(Magic2);
                    actor.statsDirty = true;
                    actor.updateStats();
                    actor.startColorEffect(ActorColorEffect.Red);
                }
            }
        }
        pSelf.a.statsDirty = true;
        pSelf.a.updateStats();
        return true;
    }
    public static bool action_absorb(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (!pSelf.a.Any()) { return false; }
        if (pTarget != null)
        {
            pTile = pTarget.currentTile;
        }
        if (pTile == null)
        {
            return false;
        }
        // if (!Main.Actor_Magic.TryGetValue(pSelf.a, out List<magic> selfMagic) || !selfMagic.Any(m => m.id == "swallow"))
        // {
        //     return false;  // 如果没有找到有效的魔法，直接返回
        // }
        Vector3 posV = pTile.tile_up.posV3;
        posV.x += Toolbox.randomFloat(-0.3f, 0.3f);
        posV.y += Toolbox.randomFloat(-0.3f, 0.3f);
        if (!MWTools.checkCMP(pSelf.a, 10)) { return true; }
        EffectsLibrary.spawnAt("fx_absorb_effect", posV, 0.1f);
        // //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "absorb");
        foreach (Actor actor in Main.getObjectsInChunks(pSelf.a.currentTile, (int)Magic.stats[mw_S.target], MapObjectType.Actor).Cast<Actor>())
        {
            if (actor.Any() && actor.kingdom != null && pSelf.a.kingdom.isEnemy(actor.kingdom))
            {
                if (actor.isInAir() || actor.isFlying())
                {
                    actor.getHit(10f, false, AttackType.Other, pSelf.a, false);
                    pSelf.a.restoreHealth(8);
                }
            }
        }

        return true;
    }
    public static bool actionSnow(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (pTarget==null||!pSelf.a.Any()) { return true; }
        if (pTarget != null)
        {
            pTile = pTarget.currentTile;
        }
        if (pTile == null)
        {
            return false;
        }
        // if (!Main.Actor_Magic.TryGetValue(pSelf.a, out List<magic> selfMagic) || !selfMagic.Any(m => m.id == "swallow"))
        // {
        //     return false;  // 如果没有找到有效的魔法，直接返回
        // }
        if (!MWTools.checkCMP(pSelf.a, 3)) { return true; }
        // int effect_texture_width = 3;
        // int effect_texture_height = 1;
        // //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "snow");
        int incidence = (int)Magic.stats[mw_S.incidence];
        int startX = (int)pTarget.transform.localPosition.x;
        int startY = (int)pTarget.transform.localPosition.y;

        for (int x = startX - incidence / 2; x <= startX + (incidence / 2 - 1); x++)
        {
            for (int y = startY - incidence / 2; y <= startY + (incidence / 2 - 1); y++)
            {
                WorldTile tile = World.world.GetTile(x, y);
                if (tile != null)
                {
                    World.world.dropManager.spawn(tile, "snow", 5f, -1f);
                }
            }
        }

        return true;
    }

    public static bool action_root_wrapping(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (!pSelf.a.Any()) { return false; }
        if (pTarget != null)
        {
            pTile = pTarget.currentTile;
        }
        if (pTile == null)
        {
            return false;
        }
        // if (!Main.Actor_Magic.TryGetValue(pSelf.a, out List<magic> selfMagic) || !selfMagic.Any(m => m.id == "root_wrapping"))
        // {
        //     return false;  // 如果没有找到有效的魔法，直接返回
        // }
        if (!MWTools.checkCMP(pSelf.a, 5)) { return true; }
        // //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "root_wrapping");

        List<Actor> actorsToModify = new();


        foreach (Actor actor in Main.getObjectsInChunks(pSelf.a.currentTile, (int)Magic.stats[mw_S.target], MapObjectType.Actor).Cast<Actor>())
        {
            if (actor.Any() && actor.kingdom != null && pSelf.a.kingdom.isEnemy(actor.kingdom))
            {
                if (actor.isInAir() || actor.isFlying())
                {
                    actor.addStatusEffect("slowness", 5f + Magic.stats[mw_S.element_effect_extension]);
                    actor.getHit(8, false, AttackType.Other, pSelf.a, false, false);
                    // EffectsLibrary.spawn("fx_fx_root_wrapping", actor.currentTile, null, null, 0f, -1f, -1f);
                    actor.startShake(0.3f, 0.1f, true, true);
                    actor.startColorEffect(ActorColorEffect.White);
                }
            }
        }

        // Debug.Log("触发根缠");
        return true;

    }
    public static bool actionEarthBlessings(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (!pSelf.a.Any()) { return false; }
        pTile = pSelf.currentTile;
        if (pTile == null || pSelf.a.data.health < pSelf.a.stats[S.health] * 0.8)
        {
            return false;
        }
        // if (!Main.Actor_Magic.TryGetValue(pSelf.a, out List<magic> selfMagic) || !selfMagic.Any(m => m.id == "The Source of Life"))
        // {
        //     return false;  // 如果没有找到有效的魔法，直接返回
        // }
        if (!MWTools.checkCMP(pSelf.a, 5)) { return true; }

        // //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "The Source of Life");
        World.world.getObjectsInChunks(pTile, (int)Magic.stats[mw_S.target], MapObjectType.Actor);
        float world_time = (float)World.world.getCurWorldTime();
        for (int i = 0; i < World.world.temp_map_objects.Count; i++)
        {
            Actor actor = (Actor)World.world.temp_map_objects[i];
            if (actor.Any() && actor.kingdom != null && actor.kingdom == pSelf.a.kingdom)
            {
                if (actor.isInAir() || actor.isFlying())
                {
                    if (Main.Actor_Magic.ContainsKey(actor) && Main.Actor_Magic[actor].FirstOrDefault(m => m.id == "EarthBlessings_effect") != null)
                    {
                        break;
                    }

                    magic Magic2 = new magic
                    {
                        id = "EarthBlessings_effect",
                        a = actor,
                        hasInterval = true,
                        time = world_time + 10f + Magic.stats[mw_S.element_effect_extension],
                        current_time = world_time + 5f,
                        interval = 10f
                    };
                    Magic2.stats[S.armor] = 5f;
                    Main.Actor_Magic[pSelf.a].Add(Magic2);

                    actor.startShake(0.3f, 0.1f, true, true);
                    actor.startColorEffect(ActorColorEffect.White);
                }
            }
        }
        return true;

    }
    public static bool actionbloodRain(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (!pSelf.a.Any()) { return false; }
        pTile = pSelf.currentTile;
        if (pTile == null || pSelf.a.data.health < pSelf.a.stats[S.health] * 0.7)
        {
            return false;
        }
        // if (!Main.Actor_Magic.TryGetValue(pSelf.a, out List<magic> selfMagic) || !selfMagic.Any(m => m.id == "The Source of Life"))
        // {
        //     return false;  // 如果没有找到有效的魔法，直接返回
        // }
        if (!MWTools.checkCMP(pSelf.a, 5)) { return true; }

        //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "The Source of Life");
        World.world.getObjectsInChunks(pTile, (int)Magic.stats[mw_S.target], MapObjectType.Actor);
        for (int i = 0; i < World.world.temp_map_objects.Count; i++)
        {
            Actor actor = (Actor)World.world.temp_map_objects[i];
            if (actor.Any() && actor.kingdom != null && actor.kingdom == pSelf.a.kingdom && actor.data.health < actor.stats[S.health] * 0.7)
            {
                actor.finishStatusEffect("burning");
                actor.restoreHealth(5 + (int)Magic.stats[mw_S.restoreHealth]);
                actor.startShake(0.3f, 0.1f, true, true);
                actor.startColorEffect(ActorColorEffect.White);
            }
        }
        // Debug.Log("触发回血");
        return true;

    }
    public static bool actionwhirlwind(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (pTarget != null)
        {
            pTile = pTarget.currentTile;
        }
        if (pTile == null)
        {
            return false;
        }
        if (!MWTools.checkCMP(pSelf.a, 10)) { return true; }
        Actor a = World.world.units.createNewUnit(SA.tornado, pTile, 0f);
        Tornado component = a.GetComponent<Tornado>();
        component.forceScaleTo(Tornado.TORNADO_SCALE_DEFAULT / 12f);
        component.resizeTornado(Tornado.TORNADO_SCALE_DEFAULT / 6f);
        a.kingdom = pSelf.kingdom;
        a.data.set("IsRotate", true);
        return true;
    }
    public static bool action_dark_clone(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (pTarget != null)
        {
            pTile = pTarget.currentTile;
        }
        if (pTile == null || pSelf.a.data.health < pSelf.a.stats[S.health] * 0.8)
        {
            return false;
        }

        if (!MWTools.checkCMP(pSelf.a, 10)) { return true; }
        float world_time = (float)World.world.getCurWorldTime();
        if (pSelf.a.data.health <= pSelf.a.stats[S.health] * 0.1)
        {
            Actor a = World.world.units.createNewUnit(SA.baby_human, pTile, 0f);
            a.kingdom = pSelf.a.kingdom;
            a.data.set("summoningTime", world_time + 5f);
            MWTools.copyUnitToOtherUnit(pSelf.a, a);
        }
        return true;
    }
    public static bool action_DarkBall(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {

        if (pSelf==null||pTarget==null||!pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }
        if (!MWTools.checkCMP(pSelf.a, 15)) { return true; }

        //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "DarkBall");
        spawnProjectile(pSelf, pTarget, pTile, Magic);

        return true;
    }
    public static bool action_rapid_red_teeth(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {

        if (pSelf==null||pTarget==null||!pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }
        if (!Main.Actor_Magic.TryGetValue(pSelf.a, out List<magic> selfMagic) || !selfMagic.Any(m => m.id == "rapid_red_teeth"))
        {
            return false;  // 如果没有找到有效的魔法，直接返回
        }
        if (!MWTools.checkCMP(pSelf.a, 10)) { return true; }

        //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "rapid_red_teeth");
        spawnProjectile(pSelf, pTarget, pTile, Magic);
        return true;
    }
    public static bool action_photosphere(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {

        if (pSelf==null||pTarget==null||!pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }

        if (!MWTools.checkCMP(pSelf.a, 10)) { return true; }

        //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "photosphere");
        if (pTarget != null)
        {
            pTile = pTarget.currentTile;
        }
        if (pTile == null)
        {
            return false;
        }
        Vector3 posV = pTile.tile_up.posV3;
        posV.x += Toolbox.randomFloat(-0.3f, 0.3f);
        posV.y += Toolbox.randomFloat(-0.3f, 0.3f);
        EffectsLibrary.spawnAt("fx_photosphere_effect", posV, 0.3f);
        spawnProjectile(pSelf, pTarget, pTile, Magic);


        return true;
    }
    public static bool actionFire(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {

        if (pSelf==null||pTarget==null||!pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }
        if (!MWTools.checkCMP(pSelf.a, 5)) { return true; }

        //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "fire");
        spawnProjectile(pSelf, pTarget, pTile, Magic);

        return true;
    }
    public static bool actionFreezingRay(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (pSelf==null||pTarget==null||!pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }
        if (pTarget.isBuilding())
        {
            return false;
        }
        if (pTarget.currentTile.Type.lava)
        {
            return false;
        }
        if (pTarget.currentTile.isOnFire())
        {
            return false;
        }
        if (!MWTools.checkCMP(pSelf.a, 5)) { return true; }

        //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "FreezingRay");
        spawnProjectile(pSelf, pTarget, pTile, Magic);
        pTarget.a.addStatusEffect("frozen", 15f + Magic.stats[mw_S.element_effect_extension]);

        return true;
    }
    public static bool actionIcicle(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null)
    {
        if (pSelf==null||pTarget==null||!pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }
        if (pTarget.isBuilding())
        {
            return false;
        }
        if (pTarget.currentTile.Type.lava)
        {
            return false;
        }
        if (pTarget.currentTile.isOnFire())
        {
            return false;
        }
        if (!MWTools.checkCMP(pSelf.a, 5)) { return true; }

        //magic Magic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "icicle");
        spawnProjectile(pSelf, pTarget, pTile, Magic);
        pTarget.a.addStatusEffect("frozen", 15f + Magic.stats[mw_S.element_effect_extension]);

        return true;
    }
    #endregion
    public static void spawnProjectile(BaseSimObject a, BaseSimObject target, WorldTile pTile, magic magic)
    {
        pTile = a.currentTile;
        if (pTile == null) { return; }
        a.a.data.get("currentMagicPower", out int CMP, 0);
        if (CMP <= 0) { return; }
        float pDist = target.stats[S.size];
        float num = a.stats[S.size];
        int num2 = (int)magic.stats[mw_S.projectiles];
        Vector2 attackPosition = getAttackPosition(target);
        attackPosition.y += 0.1f;
        Debug.Log(num2);
        for (int i = 0; i < num2; i++)
        {
            Vector2 vector = new Vector2(attackPosition.x, attackPosition.y);
            vector.x += Toolbox.randomFloat(-(num + 1f), num + 1f);
            vector.y += Toolbox.randomFloat(-num, num);
            Vector3 newPoint = Toolbox.getNewPoint(a.currentPosition.x, a.currentPosition.y, vector.x, vector.y, pDist, true);
            newPoint.y += 0.5f;
            float pZ = 0f;
            if (target.isInAir())
            {
                pZ = target.getZ();
            }
            Projectile projectile = EffectsLibrary.spawnProjectile(magic.id, newPoint, vector, pZ);
            if (projectile != null)
            {
                projectile.byWho = a;
                projectile.stats["damage"] += magic.stats[mw_S.additional_damage];
                projectile.targetObject = target;
            }
        }
    }
    public static Vector2 getAttackPosition(BaseSimObject pTarget)
    {
        float num = pTarget.stats[S.size];
        Vector2 vector = new Vector2(pTarget.currentPosition.x, pTarget.currentPosition.y);
        if (pTarget.isActor() && pTarget.a.is_moving && pTarget.isFlying())
        {
            vector = Vector2.MoveTowards(vector, pTarget.a.nextStepPosition, num * 3f);
        }
        return vector;
    }
    public static bool burnTile_Fire(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null)
    {
        if (pSelf == null || pTarget == null || !pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }
        int num = 1;
        List<WorldTile> worldTiles = new();
        if (Main.Actor_Magic.ContainsKey(pSelf.a) && Main.Actor_Magic[pSelf.a].Any(m => m.id == "fire"))
        {
            magic fireMagic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "fire");
            num = (int)fireMagic.stats[mw_S.incidence];
        }
        burnTile(pSelf, pTarget, pTile, num);
        return true;
    }
    public static bool burnTile_FireE(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null)
    {
        if (pSelf == null || pTarget == null || !pSelf.a.Any() || !pTarget.a.Any() || pTarget.a.stats == null) { return false; }
        int num = 1;
        List<WorldTile> worldTiles = new();
        if (Main.Actor_Magic.ContainsKey(pSelf.a) && Main.Actor_Magic[pSelf.a].Any(m => m.id == "rapid_red_teeth"))
        {
            magic fireMagic = Main.Actor_Magic[pSelf.a].FirstOrDefault(m => m.id == "rapid_red_teeth");
            num = (int)fireMagic.stats[mw_S.incidence];

        }
        burnTile(pSelf, pTarget, pTile, num);
        return true;
    }
    public static bool burnTile(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null, int num = 1)
    {
        if (pTarget == null) { return false; }
        pTile = pTarget.currentTile;
        if (pTile == null) { return false; }
        List<WorldTile> worldTiles = new();
        if (!World.world.flashEffects.contains(pTile) && Toolbox.randomChance(0.2f))
        {
            World.world.particlesFire.spawn(pTile.posV3);
        }
        pTile.startFire(true);
        if (num > 1)
        {
            worldTiles = MWTools.GetSurroundingTiles(ActorTileTarget.RandomBurnableTile, pTile.chunk, num);
        }
        foreach (WorldTile tile in worldTiles)
        {
            if (!World.world.flashEffects.contains(tile) && Toolbox.randomChance(0.2f))
            {
                World.world.particlesFire.spawn(tile.posV3);
            }
            tile.startFire(true);
        }
        return true;
    }

}