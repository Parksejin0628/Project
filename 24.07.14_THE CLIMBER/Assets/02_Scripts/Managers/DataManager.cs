using System;
using System.Globalization;
using UnityEngine;

namespace DefaultSetting
{
    public class DataManager : MonoBehaviour
    {
        private string _currentVersion = "Version_0001";
        public string CurrentVersion
        {
            get { return _currentVersion; }
        }

        private MstMaster _mstMaster = null;
        public MstMaster MstMaster
        {
            get
            {
                if (_mstMaster == null)
                {
                    _mstMaster = Managers.Resource.Load<MstMaster>("Data/MstMaster");
                    if (_mstMaster == null)
                    {
                        Debug.LogError("MstMaster 못찾음");
                    }
                }
                return _mstMaster;
            }
        }

        [field: SerializeField] public Define.PlayState playState { get; private set; } = Define.PlayState.Build_PC;
        [field: SerializeField] public Define.PlayPlatform playFlatform { get; private set; } = Define.PlayPlatform.NotSetting;
        [field: SerializeField] public Define.Language currentLanguage { get; private set; } = Define.Language.NotSetting;

        public void Init()
        {
            print($"DataManager\nTry Load: MstMaster[{MstMaster.name}]\n");
            GetLanguage();

            PlayerPrefs.DeleteKey(Define.IS_SHOW_TUTORIAL_KEY);
        }

        public void GetLanguage()
        {

            switch (playFlatform)
            {
                case Define.PlayPlatform.Steam:
                    GetSteamLanguage();
                    return;
                //case Define.PlayPlatform.Stove:
                //    break;
                case Define.PlayPlatform.Zempie:
                    GetLocalLanguage();
                    break;
                case Define.PlayPlatform.OPGG:
                    GetLocalLanguage();
                    return;
                default:
                    //Debug.LogWarning("플랫폼이 잘못되어 있습니다.");
                    break;
            }

            if (!Managers.Data.CustomPrefsHasKey(Define.LANGUAGE_KEY))
            {
                currentLanguage = Define.Language.Korean;
                return;
            }

            string str = Managers.Data.CustomGetPrefsString(Define.LANGUAGE_KEY);
            currentLanguage = (Define.Language)Enum.Parse(typeof(Define.Language), str);

            void GetSteamLanguage()
            {
#if !DISABLESTEAMWORKS
            string steamLang = SteamApps.GetCurrentGameLanguage();
            switch (steamLang)
            {
                case "koreana":
                    currentLanguage = Define.Language.Korean;
                    break;
                case "english":
                    currentLanguage = Define.Language.English;
                    break;
                case "japanese":
                    currentLanguage = Define.Language.Japanese;
                    break;
                default:
                    currentLanguage = Define.Language.English;
                    break;
            }
#else
                Debug.LogWarning("스팀이 아닌데 실행됨.");
#endif

            }

            void GetLocalLanguage()
            {

                string systemLang = CultureInfo.CurrentUICulture.Name;

                if (systemLang.Contains("ko") || systemLang.Contains("KR"))
                {
                    currentLanguage = Define.Language.Korean;
                }
                else if (systemLang.Contains("en"))
                {
                    currentLanguage = Define.Language.English;
                }
                else if (systemLang.Contains("ja") || systemLang.Contains("JP"))
                {
                    currentLanguage = Define.Language.Japanese;
                }
                else
                {
                    currentLanguage = Define.Language.English;
                }
            }
        }
        public void SetLanguage(Define.Language changeLangValue)
        {
            currentLanguage = changeLangValue;
            Managers.Data.CustomSetPrefsString(Define.LANGUAGE_KEY, currentLanguage.ToString());
            Managers.UI.UpdateLocalized();
        }

        [ContextMenu("Reset Data")]
        public void ResetData()
        {
            PlayerPrefs.DeleteAll();
            Managers.Sound.InitVolume();
        }


        #region CustomPrefs
        string MakeFlatformKey(string key)
        {
            return $"{playFlatform}_{key}";
        }


        public void CustomPrefsDeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(MakeFlatformKey(key));
        }
        public bool CustomPrefsHasKey(string key)
        {
            return PlayerPrefs.HasKey(MakeFlatformKey(key));
        }

        public int CustomGetPrefsInt(string key)
        {
            return PlayerPrefs.GetInt(MakeFlatformKey(key));
        }

        public int CustomGetPrefsInt(string key, int defaultData)
        {
            return PlayerPrefs.GetInt(MakeFlatformKey(key), defaultData);
        }

        public void CustomSetPrefsInt(string key, int value)
        {
            PlayerPrefs.SetInt(MakeFlatformKey(key), value);
        }

        public void CustomSetPrefsString(string key, string value)
        {
            PlayerPrefs.SetString(MakeFlatformKey(key), value);
        }

        public string CustomGetPrefsString(string key)
        {
            return PlayerPrefs.GetString(MakeFlatformKey(key));
        }

        public string CustomGetPrefsString(string key, string defaultData)
        {
            return PlayerPrefs.GetString(MakeFlatformKey(key), defaultData);
        }

        public void CustomSetPrefsFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(MakeFlatformKey(key), value);
        }

        public float CustomGetPrefsFloat(string key)
        {
            return PlayerPrefs.GetFloat(MakeFlatformKey(key));
        }

        public float CustomGetPrefsFloat(string key, float defaultData)
        {
            return PlayerPrefs.GetFloat(MakeFlatformKey(key), defaultData);
        }
        #endregion

        #region Achievement
        public bool IsThisAchievementUnlocked(string id)
        {
#if !DISABLESTEAMWORKS
            if (!SteamManager.Initialized)
                return false;

            if (Managers.Game.playFlatform != Define.PlayPlatform.Steam)
            {
                Debug.Log("플랫폼이 스팀X");
                return false;
            }

            SteamUserStats.GetAchievement(id, out bool achievementCompleted);
            //print($"Target Achievement: {id}\n" +
            //    $"status: {achievementCompleted}\n");

            return achievementCompleted;

#else
            Debug.LogWarning("스팀이 아닌데 실행됨.");
            return false;
#endif
        }
        public void UnlockAchievement(string id)
        {
#if !DISABLESTEAMWORKS
            //사전 조건
            if (!SteamManager.Initialized)
                return;

            if (Managers.Game.playFlatform != Define.PlayPlatform.Steam)
            {
                Debug.Log("플랫폼이 스팀X");
                return;
            }

            //클리어 상태라면 실행 안하도록
            SteamUserStats.GetAchievement(id, out bool achievementCompleted);
            if (achievementCompleted)
                return;

            //로직
            SteamUserStats.SetAchievement(id);

            //사후 조건
            SteamUserStats.StoreStats();
            print($"Target Achievement: {id}\n" +
                $"UnLocked\n");
#else
            Debug.LogWarning($"스팀이 아닌데 실행됨.\nAchievement id: {id}\n");
#endif
        }
        public void ClearAchievementStatus(string id)
        {
#if !DISABLESTEAMWORKS
            //사전 조건
            if (!SteamManager.Initialized)
                return;

            if (Managers.Game.playFlatform != Define.PlayPlatform.Steam)
            {
                Debug.Log("플랫폼이 스팀X");
                return;
            }

            //로직
            SteamUserStats.ClearAchievement(id);

            //사후 조건
            print($"Target Achievement: {id}\n" +
                $"UnLocked\n");
#else
            Debug.LogWarning("스팀이 아닌데 실행됨.");
#endif
        }
        public void ClearAllAchievementStatus()
        {
#if !DISABLESTEAMWORKS
            //사전 조건
            if (!SteamManager.Initialized)
                return;

            if (Managers.Game.playFlatform != Define.PlayPlatform.Steam)
            {
                Debug.Log("플랫폼이 스팀X");
                return;
            }

            //로직
            var achievementList = Enum.GetNames(typeof(Define.AchievementList));
            foreach (var item in achievementList)
            {
                ClearAchievementStatus(item);
            }

            //사후 조건
            SteamUserStats.StoreStats();
            print($"All Achievement UnLocked\n");
#else
            Debug.LogWarning("스팀이 아닌데 실행됨.");
#endif
        }
        #endregion

    }
}
