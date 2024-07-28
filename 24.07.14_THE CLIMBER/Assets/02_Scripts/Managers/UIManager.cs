using DefaultSetting.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UIManager : MonoBehaviour
    {


        int _order = 1100;

        private UI_Loading _loadingPopup = null;
        public UI_Loading LoadingPopup
        {
            get
            {
                if (_loadingPopup == null)
                {
                    GameObject go = Managers.Resource.Instantiate($"UI/Popup/{typeof(UI_Loading).Name}");
                    go.transform.SetParent(Root.transform);

                    _loadingPopup = Util.GetOrAddComponent<UI_Loading>(go);
                    Canvas canvas = _loadingPopup.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = 10000;
                }
                return _loadingPopup;
            }
            private set
            {
                _loadingPopup = value;
            }
        }

        Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
        UI_Scene _sceneUI = null;

        [HideInInspector]
        public bool isRestoreUI = false;

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Root");
                if (root == null)
                {
                    root = new GameObject { name = "@UI_Root" };
                    DontDestroyOnLoad(root);
                }
                return root;
            }
        }

        public UI_Scene currentScene { get { return _sceneUI; } }
        public UI_Popup currentPopup
        {
            get
            {
                if (_popupStack.Count == 0)
                    return null;
                else
                    return _popupStack.Peek();
            }
        }

        public Action updateLocalizationAction = null;
        public Action updateUIAction = null;

        public void Init()
        {
            updateLocalizationAction = null;
            updateUIAction = null;

            print($"UIManager\nTry Load: Loading PopUp[{LoadingPopup.name}]\n");
        }

        public void SetCanvas(GameObject go, bool sort = true)
        {
            //변경 시 로딩 팝업의 내용도 수정 필요

            //Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
            //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //canvas.overrideSorting = true;

            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.overrideSorting = true;
            // 캔버스 안에 캔버스 중첩 경우 (부모 캔버스가 어떤 값을 가지던 나는 내 오더값을 가지려 할때)


            if (sort)
            {
                canvas.sortingOrder = _order;
                _order++;
            }
            else
            {
                canvas.sortingOrder = 1000;
            }
        }

        public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
            if (parent != null)
                go.transform.SetParent(parent);

            Canvas canvas = go.GetOrAddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            return Util.GetOrAddComponent<T>(go);
        }

        public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
            if (parent != null)
                go.transform.SetParent(parent);

            return Util.GetOrAddComponent<T>(go);
        }

        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            if (CheckThisUI(name, Define.UIType.Popup))
            {
                Debug.Log("같은 UI 실행");
                return null;
            }

            GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
            T sceneUI = Util.GetOrAddComponent<T>(go);
            _sceneUI = sceneUI;

            go.transform.SetParent(Root.transform);

            return sceneUI;
        }

        public T ShowPopupUI<T>(string name = null) where T : UI_Popup
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            if (CheckThisUI(name, Define.UIType.Popup))
            {
                Debug.Log("같은 UI 실행");
                return null;
            }

            GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
            T popup = Util.GetOrAddComponent<T>(go);
            _popupStack.Push(popup);

            go.transform.SetParent(Root.transform);

            return popup;
        }

        /// <summary>
        /// 들어오는 UI가 현재 실행중인 UI인지 확인하는 함수
        /// 같은 이름의 UI를 한번 더 실행하려 하면 true 반환
        /// </summary>
        public bool CheckThisUI(string UIName, Define.UIType uiType)
        {
            bool check = false;

            switch (uiType)
            {
                case Define.UIType.Scene:
                    if (_sceneUI == null || _sceneUI?.name != UIName)
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = _sceneUI.gameObject.name == UIName;
                        break;
                    }
                case Define.UIType.Popup:
                    if (_popupStack.Count == 0)
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = _popupStack.Peek()?.name == UIName;
                        break;
                    }
            }
            return check;
        }

        public void ClosePopupUI(UI_Popup popup)
        {
            if (_popupStack.Count == 0)
                return;

            if (_popupStack.Peek() != popup)
            {
                Debug.Log("Close Popup Failed!");
                return;
            }

            ClosePopupUI();
        }

        public void ClosePopupUI()
        {
            if (_popupStack.Count == 0)
                return;

            UI_Popup popup = _popupStack.Pop();
            popup.CloseSetting();
            Managers.Resource.Destroy(popup.gameObject);
            popup = null;
            _order--;
        }

        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();
        }

        //TODO: 이것도 UpdateLocalization처럼 등록 방식으로 변경하자
        public void UpdateUI()
        {
            if (updateUIAction != null)
                updateUIAction.Invoke();

            //_sceneUI.UpdateUI();
            //foreach (var popup in _popupStack.ToArray())
            //{
            //    popup.UpdateUI();
            //} 
        }

        //구조의 체계화를 위해 Action으로 변경
        public void UpdateLocalized()
        {
            if (updateLocalizationAction != null)
                updateLocalizationAction.Invoke();


            //_sceneUI.UpdateLocalization();
            //foreach (var popup in _popupStack.ToArray())
            //{
            //    popup.UpdateLocalization();
            //}
        }

        public void ShowLog(string text)
        {
            GameObject go = Managers.Resource.Instantiate("UI/LogCanvas", Root.transform);
            go.GetComponentInChildren<LogText>().SetLogTxt(text);
        }

        public void Clear()
        {
            CloseAllPopupUI();
            updateLocalizationAction = null;
            if (_sceneUI != null)
                Managers.Resource.Destroy(_sceneUI.gameObject);
            _sceneUI = null;
        }
    }
}
