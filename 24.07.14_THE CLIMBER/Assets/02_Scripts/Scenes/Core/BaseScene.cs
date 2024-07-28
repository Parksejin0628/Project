using UnityEngine;


namespace DefaultSetting
{
    public abstract class BaseScene : MonoBehaviour
    {
        public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
        }

        public abstract void Clear();
    }
}