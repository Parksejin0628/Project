using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Use1 기존 메소드 호출
//GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);
//Use2 무명 메소드
//GetButton((int)Buttons.PointButton).gameObject.BindEvent((PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
namespace DefaultSetting
{
    public abstract class UI_Base : MonoBehaviour
    {
        protected bool isInit = false;
        [ReadOnly] public List<LocalizedSetting> localizedList = null;

        protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
        public virtual void Init()
        {
            isInit = true;
            Managers.UI.updateUIAction -= UpdateUI;
            Managers.UI.updateUIAction += UpdateUI;
            Managers.UI.updateLocalizationAction -= UpdateLocalization;
            Managers.UI.updateLocalizationAction += UpdateLocalization;
            UpdateLocalization();
        }

        protected void Start()
        {
            if (!isInit)
            {
                isInit = true;
                Init();
            }
        }

        public void CloseSetting()
        {
            Managers.UI.updateUIAction -= UpdateUI;
            Managers.UI.updateLocalizationAction -= UpdateLocalization;
        }

        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _objects.Add(typeof(T), objects);

            for (int i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Util.FindChild(gameObject, names[i], true);
                else
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true);

                if (objects[i] == null)
                    Debug.Log($"Failed to bind({names[i]})");
            }
        }

        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            if (isInit == false)
                Init();

            UnityEngine.Object[] objects = null;
            if (_objects.TryGetValue(typeof(T), out objects) == false)
                return null;

            return objects[idx] as T;
        }

        protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
        protected Text GetText(int idx) { return Get<Text>(idx); }
        protected TextMeshProUGUI GetTMP(int idx) { return Get<TextMeshProUGUI>(idx); }
        protected TMP_Dropdown GetTMP_Dropdown(int idx) { return Get<TMP_Dropdown>(idx); }
        protected Button GetButton(int idx) { return Get<Button>(idx); }
        protected Image GetImage(int idx) { return Get<Image>(idx); }
        protected Slider GetSlider(int idx) { return Get<Slider>(idx); }
        protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }

        public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

            switch (type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case Define.UIEvent.BeginDrag:
                    evt.OnBeginDragHandler -= action;
                    evt.OnBeginDragHandler += action;
                    break;
                case Define.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
                case Define.UIEvent.EndDrag:
                    evt.OnEndDragHandler -= action;
                    evt.OnEndDragHandler += action;
                    break;
                case Define.UIEvent.Enter:
                    evt.OnEnterHandler -= action;
                    evt.OnEnterHandler += action;
                    break;
                case Define.UIEvent.Exit:
                    evt.OnExitHandler -= action;
                    evt.OnExitHandler += action;
                    break;
                case Define.UIEvent.Up:
                    evt.OnUpHandler -= action;
                    evt.OnUpHandler += action;
                    break;
                case Define.UIEvent.Down:
                    evt.OnDownHandler -= action;
                    evt.OnDownHandler += action;
                    break;
            }
        }

        public static void DeleteEvent(GameObject go)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
            evt.OnClickHandler = null;
            evt.OnDragHandler = null;
        }

        public virtual void UpdateUI()
        {

        }

        [ContextMenu("Set LocalizedID")]
        public void SetLocalizedID()
        {
            localizedList = GetComponentsInChildren<LocalizedSetting>(includeInactive: true).ToList();
            string className = GetType().Name;
            foreach (var localized in localizedList)
            {
                localized.SetDefaultID(className);
            }
        }

        public void UpdateLocalization()
        {
            if (localizedList == null)
                return;

            foreach (var localized in localizedList)
            {
                if (localized == null)
                {
                    Debug.LogWarning("존재X, 임의로 재설정합니다.");
                    SetLocalizedID();
                    UpdateLocalization();
                    break;
                }
                localized.UpdateLocalized();
            }
        }
    }

}