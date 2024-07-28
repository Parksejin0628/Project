using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_ExampleSubItem : UI_Base
    {
        enum Images
        {
            HPBlock
        }

        public override void Init()
        {
            Bind<Image>(typeof(Images));
        }
    }
}
