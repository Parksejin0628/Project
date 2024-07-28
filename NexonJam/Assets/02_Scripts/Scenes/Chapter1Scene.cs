namespace DefaultSetting
{
    public class Chapter1Scene : BaseScene
    {
        protected override void Init()
        {
            base.Init();
            SceneType = Define.Scene.Chapter1;
            Managers.UI.ShowSceneUI<UI_IngameScene>();

            Managers.Sound.Play(Managers.Data.MstMaster.BgmData.InGameBgm, Define.Sound.Bgm);
        }

        public override void Clear()
        {

        }
    }
}
