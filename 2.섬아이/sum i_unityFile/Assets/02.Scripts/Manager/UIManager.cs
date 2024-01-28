using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    //UIManager에 필요한 기본 변수
    static public UIManager instance;
    public GameObject player;
    private PlayerCtrl playerCtrl;
    public GameObject waveTime;
    private wave_time waveTimeCtrl;


    //
    bool SceneStart = false;
    public Image[] imageArray;
    public int currentIndex = 0;
    const int start_end = 1;
    //hp와 관련된 변수
    public GameObject[] hpEmptyObject;
    //wave와 관련된 변수
    public Image WaveImage;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //SceneStart = true;
        SetActiveImage(currentIndex);
        playerCtrl = player.GetComponent<PlayerCtrl>();
        waveTimeCtrl = waveTime.GetComponent<wave_time>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneStart)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (currentIndex >= start_end)
                {
                    imageArray[currentIndex].gameObject.SetActive(false);
                    SceneStart = false;
                }
                else
                {
                    currentIndex++;
                    SetActiveImage(currentIndex);
                    Debug.Log(currentIndex);
                }
            }
        }
        UpdateHpUI();
        UpdateWaveTimeUI();
    }

    // 현재 인덱스에 해당하는 이미지를 활성화하고 나머지는 비활성화
    void SetActiveImage(int index)
    {
        for (int i = 0; i < imageArray.Length; i++)
        {
            if (i == index)
                imageArray[i].gameObject.SetActive(true);
            else
                imageArray[i].gameObject.SetActive(false);
        }
    }

    void UpdateHpUI()
    {
        for(int i=0; i<playerCtrl.maxHp; i++)
        {
            if(playerCtrl.currentHp <= i)
            {
                hpEmptyObject[i].SetActive(true);
            }
            else
            {
                hpEmptyObject[i].SetActive(false);
            }
        }
    }

    void UpdateWaveTimeUI()
    {
        if(waveTimeCtrl.isUpWave == true)
        {
            WaveImage.fillAmount = waveTimeCtrl.waveTime / waveTimeCtrl.UpWaveTime;
        }
        else if (waveTimeCtrl.isUpWave == false)
        {
            WaveImage.fillAmount = waveTimeCtrl.waveTime / waveTimeCtrl.DownWaveTime;
        }
    }
}