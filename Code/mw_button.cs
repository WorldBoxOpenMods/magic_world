using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;

namespace magic_world.Code
{
  public class mw_button
  {
    public static ToggleIcon PVZPublicIcon;

    public static Dictionary<string, int> PlantSuns = new Dictionary<string, int>();
    public static bool action_Unit(WorldTile pTile, GodPower pPower) { AssetManager.powers.CallMethod("spawnUnit", pTile, pPower); return true; }
    public static bool action_Drop(WorldTile pTile, GodPower pPower) { AssetManager.powers.CallMethod("spawnDrops", pTile, pPower); return true; }
    public static bool action_1(WorldTile pTile, GodPower pPower) { AssetManager.powers.CallMethod("loopWithCurrentBrushPower", pTile, pPower); return true; }
    public static bool action_0(WorldTile pTile, GodPower pPower) { return true; }
    public static bool action_Unit(WorldTile pTile, string pPower) { AssetManager.powers.CallMethod("spawnUnit", pTile, pPower); return true; }
    public static bool action_Drop(WorldTile pTile, string pPower) { AssetManager.powers.CallMethod("spawnDrops", pTile, pPower); return true; }
    public static bool action_1(WorldTile pTile, string pPower) { AssetManager.powers.CallMethod("loopWithCurrentBrushPower", pTile, pPower); return true; }
    public static bool action_0(WorldTile pTile, string pPower) { return true; }
    public static PowerActionWithID PWspawnUnit = new PowerActionWithID(action_Unit);
    public static PowerActionWithID PWspawnDrops = new PowerActionWithID(action_Drop);
    public static PowerActionWithID PWloopWithCurrentBrushPower = new PowerActionWithID(action_1);
    public static PowerActionWithID PWfalse = new PowerActionWithID(action_0);
    public static PowerAction PspawnUnit = new PowerAction(action_Unit);
    public static PowerAction PspawnDrops = new PowerAction(action_Drop);
    public static PowerAction PloopWithCurrentBrushPower = new PowerAction(action_1);
    public static PowerAction Pfalse = new PowerAction(action_0);
    public static void init()
    {
      GameObject line = new GameObject("PVZLine");
      var lineRTF = line.AddComponent<RectTransform>();
      line.AddComponent<CanvasRenderer>();
      line.AddComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/PVZline");
      lineRTF.sizeDelta = new Vector2(6f, 86f);
      var newLine0 = UnityEngine.Object.Instantiate(line, pvz_ui.CustomTabObjs["magic_world"].transform);

      PowerButtons.CreateButton("mw_modder", Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.other.modder.png"),
"modder", "作者简介", new Vector2(72, 18), ButtonType.Click, pvz_ui.CustomTabObjs["magic_world"].transform, static () => Windows.ShowWindow(mw_modder.wid));
      PowerButtons.CreateButton("mw_Updatelog", Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.other.Updatelog.png"),
    "更新日志", "更新日志", new Vector2(108, 18), ButtonType.Click, pvz_ui.CustomTabObjs["magic_world"].transform, () => Windows.ShowWindow( "Window_Updatelog1"));
    PowerButtons.CreateButton("mw_Mages", Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.other.Updatelog.png"),
    "法师排行榜", "排行榜", new Vector2(142, 18), ButtonType.Click, pvz_ui.CustomTabObjs["magic_world"].transform, () => Windows.ShowWindow( mw_magesWindow.wid));

    }

  }
}