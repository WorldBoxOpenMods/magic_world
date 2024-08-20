using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using magic_world.Utils;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace magic_world.Code
{
    public class mw_update_log
    {
        public static string wid = "Window_Updatelog";
        public static int w_count = 5;
        public static void init()
        {
            for (int i = 1; i < w_count + 1; i++)
            {
                int next = i + 1;
                string id = wid + i;
                var window = pvz_ui.NewWindow(id, 0, "MWUpdatelog" + i, true);
                if (i != w_count)
                {
                    var pButton = NCMS.Utils.PowerButtons.CreateButton($"Updatelog" + next, Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.other.modder.png"),
                    "催更树老弟", "催更树老弟", new Vector2(117.8f, 82), ButtonType.Click, window.transform, () => Windows.ShowWindow(wid + next));
                    pButton.button.GetComponent<Image>().sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.other.nextpage.png");
                }
                id.RTF();
            }
        }
    }
}