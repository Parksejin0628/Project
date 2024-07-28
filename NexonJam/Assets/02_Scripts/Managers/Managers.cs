using UnityEngine;

namespace DefaultSetting
{
    public class Managers : MonoBehaviour
    {
        static Managers s_instance; // 유일성이 보장된다
        static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다


        DataManager _data;
        GameManager _game;
        InputManager _input;
        PoolManager _pool;
        ResourceManager _resource;
        SceneManagerEx _scene;
        SoundManager _sound;
        UIManager _ui;
        ScreenManager _video;
        NewUIManager _newUIManager;

        public static DataManager Data
        {
            get
            {
                if (Instance._data == null)
                    Instance._data = GetOrMakeManager<DataManager>();

                return Instance._data;
            }
        }
        public static GameManager Game
        {
            get
            {
                if (Instance._game == null)
                    Instance._game = GetOrMakeManager<GameManager>();

                return Instance._game;
            }
        }
        public static InputManager Input
        {
            get
            {
                if (Instance._input == null)
                    Instance._input = GetOrMakeManager<InputManager>();

                return Instance._input;
            }
        }
        public static PoolManager Pool
        {
            get
            {
                if (Instance._pool == null)
                    Instance._pool = GetOrMakeManager<PoolManager>();

                return Instance._pool;
            }
        }
        public static ResourceManager Resource
        {
            get
            {
                if (Instance._resource == null)
                    Instance._resource = GetOrMakeManager<ResourceManager>();

                return Instance._resource;
            }
        }
        public static SceneManagerEx Scene
        {
            get
            {
                if (Instance._scene == null)
                    Instance._scene = GetOrMakeManager<SceneManagerEx>();

                return Instance._scene;
            }
        }
        public static SoundManager Sound
        {
            get
            {
                if (Instance._sound == null)
                    Instance._sound = GetOrMakeManager<SoundManager>();

                return Instance._sound;
            }
        }
        public static UIManager UI
        {
            get
            {
                if (Instance._ui == null)
                    Instance._ui = GetOrMakeManager<UIManager>();

                return Instance._ui;
            }
        }
        public static ScreenManager Video
        {
            get
            {
                if (Instance._video == null)
                    Instance._video = GetOrMakeManager<ScreenManager>();

                return Instance._video;
            }
        }



        //-------------------------------------------------
        TestManager _test;
        public static TestManager Test
        {
            get
            {
                if (Instance._test == null)
                    Instance._test = GetOrMakeManager<TestManager>();

                return Instance._test;
            }
        }

        public static NewUIManager NewUI
        {
            get
            {
                if (Instance._newUIManager == null)
                    Instance._newUIManager = GetOrMakeManager<NewUIManager>();

                return Instance._newUIManager;
            }
        }

        void Start()
        {
            Init();
            if (this != Instance)
                Destroy(gameObject);
        }

        private void Update()
        {
            Input.OnUpdate();
            Video.OnUpdate();
        }

        static void Init()
        {
            if (s_instance == null)
            {
#if !DISABLESTEAMWORKS
            //TODO: ExSteamManager 이런거 만들어서 Init으로 넣기(통일)
            if (SteamManager.Initialized)
            {
                string name = SteamFriends.GetPersonaName();
                Debug.Log(name);
            }
#endif
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    GameObject load = Resources.Load<GameObject>("Prefabs/@Managers");
                    go = Instantiate(load);
                    go.name = "@Managers";
                }

                s_instance = go.GetComponent<Managers>();
                DontDestroyOnLoad(go);

                Data.Init();
                Pool.Init();
                Sound.Init();
                Game.Init();
                Input.Init();
                UI.Init();
                Scene.Init();
                Video.Init();

                Test.Init();

                //EventSystem
                go = GameObject.Find("EventSystem");
                if (go == null)
                {
                    go = GameObject.Find("@EventSystem");
                    if (go == null)
                    {
                        DontDestroyOnLoad(Resource.Instantiate("@EventSystem"));
                    }
                }
                else
                {
                    go.name = "@EventSystem";
                    DontDestroyOnLoad(go);
                }

                CheckIntegrity();
            }
        }

        //무결성 확인
        public static void CheckIntegrity()
        {
            //print("무결성 확인할 요소가 없음!");
        }

        public static void Clear()
        {
            Sound.Clear();
            Scene.Clear();
            UI.Clear();
            Pool.Clear();
        }

        public static T GetOrMakeManager<T>(GameObject go = null) where T : Component
        {
            if (go == null)
                go = Instance.gameObject;

            T t = go.GetComponentInChildren<T>();
            if (t == null)
            {
                GameObject temp = new GameObject(typeof(T).Name);
                t = temp.AddComponent<T>();
                temp.transform.parent = go.transform;
            }
            return t;
        }
    }
}
