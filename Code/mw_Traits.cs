using System;
using System.Collections.Generic;
using System.Linq;
using magic_world.Utils;
using UnityEngine;

namespace magic_world;
public class mw_traits
{
    public static void init()
    {
        var traits = AssetManager.traits.add(new ActorTrait
        {
            id = "dark",
            path_icon = "ui/Icons/traits/icon",
            group_id = TraitGroup.mind

        });
        traits.base_stats[S.intelligence] = 999;
        traits.base_stats[mw_S.magic_power] += 999;
        PlayerConfig.unlockTrait(traits.id);
        traits.action_special_effect = (WorldAction)Delegate.Combine(traits.action_special_effect, new WorldAction(darkAction));
        AssetManager.traits.add(traits);
        traits = AssetManager.traits.add(new ActorTrait
        {
            id = "ice",
            path_icon = "ui/Icons/traits/icon",
            group_id = TraitGroup.mind

        });
        traits.base_stats[S.intelligence] = 999;
        traits.base_stats[mw_S.magic_power] += 999;
        PlayerConfig.unlockTrait(traits.id);
        traits.action_special_effect = (WorldAction)Delegate.Combine(traits.action_special_effect, new WorldAction(iceAction));
        AssetManager.traits.add(traits);
        traits = AssetManager.traits.add(new ActorTrait
        {
            id = "fire",
            path_icon = "ui/Icons/traits/icon",
            group_id = TraitGroup.mind

        });
        traits.base_stats[S.intelligence] = 999;
        traits.base_stats[mw_S.magic_power] += 999;
        PlayerConfig.unlockTrait(traits.id);
        traits.action_special_effect = (WorldAction)Delegate.Combine(traits.action_special_effect, new WorldAction(fireAction));
        AssetManager.traits.add(traits);
        traits = AssetManager.traits.add(new ActorTrait
        {
            id = "nature",
            path_icon = "ui/Icons/traits/icon",
            group_id = TraitGroup.mind

        });
        traits.base_stats[S.intelligence] = 999;
        traits.base_stats[mw_S.magic_power] += 999;
        PlayerConfig.unlockTrait(traits.id);
        traits.action_special_effect = (WorldAction)Delegate.Combine(traits.action_special_effect, new WorldAction(natureAction));
        AssetManager.traits.add(traits);
        traits = AssetManager.traits.add(new ActorTrait
        {
            id = "light",
            path_icon = "ui/Icons/traits/icon",
            group_id = TraitGroup.mind

        });
        traits.base_stats[S.intelligence] = 999;
        traits.base_stats[mw_S.magic_power] += 999;
        PlayerConfig.unlockTrait(traits.id);
        traits.action_special_effect = (WorldAction)Delegate.Combine(traits.action_special_effect, new WorldAction(lightAction));
        AssetManager.traits.add(traits);
    }
    public static bool lightAction(BaseSimObject pTarget, WorldTile pTile = null)
    {
        Actor a = pTarget.a;
        if (!a.Any())
        {
            return false;
        }
        a.data.set(mw_S.magic_power, 10000);
        a.data.set("currentMagicPower", 10000);
        if (!Main.Actor_Magic.ContainsKey(a))
        {
            Main.Actor_Magic.Add(a, new List<magic>() { });
        }
        AddMagic(a, Main.LightMagic);

        a.removeTrait("light");
        return false;
    }
    public static bool darkAction(BaseSimObject pTarget, WorldTile pTile = null)
    {
        Actor a = pTarget.a;
        if (!a.Any())
        {
            return false;
        }
        a.data.set(mw_S.magic_power, 10000);
        a.data.set("currentMagicPower", 10000);
        if (!Main.Actor_Magic.ContainsKey(a))
        {
            Main.Actor_Magic.Add(a, new List<magic>() { });
        }
        AddMagic(a, Main.DarkMagic);

        a.removeTrait("dark");
        return false;
    }
    public static bool iceAction(BaseSimObject pTarget, WorldTile pTile = null)
    {
        Actor a = pTarget.a;
        if (!a.Any())
        {
            return false;
        }
        a.data.set(mw_S.magic_power, 10000);
        a.data.set("currentMagicPower", 10000);
        if (!Main.Actor_Magic.ContainsKey(a))
        {
            Main.Actor_Magic.Add(a, new List<magic>() { });
        }
        AddMagic(a, Main.IceMagic);

        a.removeTrait("ice");
        return false;
    }
    public static bool fireAction(BaseSimObject pTarget, WorldTile pTile = null)
    {
        Actor a = pTarget.a;
        if (!a.Any())
        {
            return false;
        }
        a.data.set(mw_S.magic_power, 10000);
        a.data.set("currentMagicPower", 10000);
        if (!Main.Actor_Magic.ContainsKey(a))
        {
            Main.Actor_Magic.Add(a, new List<magic>() { });
        }
        AddMagic(a, Main.FireMagic);

        a.removeTrait("fire");
        return false;
    }
    public static bool natureAction(BaseSimObject pTarget, WorldTile pTile = null)
    {
        Actor a = pTarget.a;
        if (!a.Any())
        {
            return false;
        }
        a.data.set(mw_S.magic_power, 10000);
        a.data.set("currentMagicPower", 10000);
        if (!Main.Actor_Magic.ContainsKey(a))
        {
            Main.Actor_Magic.Add(a, new List<magic>() { });
        }
        AddMagic(a, Main.NatureMagic);
        a.removeTrait("nature");

        return false;
    }
    public static void AddMagic(Actor a, Dictionary<string, magic> MagicDict = null)
    {
        foreach (var magic in MagicDict.Values)
        {
            if (Main.Actor_Magic[a].Any(m => m.id == magic.id))
            {
                magic Magic = Main.Actor_Magic[a].First(m => m.id == magic.id);
                MWTools.NewEntry(Magic);
            }
            else
            {
                magic.a = a;
                Main.Actor_Magic[a].Add(magic);
            }
        }
    }
}

