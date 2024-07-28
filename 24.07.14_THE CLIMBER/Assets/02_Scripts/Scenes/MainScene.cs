namespace DefaultSetting
{
    public class MainScene : BaseScene
    {
        protected override void Init()
        {
            base.Init();
            SceneType = Define.Scene.Main;
            Managers.UI.ShowSceneUI<UI_MainScene>();

            Managers.Sound.Play(Managers.Data.MstMaster.BgmData.TitleBgm, Define.Sound.Bgm);
        }

        public override void Clear()
        {

        }
    }
}
