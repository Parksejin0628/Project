using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_Loading : UI_Popup
    {
        enum Images
        {
            FadeImage,
        }

        enum TMPs
        {
            RecordTxt,
        }

        public float fadeTime = 0;
        public float delayTime = 0;

        public Coroutine co;

        public override void Init()
        {
            fadeTime = Managers.Scene.loadingFadeTime;
            delayTime = Managers.Scene.loadingDelayTime;

            Bind<Image>(typeof(Images));

            GetImage((int)Images.FadeImage).color = Extension.GetChangeAlpha(GetImage((int)Images.FadeImage).color, 0);
            GetImage((int)Images.FadeImage).gameObject.SetActive(false);
        }

        public void OnStartFade(Define.Scene changeScene)
        {
            if (co != null)
            {
                StopCoroutine(co);
            }
            co = StartCoroutine(CoFade(changeScene));
        }

        private IEnumerator CoFade(Define.Scene changeScene)
        {
            //사전 조건
            GetImage((int)Images.FadeImage).gameObject.SetActive(true);

            yield return StartCoroutine(Extension.Co_FadePlay(null, GetImage((int)Images.FadeImage), Extension.Ease.EaseOutCubic, fadeTime, 0, 1, isRealTime: true));
            yield return Managers.Scene.delayWfs;
            yield return StartCoroutine(Extension.Co_FadePlay(null, GetImage((int)Images.FadeImage), Extension.Ease.EaseOutCubic, fadeTime, 1, 0, isRealTime: true));

            //사후 조건
            GetImage((int)Images.FadeImage).gameObject.SetActive(false);
            co = null;
        }
    }
}
