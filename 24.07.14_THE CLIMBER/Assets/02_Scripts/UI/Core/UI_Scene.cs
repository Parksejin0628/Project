namespace DefaultSetting
{
    public class UI_Scene : UI_Base
    {
        public override void Init()
        {
            base.Init();
            Managers.UI.SetCanvas(gameObject, false);
        }
    }
}
