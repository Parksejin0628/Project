using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_Setting : UI_Popup
    {
        enum Objects
        {
            BGMFillArea,
            EfxFillArea,
        }

        enum Images
        {
            Block,
        }

        enum Buttons
        {
            ShowCreditButton,
            HideButton,
        }

        enum Sliders
        {
            BGMSlider,
            EfxSlider,
        }

        public override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(Objects));
            Bind<Image>(typeof(Images));
            Bind<Slider>(typeof(Sliders));
            Bind<Button>(typeof(Buttons));

            GetImage(Images.Block.ToInt()).gameObject.BindEvent(_ => Managers.UI.ClosePopupUI(this));
            GetButton(Buttons.HideButton.ToInt()).gameObject.BindEvent(_ => Managers.UI.ClosePopupUI(this));

            GetSlider(Sliders.BGMSlider.ToInt()).onValueChanged.AddListener((value) => ChangeSliderValue(Define.Sound.Bgm, value));
            GetSlider(Sliders.EfxSlider.ToInt()).onValueChanged.AddListener((value) => ChangeSliderValue(Define.Sound.Effect, value));

            GetSlider(Sliders.BGMSlider.ToInt()).value = Managers.Sound.GetVolume(Define.Sound.Bgm);
            GetSlider(Sliders.EfxSlider.ToInt()).value = Managers.Sound.GetVolume(Define.Sound.Effect);

            GetButton(Buttons.ShowCreditButton.ToInt()).gameObject.BindEvent(_ => Managers.UI.ShowPopupUI<UI_Credit>());
        }


        public void ChangeSliderValue(Define.Sound type, float value)
        {
            switch (type)
            {
                case Define.Sound.Bgm:
                    GetObject(Objects.BGMFillArea.ToInt()).SetActive(value == 0 ? false : true);
                    break;
                case Define.Sound.Effect:
                    GetObject(Objects.EfxFillArea.ToInt()).SetActive(value == 0 ? false : true);
                    break;
                default:
                    break;
            }
            Managers.Sound.ChangeVolume(type, value);
        }
    }
}
