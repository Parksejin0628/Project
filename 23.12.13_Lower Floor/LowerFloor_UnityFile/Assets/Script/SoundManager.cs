using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static SoundManager;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    public int BGMchannelIndex;
    AudioSource[] bgmPlayers;
    int bgmchannel;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Adrenaline, BasicAttack, BasicAttackWind, BloodPack, BossAttack, 
                      BossDie, BossShaking, BossSummon, BrokenPipe, Cancel, CurvetDamaged, 
                      DashZombieDetected, Door, ElevatorMove, FireExtinguisher, HeavyZombieDetected, 
                      HumanZombieDetected, ItemDump, ItemUse, MenuSelect, MiniButton, Scalpel, SlotFill, 
                      TrapZombieDetected, TrapZombieSplit, UseActive1, UseActive3, UseActive4, 
                      UseDash, UseUlt1, UseUlt2
    }

    public enum Bgm { VIP, Dungeon, Boss, Done}



    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Init();
        DontDestroyOnLoad(instance);
    }

    void Init()
    {
        //배경을 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmaPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[BGMchannelIndex];

        for(int i= 0; i < bgmPlayers.Length; i++)
        {
            bgmPlayers[i] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[i].playOnAwake = false;
            bgmPlayers[i].volume = bgmVolume;
            bgmPlayers[i].loop = true;
        }

        //bgmPlayer.clip = bgmClip;

        //효과음 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake=false;
            sfxPlayers[index].volume = sfxVolume;
        }
        sfxPlayers[28].volume = sfxVolume * 0.5f;
    }

    public void PlayBgm(Bgm bgm, bool isPlay)
    {
        for (int index = 0; index < bgmPlayers.Length; index++)
        {
            int loopIndex = (index + bgmchannel) % bgmPlayers.Length;

            bgmchannel = loopIndex;
            bgmPlayers[loopIndex].clip = bgmClips[(int)bgm];
            bgmPlayers[loopIndex].Play();

            break;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)    continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
