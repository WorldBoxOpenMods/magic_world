using HarmonyLib;
using System;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using ReflectionUtility;
using System.Threading;

namespace magic_world
{
    class mw_effects : StackEffects
    {
        public static void init()
        {
            EffectAsset root_wrapping = new EffectAsset
            {
                id = "fx_root_wrapping",
                use_basic_prefab = true,
                sorting_layer_id = "EffectsBack",
                sprite_path = $"effects/fx_root_wrapping_t",
                draw_light_area = true,
                draw_light_size = 0.2f,
                limit = 80
            };
            AssetManager.effects_library.add(new EffectAsset
            {
                id = "fx_absorb_effect",
                // prefab_id = "effects/prefabs/PrefabAntimatterEffect",
                use_basic_prefab = true,
                sorting_layer_id = "EffectsBack",
                sprite_path = $"effects/fx_absorb_t",
                // show_on_mini_map = true,
                // sound_launch = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb",
                // spawn_action = new EffectAction(AssetManager.effects_library.spawnSimpleTile)
            });
                        AssetManager.effects_library.add(new EffectAsset
            {
                id = "photosphere",
                // prefab_id = "effects/prefabs/PrefabAntimatterEffect",
                use_basic_prefab = true,
                sorting_layer_id = "EffectsBack",
                sprite_path = $"effects/fx_photosphere_t",
                // show_on_mini_map = true,
                // sound_launch = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb",
                // spawn_action = new EffectAction(AssetManager.effects_library.spawnSimpleTile)
            });
            AssetManager.effects_library.add(root_wrapping);
            BaseStatAsset magic_power = new BaseStatAsset
            {
                id = mw_S.magic_power,
                normalize = true,
                normalize_min = 0f,
                used_only_for_civs = true
            };
            AssetManager.base_stats_library.add(magic_power);
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.projectiles,
                normalize = true,
                normalize_min = 1,
                normalize_max = 10,
                used_only_for_civs = true
            });
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.incidence,
                normalize = true,
                normalize_min = 1,
                used_only_for_civs = true
            });
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.additional_damage,
                normalize = true,
                normalize_min = 0,
                used_only_for_civs = true
            });
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.change,
                normalize = true,
                show_as_percents = true,
                normalize_min = 0.1f,
                normalize_max = 1,
                used_only_for_civs = true
            });
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.restoreHealth,
                normalize = true,
                normalize_min = 0,
                used_only_for_civs = true
            });
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.target,
                normalize = true,
                normalize_min = 3,
                used_only_for_civs = true
            });
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.element_effect_extension,
                normalize = true,
                normalize_min = 0,
                used_only_for_civs = true
            });
            AssetManager.base_stats_library.add(new BaseStatAsset
            {
                id = mw_S.currentMagicPower,
                normalize = true,
                normalize_min = 0,
                used_only_for_civs = true
            });

        }

    }
}