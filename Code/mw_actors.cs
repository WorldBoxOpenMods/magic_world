using NCMS.Utils;
using ReflectionUtility;

namespace magic_world;
class mw_actors
{
    public static void init()
    {
        var dark_clone = AssetManager.actor_library.clone("dark_clone", "_mob");
        dark_clone.id = "dark_clone";
        dark_clone.setBaseStats(80, 5, 45, 0, 0, 0, 0);
        dark_clone.base_stats[S.attack_speed] = 30;
        dark_clone.job = "random_move";
        dark_clone.texture_path = "dark_clone";
        dark_clone.canTurnIntoZombie = false;
        dark_clone.sound_hit = "event:/SFX/HIT/HitFlesh";
        AssetManager.actor_library.CallMethod("loadShadow", dark_clone);
        Localization.addLocalization("dark_clone", "暗影分身");
        AssetManager.actor_library.add(dark_clone);
    }
}