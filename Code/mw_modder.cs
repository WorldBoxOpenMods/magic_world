using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using magic_world.Utils;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;

namespace magic_world
{
    public class mw_modder
    {
        public static int Buttons = 0;
        public static string wid = "Window_modder";
        public static void init()
        {
            pvz_ui.NewWindow(wid, 0, "mw_MContentText", true);
            pvz_ui.CustomTextColors.Add(wid, "#FFFFFF");
            NewLJTZ("bilibili", "https://space.bilibili.com/3493140006701370");
            NewLJTZ("qq", "https://qm.qq.com/q/qtnN1CyApO");
            wid.RTF();
        }
        public static void NewLJTZ(string id, string wz)
        {
            GodPower NewL = new GodPower
            {
                id = "mw" + id,
                name = "mw" + id,
                unselectWhenWindow = true,
                toggle_action = new PowerToggleAction((pPower) => Application.OpenURL(@$"{wz}"))
            };
            AssetManager.powers.add(NewL);
            var NewB = PowerButtons.CreateButton(
            "mw" + id,
            Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.other.{id}.png"),
            LocalizedTextManager.getText("mw" + id,null),
            LocalizedTextManager.getText("mw" + id,null),
            new Vector2(120, 110 - (Buttons * 32)),
            ButtonType.Click, pvz_ui.CustomWindows[wid].transform,
            null
            );
            Reflection.SetField<GodPower>(NewB, "godPower", NewL);
            NewB.type = PowerButtonType.Special;
            Buttons++;
        }
    }
}