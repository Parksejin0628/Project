using DefaultSetting.Utility;
using UnityEngine;

namespace DefaultSetting
{

    [CreateAssetMenu(fileName = "BgmData", menuName = "Scriptable Object/Bgm Data", order = 0)]
    public class BgmSc : ScriptableObjectEx
    {
        [field: SerializeField] public AudioClip TitleBgm { get; set; }
        [field: SerializeField] public AudioClip InGameBgm { get; set; }

        public override void AutoFind()
        {
#if UNITY_EDITOR
            this.AutoLoadAsset();
#endif
        }
    }
}
