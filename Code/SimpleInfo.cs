using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace magic_world.Code
{
    internal abstract class SimpleInfo : MonoBehaviour
    {
        public GameObject disp;
        public GameObject info;
        public Text object_name;

        public virtual void load_obj(object obj, string value, string icon_path = "")
        {
            name = object_name.text;
        }
    }
    internal class SimpleCreatureInfo : SimpleInfo
    {
        public StatBar health_bar;
        public Text value_text;
        public Image value_icon;

        public override void load_obj(object obj, string value, string icon_path = "")
        {
            Actor actor = (Actor)obj;
            object_name.text = actor.getName();
            disp.GetComponent<UiUnitAvatarElement>().show_banner_clan = false;
            disp.GetComponent<UiUnitAvatarElement>().show_banner_kingdom = false;
            disp.GetComponent<UiUnitAvatarElement>().show(actor);
            value_text.text = value;
            health_bar.setBar(actor.data.health, actor.getMaxHealth(), $"/{Toolbox.formatNumber(actor.getMaxHealth())}");

            if (string.IsNullOrEmpty(icon_path))
            {
                value_icon.sprite =
                    SpriteTextureLoader.getSprite("ui/icons/iconFavoriteStar");
            }
            else
            {
                value_icon.sprite = SpriteTextureLoader.getSprite(icon_path);
            }

            base.load_obj(obj, value);
        }
    }

    // internal class SimpleCultibookInfo : SimpleInfo
    // {
    //     public Text author_text;
    //     public Text value_text;
    //     public Image value_icon;
    //     public Text pop_text;
    //     public Button cultibook_button;

    //     public override void load_obj(object obj, string value, string icon_path = "")
    //     {
    //         Cultibook cultibook = (Cultibook)obj;
    //         object_name.text = cultibook.name;
    //         pop_text.text = Toolbox.formatNumber(cultibook.cur_users);
    //         value_text.text = value;
    //         cultibook_button.OnHover(() =>
    //         {
    //             Tooltip.show(obj, Constants.Core.mod_prefix + "cultibook", new TooltipData
    //             {
    //                 tip_name = cultibook.id
    //             });
    //         });
    //         cultibook_button.OnHoverOut(Tooltip.hideTooltip);

    //         if (string.IsNullOrEmpty(icon_path))
    //         {
    //             value_icon.sprite =
    //                 SpriteTextureLoader.getSprite("ui/icons/iconFavoriteStar");
    //         }
    //         else
    //         {
    //             value_icon.sprite = SpriteTextureLoader.getSprite(icon_path);
    //         }

    //         base.load_obj(obj, value, icon_path);
    //     }
    // }
}