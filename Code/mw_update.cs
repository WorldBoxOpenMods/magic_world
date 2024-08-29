using System;
using System.Linq;
using System.Collections.Generic;
using NCMS.Utils;
using UnityEngine;
using HarmonyLib;
using ai.behaviours;
using UnityEngine.UI;
using System.Security.Cryptography;
using magic_world;
using magic_world.Utils;

namespace magic_world
{
    class mw_update
    {
        public static void update_magic(bool paused)
        {
            if (paused)
            {
                return;
            }
            List<Actor> actorsToRemove = new List<Actor>();
            List<magic> destroyAction = new List<magic>();

            foreach (Actor a in Main.Actor_Magic.Keys)
            {
                if (a == null || !Main.Actor_Magic.ContainsKey(a) || Main.Actor_Magic[a] == null)
                {
                    if (a != null && Main.Actor_Magic.ContainsKey(a))
                    {
                        Main.Actor_Magic.TryRemove(a, out _);
                    }
                    continue;
                }

                for (int i = Main.Actor_Magic[a].Count - 1; i >= 0; i--)
                {
                    magic Magic = Main.Actor_Magic[a][i];

                    if (Magic.destroy)
                    {
                        destroyAction.Add(Magic);
                    }
                    else if (Magic.hasInterval)
                    {
                        if (Magic.a == null)
                        {
                            Magic.a = a;
                        }
                        Magic.update(paused);
                    }
                }

                foreach (magic destroyedMagic in destroyAction)
                {
                    if (Main.Actor_Magic.ContainsKey(a))
                    {
                        Main.Actor_Magic[a].Remove(destroyedMagic);
                    }
                }
            }

            foreach (magic Magic in destroyAction)
            {
                Actor a = Magic.a;

                if (Main.Actor_Magic.ContainsKey(a))
                {
                    Main.Actor_Magic[a].Remove(Magic);

                    if (Main.Actor_Magic[a].Count == 0)
                    {
                        Debug.Log("啊？");
                        Main.Actor_Magic.TryRemove(a, out _);
                    }
                }


                if (a != null && a.Any())
                {
                    a.setStatsDirty();
                }
            }
        }
        public static void update_projectiles(bool paused)
        {
            List<Projectile> RemovePro = new List<Projectile>();
            for (int i = 0; i < Main.MWProjectile.Count; i++)
            {
                Projectile pro = Main.MWProjectile[i];
                bool flag = pro.byWho != null && pro.byWho.isAlive();
                if (flag && pro.asset != null && pro.m_transform != null)
                {
                    Vector3 pos1 = pro.m_transform.position;
                    WorldTile tile = World.world.GetTile((int)pos1.x, (int)pos1.y);
                    if (tile != null)
                    {
                        bool targetReached = false;
                        // if (flag && pro.byWho.isActor() && pro.byWho.a.IsID("Squash"))
                        // {
                        //     pro.byWho.a.currentTile = tile;
                        //     pro.m_transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                        //     pro.byWho.a.transform.position = pos1;
                        //     pro.byWho.a.currentPosition.Set(pos1.x, pos1.y);
                        //     pro.byWho.a.dirty_position = true;
                        //     pro.byWho.a.hitboxZ = pro.byWho.a.asset.defaultZ;
                        // }
                        if (flag && Main.projectiles.Contains(pro.asset.id))
                        {
                            if (pro.targetObject != null && pro.targetObject.isAlive())
                            {
                                Vector3 pos2 = pro.targetObject.currentPosition;
                                if (Toolbox.Dist(pos2.x, pos2.y + pro.targetObject.getZ(), pos1.x, pos1.y) < pro.byWho.stats[S.area_of_effect] + pro.targetObject.stats[S.size])
                                {                                    
                                    if(pos1!=null&&pro!=null)
                                    {targetReached = pro.checkHit(pos1);}

                                }
                            }
                            if (!targetReached && !pro.asset.parabolic && tile != null)
                            {
                                targetReached = pro.checkHit(pos1);
                                WorldTile byTile = World.world.GetTile((int)pro.byWho.currentPosition.x, (int)pro.byWho.currentPosition.y);
                                foreach (string id in new List<string> { ST.mountains, ST.snow_block})
                                {
                                    if ((byTile == null || byTile.Type.id != id) && tile.Type.id == id)
                                    {
                                        targetReached = true;
                                        MusicBox.playSound("event:/SFX/HIT/HitGeneric", tile, false, true);
                                        break;
                                    }
                                }
                            }
                        }
                        if (targetReached)
                        {
                            pro.vecTarget = pos1;
                            pro.vecTargetZ = new Vector3(pro.vecTarget.x, pro.vecTarget.y + pro.targetZ);
                            pro.targetReached();
                            RemovePro.Add(pro);
                        }
                    }
                }
                else { RemovePro.Add(pro); }
            }
            foreach (Projectile pro in mw_harmony_projectiles.CattailStab)
            {
                if (!RemovePro.Contains(pro))
                {
                    bool targetReached = false;
                    Vector3 pos1 = pro.m_transform.position;
                    if (pro.byWho != null && pro.byWho.isAlive())
                    {
                        if (pro.targetObject != null && pro.targetObject.isAlive())
                        {
                            Vector3 pos2 = pro.targetObject.currentPosition;
                            if (Toolbox.Dist(pos2.x, pos2.y + pro.targetObject.getZ(), pos1.x, pos1.y) < pro.byWho.stats[S.area_of_effect] + pro.targetObject.stats[S.size])
                            {
                                targetReached = pro.checkHit(pos1);
                            }
                            if (!targetReached)
                            {
                                float pAngle = Toolbox.getAngle(pos1.x, pos1.y, pos2.x, pos2.y);
                                float pSpeed = pro.speed * World.world.getCurElapsed();
                                float xst = Mathf.Cos(pAngle) * pSpeed;
                                float yst = Mathf.Sin(pAngle) * pSpeed;
                                Vector3 pVector = new Vector3(pos1.x + xst, pos1.y + yst, pos1.z);
                                if (xst > Toolbox.Dist(pos1.x, 0f, pos2.x, 0f))
                                {
                                    pVector = new Vector3(pos2.x, pos1.y, pos1.z);
                                }
                                if (yst > Toolbox.Dist(0f, pos1.y, 0f, pos2.y))
                                {
                                    pVector = new Vector3(pos1.x, pos2.y, pos1.z);
                                }
                                pro.m_transform.position = pVector;
                                pro.m_transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Toolbox.getAngle(pos1.x, pos1.y, pos2.x, pos2.y) * Mathf.Rad2Deg));
                                pro.currentPosition = pVector;
                                if (Config.shadowsActive && !string.IsNullOrEmpty(pro.asset.texture_shadow))
                                {
                                    if (pro.shadow == null)
                                    {
                                        pro.shadow = World.world.createShadow(pro, pro.asset.texture_shadow);
                                    }
                                    pro.shadow.m_transform.localPosition = pVector;
                                    pro.shadow.m_transform.rotation = pro.m_transform.rotation;
                                    pro.shadow.m_transform.localScale = pro.transform.localScale;
                                }
                                else if (pro.shadow != null)
                                {
                                    UnityEngine.Object.Destroy(pro.shadow.gameObject);
                                    pro.shadow = null;
                                }
                            }
                        }
                        else
                        {
                            pro.targetObject = pro.byWho.FindEnemyActor(pos1);
                            if (pro.targetObject == null || !pro.targetObject.isAlive()) { targetReached = true; }
                        }
                    }
                    else { targetReached = true; }
                    if (targetReached)
                    {
                        pro.vecTarget = pos1;
                        pro.vecTargetZ = new Vector3(pro.vecTarget.x, pro.vecTarget.y + pro.targetZ);
                        pro.targetReached();
                        RemovePro.Add(pro);
                    }
                }
            }
            foreach (Projectile pro in RemovePro)
            {
                Main.MWProjectile.Remove(pro);
                mw_harmony_projectiles.CattailStab.Remove(pro);
            }
        }
    }
}