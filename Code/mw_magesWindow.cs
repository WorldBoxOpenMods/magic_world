using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using magic_world.Utils;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace magic_world
{
    public class mw_magesWindow : WindowListBase<PrefabUnitElement, Actor>
    {
        public static Text contentText;
        public static int Buttons = -1;
        public static int Buttons2 = 1;
        public static float NYJG;
        public static string wid = "mw_magesWindow";
        public static ScrollWindow magesWindow;
        public static StatBar MPBar = new StatBar();

        public static void init()
        {

            Main.easyTranslate("cn", "mw_magesWindow", "法师排行榜");
            Main.easyTranslate("cz", "mw_magesWindow", "法师排行榜");
            Main.easyTranslate("en", "mw_magesWindow", "法师排行榜");
            Main.easyTranslate("cn", "mwmagic_power", "魔力");
            Main.easyTranslate("cz", "mwmagic_power", "魔力");
            Main.easyTranslate("en", "mwmagic_power", "魔力");
            pvz_ui.NewWindow("mw_magesWindow", 0, "mw_magesWindow", true);


            GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/MWHelper/Background/Scroll View").SetActive(true);
            GameObject SHAuxInventoryContent = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/mw_magesWindow/Background/Scroll View/Viewport/Content");
            GameObject contentComponent = pvz_ui.CustomWindowObjects[wid];
            GameObject UIG = NCMS.Utils.GameObjects.FindEvenInactive("MW_UIG");
            float ngjg = pvz_ui.CustomWindowTexts[wid].preferredHeight;
            NYJG = pvz_ui.CustomWindowTexts[wid].preferredHeight - ngjg;


            float MinY = ((pvz_ui.CustomWindowTexts[wid].preferredHeight / 2) + 30) * -1;
            NewLB("magic_power");

            NewOptimizeSet("DragFloatSet", "other.magic_power", contentComponent, null, null);
            NewOptimizeSet("AutoCleanJGSet", "other.magic_power", contentComponent, null, null);
            NewOptimizeSet("AutoSaveJGSet", "other.magic_power", contentComponent, null, null);
            NewOptimizeSet("MultipleSet", "other.magic_power", contentComponent, null, null);
            NewOptimizeSet("SAMSizeSet", "other.magic_power", contentComponent, null, null);


            // pvz_ui.CustomTextColors.Add(wid, "#FFFFFF");

            wid.RTF();

        }
        public static void NewLB(string id)
        {
            GodPower NewL = new GodPower
            {
                id = "mw" + id,
                name = "mw" + id,
                unselectWhenWindow = true,
                toggle_action = new PowerToggleAction((pPower) => action())
            };
            AssetManager.powers.add(NewL);
            var NewB = PowerButtons.CreateButton(
            "mw" + id,
            Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.other.{id}.png"),
            LocalizedTextManager.getText("mw" + id, null),
            LocalizedTextManager.getText("mw" + id, null),
            new Vector2(120, 90 - (Buttons2 * 32)),
            ButtonType.Click, pvz_ui.CustomWindowObjects[wid].transform,
            null
            );
            Reflection.SetField<GodPower>(NewB, "godPower", NewL);
            NewB.type = PowerButtonType.Special;
            Buttons2++;
        }
        public static void action()
        {
            Debug.Log("啥也没");

        }
        public static void NewOptimizeSet(string id, string icon, GameObject CT, UnityAction call, UnityAction call2)
        {
            Buttons++;
            GameObject UIG = NCMS.Utils.GameObjects.FindEvenInactive("MW_UIG");
            var nesmdmyIG1 = GameObject.Instantiate(UIG, CT.transform);
            nesmdmyIG1.transform.localPosition = new Vector2(0f, NYJG * -10f - (Buttons * 40f));
            var OPIcon = new GameObject("MW" + id + "Icon");
            var OPRTF = OPIcon.AddComponent<RectTransform>();
            OPIcon.AddComponent<CanvasRenderer>();
            OPIcon.AddComponent<Image>().sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.{icon}.png");
            OPIcon.transform.SetParent(CT.transform);
            OPIcon.transform.localScale = Vector3.one;
            OPRTF.localPosition = new Vector3(-80f, NYJG * 51.69484f - (Buttons * 40f));
            OPRTF.sizeDelta = new Vector2(25, 25);
            var BVC = new Vector3(10f, NYJG * -0.5431562f - (Buttons * 40f) - 9f);
            var BVC2 = new Vector3(44f, NYJG * -0.5431562f - (Buttons * 40f) - 9f);
            var BVC3 = new Vector3(75f, NYJG * -0.5431562f - (Buttons * 40f) - 9f);
            GameObject OPRef = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/MWHelper/Background/Name");
            GameObject OPObj = GameObject.Instantiate(OPRef, CT.transform);
            OPObj.SetActive(true);
            Text OPText = OPObj.GetComponent<Text>();
            OPText.text = "名字";
            OPText.supportRichText = true;
            OPText.transform.SetParent(CT.transform);

            var OPObjRTF = OPObj.GetComponent<RectTransform>();
            OPObjRTF.position = new Vector3(0, 0, 0);
            OPObjRTF.sizeDelta = new Vector2(25, 25);
            OPObjRTF.localPosition = new Vector3(-42f, NYJG * -0.5431562f - (Buttons * 40f) - 10f);

            MPBar = pvz_ui.NewBar("magic_power", "ui/tab", "#99DB1E", "/" + 100, (float)20, 100,
            wid.GetObj(), new Vector3(41f, NYJG * -10f - (Buttons * 40f) - 3f), new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));
            // Main.OPText.Add(id, OPText); Main.OPSId.Add(id);




            var a = NCMS.Utils.PowerButtons.CreateButton(S.intelligence, NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/GameResources/ui/UIB2.png"),
            "智力", "智力", BVC, ButtonType.Click, CT.transform, call);
            a.button.GetComponent<Image>().sprite = NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/GameResources/ui/smdmy.png");

            var b = NCMS.Utils.PowerButtons.CreateButton("mwmagic_power", NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/GameResources/ui/UIB2.png"),
            "魔法", "魔法", BVC2, ButtonType.Click, CT.transform, call2);
            b.button.GetComponent<Image>().sprite = NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/GameResources/ui/smdmy.png");

            var c = NCMS.Utils.PowerButtons.CreateButton("element", NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/GameResources/ui/UIB2.png"),
            "元素偏好", "魔法", BVC3, ButtonType.Click, CT.transform, call2);
            c.button.GetComponent<Image>().sprite = NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/GameResources/ui/smdmy.png");
            // NewUI.createActorUI(getObjects().GetRandom(), a.gameObject, new Vector3(40, 0, 0));
            Vector3 Size = new(20f, 10f);
            a.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = Size;
            a.gameObject.GetComponent<RectTransform>().sizeDelta = Size;
            b.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = Size;
            b.gameObject.GetComponent<RectTransform>().sizeDelta = Size;
            c.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = Size;
            c.gameObject.GetComponent<RectTransform>().sizeDelta = Size;

            // NewText("1", "", a.gameObject.transform, new Vector3(-10f, NYJG * -0.5431562f - (Buttons * 40f) - 9f), new Vector3(20f, 10f));
            // NewText("2", "", b.gameObject.transform, new Vector3(24f, NYJG * -0.5431562f - (Buttons * 40f) - 9f), new Vector3(20f, 10f));
            // NewText("3", "", c.gameObject.transform, new Vector3(55f, NYJG * -0.5431562f - (Buttons * 40f) - 9f), new Vector3(20f, 10f));
            NewText("1", "", a.gameObject.transform, new Vector3(0, 0, 0), new Vector3(-1, -1));
            NewText("2", "", b.gameObject.transform, new Vector3(0, 0, 0), new Vector3(0.7f, 0.7f, 1f));
            NewText("3", "", c.gameObject.transform, new Vector3(0, 0, 0), new Vector3(0.7f, 0.7f, 1f));



        }
        public static Text NewText(string text, string color, Transform parentr, Vector3 position, Vector3 size, GameObject MObj = null)
        {
            if (MObj == null)
            {
                GameObject MRef = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/MWHelper/Background/Name");
                MObj = GameObject.Instantiate(MRef, parentr);
            }
            MObj.SetActive(true);
            Text MText = MObj.GetComponent<Text>();
            if (!string.IsNullOrEmpty(color))
            {
                text = $"<color={color}><b>{text}</b></color>";
            }
            MText.text = text;
            MText.supportRichText = true;

            // 根据文本内容长度调整 Text 的宽度
            RectTransform textRTF = MText.GetComponent<RectTransform>();
            textRTF.sizeDelta = new Vector2(MText.preferredWidth, textRTF.sizeDelta.y);

            MText.transform.SetParent(parentr);
            var MObjRTF = MObj.GetComponent<RectTransform>();
            if (size != new Vector3(-1, -1)) { MObjRTF.localScale = size; }
            MObjRTF.position = new Vector3(0f, 0f, 0f);
            MObjRTF.localPosition = position;
            return MText;
        }
        public static List<Actor> getObjects()
        {
            _temp_list_actor.Clear();
            foreach (Actor actor in Main.Actor_Magic.Keys)
            {
                if (actor.Any())
                {
                    _temp_list_actor.Add(actor);
                }
            }
            return _temp_list_actor;
        }

        // Token: 0x06001ACD RID: 6861 RVA: 0x00014CF9 File Offset: 0x00012EF9
        public static int sortByKingdom(Actor pActor1, Actor pActor2)
        {
            return pActor2.kingdom.CompareTo(pActor1.kingdom);
        }

        // Token: 0x06001ACE RID: 6862 RVA: 0x000DD370 File Offset: 0x000DB570
        public static int sortByAge(Actor pActor1, Actor pActor2)
        {
            return pActor2.getAge().CompareTo(pActor1.getAge());
        }

        // Token: 0x06001ACF RID: 6863 RVA: 0x00014D0C File Offset: 0x00012F0C
        public static int sortByLevel(Actor pActor1, Actor pActor2)
        {
            return pActor2.data.level.CompareTo(pActor1.data.level);
        }

        // Token: 0x06001AD0 RID: 6864 RVA: 0x00014D29 File Offset: 0x00012F29
        public static int sortByKills(Actor pActor1, Actor pActor2)
        {
            return pActor2.data.kills.CompareTo(pActor1.data.kills);
        }

        // Token: 0x04001F51 RID: 8017
        private static List<Actor> _temp_list_actor = new List<Actor>();
    }

}
