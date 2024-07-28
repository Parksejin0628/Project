using DefaultSetting.Utility;
using UnityEngine;

namespace DefaultSetting
{
    [CreateAssetMenu]
    public class MstMaster : ScriptableObjectEx
    {
        [field: SerializeField] public MstLocalizeDataScript MstLocalizeDataAsset { get; set; }
        [field: SerializeField] public BgmSc BgmData { get; set; }
        [field: SerializeField] public PlayerSoundSc PlayerSoundData { get; set; }

        public override void AutoFind()
        {
#if UNITY_EDITOR
            this.AutoLoadAsset();
#endif
        }
    }
}
