using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultSetting
{
    public class SceneManagerEx : MonoBehaviour
    {
        [field: SerializeField]
        public float loadingFadeTime { get; private set; } = 0.3f;
        [field: SerializeField]
        public float loadingDelayTime { get; private set; } = 0.1f;

        public WaitForSecondsRealtime fadeWfs = null;
        public WaitForSecondsRealtime delayWfs = null;

        public BaseScene CurrentScene { get { return FindFirstObjectByType<BaseScene>(); } }

        public void Init()
        {
            fadeWfs = new WaitForSecondsRealtime(loadingFadeTime);
            delayWfs = new WaitForSecondsRealtime(loadingDelayTime);

            SceneManager.sceneLoaded -= OnLoadSetting;
            SceneManager.sceneLoaded += OnLoadSetting;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLoadSetting;
        }

        public void LoadScene(Define.Scene type)
        {
            StartCoroutine(CoLoadScene(type));
        }

        IEnumerator CoLoadScene(Define.Scene type)
        {
            Managers.UI.LoadingPopup.OnStartFade(type);
            yield return fadeWfs;
            yield return new WaitForSecondsRealtime(0.1f);
            Managers.Clear();
            SceneManager.LoadScene(GetSceneName(type));

        }

        public void OnLoadSetting(Scene scene, LoadSceneMode sceneMode)
        {
            Managers.Video.CheckLetterBox();
        }

        string GetSceneName(Define.Scene type)
        {
            string name = System.Enum.GetName(typeof(Define.Scene), type);
            return name;
        }

        public void Clear()
        {
            CurrentScene?.Clear();
        }
    }
}
