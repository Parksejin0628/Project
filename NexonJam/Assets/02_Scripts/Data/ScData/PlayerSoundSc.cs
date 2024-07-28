using DefaultSetting.Utility;
using UnityEngine;

namespace DefaultSetting
{

    [CreateAssetMenu(fileName = "PlayerSoundData", menuName = "Scriptable Object/Player Sound Data", order = 0)]
    public class PlayerSoundSc : ScriptableObjectEx
    {
        [field: SerializeField] public AudioClip PlayerMoveClip { get; private set; }
        [field: SerializeField] public AudioClip PlayerJumpSound { get; private set; }
        [field: SerializeField] public AudioClip PlayerDashSound { get; private set; }
        [field: SerializeField] public AudioClip PlayerHitSound { get; private set; }
        [field: SerializeField] public AudioClip PlayerSelectItemSound { get; private set; }

        public override void AutoFind()
        {
#if UNITY_EDITOR
            this.AutoLoadAsset();
#endif
        }
    }
}
