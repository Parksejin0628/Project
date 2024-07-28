namespace DefaultSetting
{
    public class InGameScene : BaseScene
    {
        protected override void Init()
        {
            base.Init();
            SceneType = Define.Scene.InGame;
            Managers.UI.ShowSceneUI<UI_IngameScene>();

            Managers.Sound.Play(Managers.Data.MstMaster.BgmData.InGameBgm, Define.Sound.Bgm);
        }

        public override void Clear()
        {

        }
    }
}
