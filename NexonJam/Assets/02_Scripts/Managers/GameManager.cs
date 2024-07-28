using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
    public class GameManager : MonoBehaviour
    {
        private PlayerController _player;
        public PlayerController Player
        {
            get
            {
                if (_player == null)
                {
                    GameObject playerGo = GameObject.Find("Player");
                    if (playerGo != null)
                    {
                        _player = playerGo.GetComponent<PlayerController>();
                    }
                    else
                    {
                        _player = Managers.Resource.Instantiate("Unit/Player").GetComponent<PlayerController>();
                    }
                }
                return _player;
            }
        }

        public float EntryTime { get; private set; } //입장 시간
        public float CurrentGameTime
        {
            get
            {
                return Time.time - EntryTime;
            }
        }

        public int ClearTime { get; private set; } = -1;
        public bool IsClear { get; private set; } = false;

        [ReadOnly] public Define.Stage currentStage = Define.Stage.Null;
        [ReadOnly] public Define.GameMode currentGameMode = Define.GameMode.Null;

        public void Init() { }

        //게임 시작 전
        public void GameSetting()
        {
            //if (Managers.Scene.CurrentScene?.SceneType == Define.Scene.TestScene)
            //{
            //    Debug.LogError("테스트 씬인데 호출됨");
            //    return;
            //}

            ////사전 조건
            //print("GameSetting");
            ////플레이어 데이터 세팅은 SelectStage에서 진행중
            //getCoinList.Clear();
            //IsClear = false;
            //EntryTime = Time.time;
            //ClearTime = 0;

            ////게임 세팅
            //LoadMap();
            print($"Try Load: Player[{Player?.name}]\n");
            ////Player.GetComponent<PlayerController>().Stat.SetPlayerStat();

            //Managers.Resource.Instantiate("BackgroundScroller", Player.transform.position, Quaternion.identity);
            //Managers.Sound.Play(Managers.Data.MstMaster.BgmData.InGameBgm, Define.Sound.Bgm);

            ////연출
            //Camera.main.GetComponent<CameraController>().ShowGlitchEnterInGame();
        }

        public void OnPlayerDie()
        {
            Debug.Log("플레이어 사망");
            StartCoroutine(CoPlayerDie());
        }

        public IEnumerator CoPlayerDie()
        {
            yield return 0;
            Managers.Scene.LoadScene(Managers.Scene.CurrentScene.SceneType);
        }

        //게임 종료
        public void ClearGame()
        {
            ////클리어
            //print("ClearGame");
            //IsClear = true;
            //ClearTime = Extension.GetTime2Millisecond(CurrentGameTime);

            ////클리어 사전 조건 처리
            //Managers.Sound.Play(Managers.Data.MstMaster.UIData.ClearSound);
            //OpenStage();
            //AddRank();
            //SetStar();

            ////기록은 나중으로
            //CheckAchievement();

            ////일등 기록이 갱신됐을 때에만 호출해야 함.
            //Managers.UI.isRestoreUI = true;
            ////화면 처리
            //Managers.UI.ShowPopupUI<UI_ClearStage>();
        }

        //private void CheckAchievement()
        //{
        //얘도 DataManager에 가야할 것 같은데?
        //}

        public void Clear()
        {
            IsClear = false;
        }
    }
}
