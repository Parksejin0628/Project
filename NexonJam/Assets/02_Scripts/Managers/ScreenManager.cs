using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField]
        private List<Resolution> resolutions = new List<Resolution>();
        public List<Resolution> Resolutions
        {
            get
            {
                return resolutions;
            }
        }
        public FullScreenMode screenMode = FullScreenMode.FullScreenWindow;
        [ReadOnly] public int resolutionNum = 0;
        [SerializeField] private CursorLockMode defaultCursorLockMode = CursorLockMode.None;

        public bool isLetterBox = true;
        public float letterboxWidthScale = 16;
        public float letterboxHeightScale = 9;
        public Color backgroundColor;

        //스크린 변경을 감지하는 코드
        [SerializeField, ReadOnly]
        private int beforeScreenWidth = 0;
        [SerializeField, ReadOnly]
        private int beforeScreenHeight = 0;

        public void Init()
        {
            beforeScreenWidth = Screen.width;
            beforeScreenHeight = Screen.height;

            //커서 설정 확인
            SetDefaultLockCursor();
            FindResolution();
            Managers.Video.CheckLetterBox();
        }

        public void OnUpdate()
        {
            CheckChangeScreen();
        }

        public void CheckLetterBox()
        {
            //사전 조건
            if (!isLetterBox)
                return;

            //로직
            SetLetterBox();


            void SetLetterBox()
            {
                Camera camera = Camera.main;
                Rect rect = camera.rect;
                float scaleheight = ((float)Screen.width / Screen.height) / ((float)letterboxWidthScale / letterboxHeightScale); // (가로 / 세로)
                float scalewidth = 1f / scaleheight;
                if (scaleheight < 1)
                {
                    rect.width = 1;
                    rect.height = scaleheight;
                    rect.x = 0;
                    rect.y = (1f - scaleheight) / 2f;
                }
                else
                {
                    rect.x = (1f - scalewidth) / 2f;
                    rect.y = 0;
                    rect.width = scalewidth;
                    rect.height = 1;
                }
                camera.rect = rect;
            }
        }

        public void SetDefaultLockCursor()
        {
            //기본값 설정
            if (!Managers.Data.CustomPrefsHasKey(Define.IS_CURSOR_LOCK_KEY))
            {
                Cursor.lockState = defaultCursorLockMode;
                return;
            }

            //저장된 값 설정
            int isLock = Managers.Data.CustomGetPrefsInt(Define.IS_CURSOR_LOCK_KEY, 1);
            if (isLock == 1)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        void FindResolution()
        {
            resolutions = new List<Resolution>();
            foreach (var item in Screen.resolutions)
            {
                if (item.width < 1200)
                    continue;

                if ((float)item.width / item.height < 1.4f)
                    continue;

                if ((float)item.width / item.height > 2f)
                    continue;


                resolutions.Add(item);

                //if (item.width == 1280 && item.height == 720)
                //    resolutions.Add(item);

                //if (item.width == 1366 && item.height == 768)
                //    resolutions.Add(item);

                //if (item.width == 1920 && item.height == 1080)
                //    resolutions.Add(item);
            }
        }

        public void SetResolution()
        {
            if (Managers.Data.playState == Define.PlayState.Demo_Web)
                return;

            Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode, resolutions[resolutionNum].refreshRateRatio);
        }

        public void SetLockMouse(bool isLock)
        {
            Managers.Data.CustomSetPrefsInt(Define.IS_CURSOR_LOCK_KEY, isLock == true ? 1 : 0);

            if (isLock == true)
                Cursor.lockState = CursorLockMode.Confined;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        public void SetScreenMode(bool isFullScreen)
        {
            screenMode = isFullScreen == true ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }


        public void CheckChangeScreen()
        {
            if ((Screen.width != beforeScreenWidth) || (Screen.height != beforeScreenHeight))
            {
                print("화면 변화 이벤트 발생");
                OnChangeScreenEvent();
                beforeScreenWidth = Screen.width;
                beforeScreenHeight = Screen.height;
            }

            void OnChangeScreenEvent()
            {
                CheckLetterBox();
            }
        }
    }
}
