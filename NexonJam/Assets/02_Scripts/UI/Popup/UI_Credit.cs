using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_Credit : UI_Popup
    {
        enum Buttons
        {
            HideButton,
        }

        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));

            GetButton(Buttons.HideButton.ToInt()).gameObject.BindEvent(_ => Managers.UI.ClosePopupUI(this));
        }
    }
}
