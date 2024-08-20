

using System;

namespace magic_world;
class mw_projectile
{
    public static void init()
    {
        var icicle = AssetManager.projectiles.clone("icicle", "snowball");
        icicle.texture = "icicle";
        icicle.speed = 12f;
        icicle.texture_shadow = "shadow_ball";
        icicle.hitFreeze = true;
        icicle.sound_launch = "event:/SFX/WEAPONS/WeaponStartThrow";
        icicle.sound_impact = "event:/SFX/WEAPONS/WeaponSnowballLand";
        icicle.startScale = 0.08f;
        icicle.targetScale = 0.08f;
        AssetManager.projectiles.add(icicle);
        var fire = AssetManager.projectiles.add(new ProjectileAsset
        {
            id = "fire",
            speed = 14f,
            speed_random=5f,
            texture = "fire",
            parabolic = true,
            texture_shadow = "shadow_ball",
            terraformOption = "mw_fire",
            startScale = 0.075f,
            targetScale = 0.075f,
            draw_light_area = true,
            sound_launch = "event:/SFX/WEAPONS/WeaponRedOrbStart",
            sound_impact = "event:/SFX/WEAPONS/WeaponRedOrbLand",
            // terraformRange = 1
        });
        fire.world_actions = (AttackAction)Delegate.Combine(fire.world_actions, new AttackAction(magicLibrary.burnTile_Fire));
        var rapid_red_teeth = AssetManager.projectiles.add(new ProjectileAsset
        {
            id = "rapid_red_teeth",
            speed = 15f,
            speed_random=5f,
            texture = "fireE",
            parabolic = true,
            texture_shadow = "shadow_ball",
            terraformOption = "mw_fire",
            startScale = 0.075f,
            targetScale = 0.075f,
            draw_light_area = true,
            sound_launch = "event:/SFX/WEAPONS/WeaponRedOrbStart",
            sound_impact = "event:/SFX/WEAPONS/WeaponRedOrbLand",
            // terraformRange = 1
        });
        rapid_red_teeth.world_actions = (AttackAction)Delegate.Combine(rapid_red_teeth.world_actions, new AttackAction(magicLibrary.burnTile_Fire));
        var DarkBall = AssetManager.projectiles.add(new ProjectileAsset
        {
            id = "DarkBall",
            speed = 10f,
            texture = "DarkBall",
            parabolic = true,
            texture_shadow = "shadow_ball",
            terraformOption = "mw_DarkBall",
            terraformRange = 3,
            startScale = 0.1f,
            targetScale = 0.1f,
            draw_light_area = false,
            sound_launch = "event:/SFX/WEAPONS/WeaponRedOrbStart",
            sound_impact = "event:/SFX/WEAPONS/WeaponRedOrbLand"
        });
        var photosphere = AssetManager.projectiles.add(new ProjectileAsset
        {
            id = "photosphere",
            speed = 18f,
            texture = "photosphere",
            parabolic = false,
            texture_shadow = "shadow_ball",
            terraformOption = "mw_photosphere",
            startScale = 0.075f,
            targetScale = 0.075f,
            draw_light_area = true,
            sound_launch = "event:/SFX/WEAPONS/WeaponRedOrbStart",
            sound_impact = "event:/SFX/WEAPONS/WeaponRedOrbLand"
        });
                var FreezingRay = AssetManager.projectiles.add(new ProjectileAsset
        {
            id = "FreezingRay",
            speed = 18f,
            texture = "FreezingRay",
            parabolic = false,
            texture_shadow = "shadow_ball",
            terraformOption = "mw_photosphere",
            startScale = 0.075f,
            targetScale = 0.075f,
            look_at_target=true,
            draw_light_area = true,
            sound_launch = "event:/SFX/WEAPONS/WeaponRedOrbStart",
            sound_impact = "event:/SFX/WEAPONS/WeaponRedOrbLand"
        });


        var mw_fire = AssetManager.terraform.add(new TerraformOptions
        {
            id = "mw_fire",
            flash = true,
            addBurned = true,
            force_power = 0.5f,
            damage = 15,
            setFire = true,
            applyForce = false,
            shake = false,
            damageBuildings = true,
            removeFrozen = true,
            attackType = AttackType.Fire
        });
        var mw_DarkBall = AssetManager.terraform.add(new TerraformOptions
        {
            id = "mw_DarkBall",
            flash = false,
            addBurned = false,
            force_power = 0.1f,
            damage = 20,
            setFire = false,
            applyForce = true,
            shake = false,
            damageBuildings = false,
            removeFrozen = true,
            attackType = AttackType.Other
        });
        var mw_photosphere = AssetManager.terraform.add(new TerraformOptions
        {
            id = "mw_photosphere",
            flash = true,
            addBurned = false,
            damage = 25,
            setFire = false,
            applyForce = false,
            shake = false,
            damageBuildings = false,
            removeFrozen = false,
            attackType = AttackType.Other
        });
    }
}