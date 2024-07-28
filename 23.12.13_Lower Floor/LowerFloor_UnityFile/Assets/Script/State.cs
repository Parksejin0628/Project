using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class State : MonoBehaviour
{
    //캐릭터 스킬 액티브 
    public int active_e = -1;
    public int active_shift = -1;

    public float exp; //캐릭터 경험치
    public int skill_exp = 10; //스킬포인트
    public float maxHP = 100f;          //최대 체력
    public float maxEnergy = 100f;      //최대 기력
    public int maxHunger = 2;           //최대 허기
    public float currentHP;             //현재 체력         
    public float currentEnergy;         //현재 기력
    public int currentHunger;           //현재 허기
    //재생과 관련된 변수들
    public float HPRegenerationAmount = 2f;                     //재생 시 초당 체력 회복량
    public float EnergyRegenerationAmount = 3f;                 //재생 시 초당 기력 회복량
    public float HPRegenerationCondition_EnergyPercent = 1.0f;  //체력 재생 발동 조건이 되는 기력의 퍼센트, 0~1사이 값이다.
    private float lastDecreaseHungerTime;                       //마지막으로 허기가 감소된 시간
    public float decreaseHungerCoolTime = 120.0f;               //허기가 감소되는 주기
    //아이템, 무기와 관련된 변수들
    public Item[] item = new Item[3];                           //현재 소지 중인 아이템
    public Item auxiliaryWeapon = new Item();
    public GameObject DeadImage;

    public float PlayerAttackDamage = 25;
    public int testIndex = 0;
    private float previousHP;
    

    void Awake()
    {
        //플레이어 스탯 초기화
        currentHP = maxHP;
        currentEnergy = maxEnergy;
        currentHunger = maxHunger;
        lastDecreaseHungerTime = Time.time;
    }

    private void Update()
    {
        //현재 기력이 최대기력 * HPRegenerationCondition_EnergyPercent인 경우 초당 HPRegenerationAmount만큼 체력 회복
        if (currentEnergy >= maxEnergy * HPRegenerationCondition_EnergyPercent)
        {
            SetHP(currentHP + (Time.deltaTime * HPRegenerationAmount));
        }
        //현재 허기가 최대 허기인 경우 초당 EnergyRegenerationAmount만큼 기력 회복
        if (currentHunger >= maxHunger)
        {
            SetEnergy(currentEnergy + (Time.deltaTime * EnergyRegenerationAmount));
        }
        //허기 감소 주기가 지날 경우 허기 
        if (Time.time - lastDecreaseHungerTime >= decreaseHungerCoolTime)
        {
            SetHunger(currentHunger - 1);
            lastDecreaseHungerTime = Time.time;
        }
        if(exp > 100)
        {
            exp %= 100;
            skill_exp++;
        }
        if(previousHP > currentHP)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.CurvetDamaged);
        }
        previousHP = currentHP;
        //Debug.Log(currentHP + ", " + ++testIndex);
    }


    
    public bool GetItem(Item obtainedItem)  //아이템을 획득하는 함수이다. 현재 비어있는 인벤토리를 확인하여 빈 인벤토리에 아이템을 넣는다.
    {
        if (obtainedItem.category == InteractionObjectCategory.ITEM)
        {
            for (int i = 0; i < 3; i++)
            {
                //아이템이 없는 인벤토리를 발견한 경우 획득
                if (item[i].count == 0)
                {
                    item[i] = obtainedItem;
                    return true;
                }
            }
            //인벤토리가 가득찬 경우 false 리턴
            return false;
        }
        else if(obtainedItem.category == InteractionObjectCategory.WEAPON)
        {
            auxiliaryWeapon = obtainedItem;
            return true;
        }
        return false;
    }

    public void SetHP(float value)      //HP를 조절하는 함수이다. 현재 체력이 value로 조정된다. 체력 증가/감소를 하고 싶으면 value에 'state.currentHP +- 변화량'을 넣으면 된다.
    {
        currentHP = value;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if(currentHP <= 0)
        {
            currentHP = 0;
            SceneManager.LoadScene("seomap");
            DeadImage.SetActive(true);
        }
    }

    public void SetEnergy(float value)  //기력을 조절하는 함수이다. 현재 기력이 value로 조정된다. 기력 증가/감소를 하고 싶으면 value에 'state.currentEnergy +- 변화량'을 넣으면 된다.
    {
        currentEnergy = value;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
    }

    public void SetHunger(int value)    //허기를 조절하는 함수이ㅏㄷ. 현재 허기가 value로 조정된다. 허기 증가/감소를 하고 싶으면 value에 'state.currentHunger +- 변화량'을 넣으면 된다.
    {
        currentHunger = value;
        if (currentHunger > maxHunger)
        {
            currentHunger = maxHunger;
        }
        if (currentHunger <= 0)
        {
            currentHunger = 0;
        }
    }
}