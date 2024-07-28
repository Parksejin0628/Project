using UnityEngine;

namespace DefaultSetting
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] public NewInputManager newInputManager;
        [SerializeField] public LegacyInputManager legacyInputManager;

        public void Init()
        {
            newInputManager = Managers.GetOrMakeManager<NewInputManager>(gameObject);
            legacyInputManager = Managers.GetOrMakeManager<LegacyInputManager>(gameObject);

#if INPUT_TYPE_NEW
            newInputManager.Init();
            legacyInputManager.gameObject.SetActive(false);
#elif INPUT_TYPE_LEGACY
            legacyInputManager.Init();
            newInputManager.gameObject.SetActive(false);
#else
            Debug.LogError("키 설정이 되어 있지 않습니다.");
#endif
        }

        public void OnUpdate()
        {
#if INPUT_TYPE_NEW
            newInputManager.OnUpdate();
#elif INPUT_TYPE_LEGACY
            legacyInputManager.OnUpdate();
#else
            Debug.LogError("키 설정이 되어 있지 않습니다.");
#endif
        }

        public void OnTest()
        {
            //플레이가 테스트인 경우에만 작동
            if (!(Managers.Data.playState == Define.PlayState.Test))
                return;

            Managers.Test.OnTest();
        }
    }
}
