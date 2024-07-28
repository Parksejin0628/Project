using UnityEngine;

namespace DefaultSetting
{
    public class UI_ExampleWorldSpace : UI_Base
    {
        enum GameObjects
        {
            HPBar
        }

        public override void Init()
        {
            Bind<GameObject>(typeof(GameObjects));
        }
    }
}