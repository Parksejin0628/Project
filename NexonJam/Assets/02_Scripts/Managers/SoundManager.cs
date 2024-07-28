using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
        private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

        // MP3 Player   -> AudioSource
        // MP3 음원     -> AudioClip
        // 관객(귀)     -> AudioListener

        public void Init()
        {
            GameObject root = GameObject.Find("@Sound");
            if (root == null)
            {
                root = new GameObject { name = "@Sound" };
                Object.DontDestroyOnLoad(root);

                string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
                for (int i = 0; i < soundNames.Length - 1; i++)
                {
                    GameObject go = new GameObject { name = soundNames[i] };
                    _audioSources[i] = go.AddComponent<AudioSource>();
                    go.transform.parent = root.transform;
                }

                _audioSources[(int)Define.Sound.Bgm].loop = true;
            }

            InitVolume();
        }

        public void Clear()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (audioSource.name == System.Enum.GetName(typeof(Define.Sound), Define.Sound.Effect))
                    continue;

                audioSource.clip = null;
                audioSource.Stop();
            }
            _audioClips.Clear();
        }

        public void InitVolume()
        {
            if (PlayerPrefs.HasKey("BgmVolume"))
                _audioSources[(int)Define.Sound.Bgm].volume = PlayerPrefs.GetFloat("BgmVolume");
            else
                _audioSources[(int)Define.Sound.Bgm].volume = 1;

            if (PlayerPrefs.HasKey("EffectVolume"))
                _audioSources[(int)Define.Sound.Effect].volume = PlayerPrefs.GetFloat("EffectVolume");
            else
                _audioSources[(int)Define.Sound.Effect].volume = 1;
        }

        public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            AudioClip audioClip = GetOrAddAudioClip(path, type);
            Play(audioClip, type, pitch);
        }

        public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            if (audioClip == null)
                return;

            if (type == Define.Sound.Bgm)
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            }
        }

        AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
        {
            if (path.Contains("Sounds/") == false)
                path = $"Sounds/{path}";

            AudioClip audioClip = null;

            if (type == Define.Sound.Bgm)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
            }
            else
            {
                if (_audioClips.TryGetValue(path, out audioClip) == false)
                {
                    audioClip = Managers.Resource.Load<AudioClip>(path);
                    _audioClips.Add(path, audioClip);
                }
            }

            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {path}");

            return audioClip;
        }

        public float GetVolume(Define.Sound soundType)
        {
            if (soundType == Define.Sound.Bgm)
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
                return audioSource.volume;
            }
            else
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
                return audioSource.volume;
            }
        }

        public void ChangeVolume(Define.Sound soundType, float value)
        {
            if (soundType == Define.Sound.Bgm)
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
                audioSource.volume = value;
                PlayerPrefs.SetFloat("BgmVolume", value);
            }
            else
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
                audioSource.volume = value;
                PlayerPrefs.SetFloat("EffectVolume", value);
            }
        }
    }
}
