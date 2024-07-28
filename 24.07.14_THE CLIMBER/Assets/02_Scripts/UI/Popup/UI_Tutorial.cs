using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_Tutorial : UI_Popup
    {
        enum Images
        {
            Block,
        }

        enum Buttons
        {
            //HideButton
        }

        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Button>(typeof(Buttons));

            GetImage(Images.Block.ToInt()).gameObject.BindEvent(_ => Managers.UI.ClosePopupUI(this));
            //GetButton(Buttons.HideButton.ToInt()).gameObject.BindEvent(_ => Managers.UI.ClosePopupUI(this));
        }
    }
}
