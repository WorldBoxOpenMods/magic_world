using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using magic_world.Code;
using magic_world.Utils;
using Unity.Profiling.LowLevel;

namespace magic_world
{
    [Serializable]
    public delegate T MWMagic<T>();
    [Serializable]
    public delegate T1 MWMagic<T1, T2>(T2 t);
    [Serializable]
    public delegate bool MW_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null);
        [Serializable]
    public delegate bool MW_GetHitAction(BaseSimObject pSelf, BaseSimObject pTarget, magic Magic,WorldTile pTile = null);
    public class magic
    {
        public Actor a = null;
        public string id = "null";
        public float current_time = 0f;
        public float interval = 1f;
        public bool animation = false;
        public bool destroy = false;
        public bool paused_ = true;
        public bool for_ = false;
        public float time = -1f;
        public bool hasInterval = false;
        public bool projectile = false;
        public bool Enmeybuff = false;
        public bool Selfbuff = false;
        public magic_type type = magic_type.normal;
        public BaseStats stats = new BaseStats();
        public List<string> textures = new List<string>();
        public MW_AttackAction attackAction = null;
        public MW_GetHitAction getHitAction = null;
        public WorldAction action = null;
        public element element;
        public void update(bool paused)
        {
            if (!hasInterval)
            {
                return;
            }
            if (destroy)
            {
                if (Main.Actor_Magic.ContainsKey(a))
                {
                    Main.Actor_Magic[a].Remove(this);
                }
                return;
            }
            if (!a.Any())
            {
                destroy = true;
                return;
            }
            if (paused_ && paused)
            {
                return;
            }
            float world_time = (float)World.world.getCurWorldTime();
            if (time > 0f && world_time >= time)
            {
                destroy = true;
                return;
            }
            float CoolingEnd = world_time + interval;
            if (current_time > CoolingEnd)
            {
                current_time = CoolingEnd;
            }
            if (world_time >= current_time)
            {
                float value = world_time - current_time;
                if (interval != float.MaxValue && interval > 0f)
                {
                    if (value >= interval)
                    {
                        value %= interval;
                    }
                    current_time = CoolingEnd - value;
                }
                else
                {
                    current_time = interval;
                }
                if (!for_)
                {
                    if (destroy)
                    {
                        Main.Actor_Magic[a].Remove(this);
                        return;
                    }
                    if (animation)
                    {
                        if (textures.Any())
                        {
                            textures.Remove(textures[0]);
                        }
                        destroy = !textures.Any();
                    }
                    if (Main.Actor_Magic[a].Contains(this))
                    {
                        // magic Magic = Main.Actor_Magic[a].FirstOrDefault(m => m.id == id);
                        if (action != null && Toolbox.randomChance(stats[mw_S.change]))
                        {
                            action(a, a.currentTile);
                        }
                    }
                    else if (!animation)
                    {
                        destroy = true;
                    }
                }
                else for (int i = 0; i < 1 + (int)(value / interval); i++)
                    {
                        if (destroy)
                        {
                            Main.Actor_Magic[a].Remove(this);
                            return;
                        }
                        if (animation)
                        {
                            if (textures.Any())
                            {
                                textures.RemoveAt(0); ;
                            }
                            destroy = !textures.Any();
                        }
                        if (Main.Actor_Magic[a].Contains(this))
                        {
                            if (action != null && Toolbox.randomChance(stats[mw_S.change]))
                            {
                                action(a, a.currentTile);
                            }
                        }
                        else if (!animation)
                        {
                            destroy = true;
                        }
                    }
            }
        }
    }
}