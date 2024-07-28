using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    public class ScriptableObjectEx : ScriptableObject
    {
        [ContextMenu("Auto Find")]
        public void CallAutoFind()
        {
            AutoFind();
        }

        public virtual void AutoFind() { Debug.Log("AutoFind not override"); }
    }
}
