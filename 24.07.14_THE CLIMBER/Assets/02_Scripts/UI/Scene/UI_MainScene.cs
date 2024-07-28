using DefaultSetting.Utility;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_MainScene : UI_Scene
    {
        enum TMPs
        {
            PressedKeyText
        }

        enum Buttons
        {
            GoInGameButton,
        }

        public PlayableDirector director;

        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<TextMeshProUGUI>(typeof(TMPs));

            director = GameObject.FindObjectOfType<PlayableDirector>();
            Managers.Game.Player.transform.Find("UnitSprite").GetComponent<Animator>().SetTrigger("isCutsceneFirst");
            GetButton(Buttons.GoInGameButton.ToInt()).gameObject.BindEvent((_) =>
            {
                director.Play();
                GetTMP(TMPs.PressedKeyText.ToInt()).gameObject.SetActive(false);
                //Managers.Scene.LoadScene(Define.Scene.Ending);
            });

            FadeInAndOutRepeatedly();
        }

        void FadeInAndOutRepeatedly()
        {
            float fadeInDuration = 0.5f; // 페이드 인 지속 시간
            float fadeOutDuration = 0.5f; // 페이드 아웃 지속 시간
            float fadeDelay = 1.0f; // 페이드 사이의 지연 시간
            TextMeshProUGUI textCanvasGroup = GetTMP(TMPs.PressedKeyText.ToInt());

            DOTween.Sequence()
                .Append(textCanvasGroup.DOFade(0, fadeInDuration)) // 투명도를 1로 변경
                .AppendInterval(0.2f) // 페이드 아웃 후 지연
                .Append(textCanvasGroup.DOFade(1, fadeOutDuration)) // 투명도를 0으로 변경
                .AppendInterval(fadeDelay) // 페이드 아웃 후 지연
                .SetLoops(-1, LoopType.Restart); // 무한 반복
        }
    }
}
