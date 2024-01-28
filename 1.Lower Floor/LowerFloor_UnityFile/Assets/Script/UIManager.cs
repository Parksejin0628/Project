using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //코드 전반적으로 필요한 데이터들
    private GameObject player;
    private State playerState;
    //itemUI를 수정하기 위한 데이터들
    public GameObject[] itemUI;
    public Dictionary<InteractionObjectName, Sprite> itemSprite = new Dictionary<InteractionObjectName, Sprite>();    //itemSprite_ItemName, itemSprite_Sprite정보를 Dictionary 형태로 사용
    public InteractionObjectName[] itemSprite_ItemName;
    public Sprite[] itemSprite_Sprite;
    private Image[] itemUIImage;
    public TextMeshProUGUI[] itemUIText;
    //stateUI를 수정하기 위한 데이터들
    public Slider HPBar;        //체력바
    public Slider EnergyBar;    //기력바
    public GameObject[] HungerBar = new GameObject[2];  //허기바
    //weaponUI를 수정하기 위한 데이터들
    public GameObject weaponUI;
    private Image weaponUIImage;
    public Slider weaponUIDurabilityBar;
    public Dictionary<InteractionObjectName, Sprite> weaponSprite = new Dictionary<InteractionObjectName, Sprite>();
    public Sprite[] weaponSprite_sprite;
    public InteractionObjectName[] weaponSprite_weaponName;
    //스킬 셋팅 캔버스 
    public GameObject SkillCanvas;
    private bool CanvasActive = false; 

    private void Awake()
    {
        //아이템UI 관련 초기화
        for (int i = 0; i < itemSprite_ItemName.Length; i++)
        {
            //ItemSprite 딕셔너리 자료구조 초기화
            itemSprite.Add(itemSprite_ItemName[i], itemSprite_Sprite[i]);
        }
        itemUIImage = new Image[itemUI.Length];
        itemUIText = new TextMeshProUGUI[itemUI.Length];
        for (int i = 0; i < itemUI.Length; i++)
        {
            //Debug.Log(itemUI[i]);
            itemUIImage[i] = itemUI[i].GetComponent<Image>();
            itemUIText[i] = itemUI[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        //weaponUI 관련 초기화
        weaponUIImage = weaponUI.GetComponent<Image>();
        for(int i=0; i<weaponSprite_weaponName.Length; i++)
        {
            weaponSprite.Add(weaponSprite_weaponName[i], weaponSprite_sprite[i]);
        }
        //플레이어와 플레이어 스탯 저장
        player = GameObject.Find("Player");
        playerState = player.GetComponent<State>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckItem();        //현재 아이템 상태 확인
        CheckWeapon();
        CheckState();       //현재 플레이어 스탯 확인
    }

    void CheckItem()        //플레이어가 보유하고 있는 아이템의 상태를 확인하여 아이템 UI에 표시
    {
        //아이템 1~3칸 전부 확인
        for(int i=0; i< itemUI.Length; i++)
        {
            //아이템을 모두 소모한 경우 해당 아이템 칸 비우기
            if (playerState.item[i].count == 0)
            {
                itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 0);
                itemUIImage[i].sprite = null;
                itemUIText[i].text = null;
                continue;
            }
            //아이템이 있는 경우 해당 아이템 표시하기
            itemUIImage[i].color = new Color(itemUIImage[i].color.r, itemUIImage[i].color.g, itemUIImage[i].color.b, 255);
            itemUIImage[i].sprite = itemSprite[playerState.item[i].name];
            itemUIText[i].text = playerState.item[i].count.ToString();
        }
    }

    private void Update()
    {
        // 캔버스가 활성화되어 있지 않고 X 키를 눌렀을 때
        if (!CanvasActive && Input.GetKeyDown(KeyCode.X))
        {
            SkillCanvas.SetActive(true);
            CanvasActive = true;
            Debug.Log("스킬UI를 활성화합니다");
        }

        // 캔버스가 활성화되어 있고 Escape 키 또는 X 키를 눌렀을 때
        else if (CanvasActive && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X)))
        {
            SkillCanvas.SetActive(false);
            CanvasActive = false;
            Debug.Log("스킬UI를 비활성화합니다");
        }
    }

    void CheckWeapon()
    {
        if (playerState.auxiliaryWeapon.count <= 0)
        {
            weaponUIImage.color = new Color(weaponUIImage.color.r, weaponUIImage.color.g, weaponUIImage.color.b, 0);
            weaponUIImage.sprite = null;
            weaponUIDurabilityBar.value = 0;
            weaponUIDurabilityBar.gameObject.SetActive(false);
            return;
        }
        weaponUIImage.color = new Color(weaponUIImage.color.r, weaponUIImage.color.g, weaponUIImage.color.b, 255);
        weaponUIImage.sprite = weaponSprite[playerState.auxiliaryWeapon.name];
        weaponUIDurabilityBar.gameObject.SetActive(true);
        weaponUIDurabilityBar.value = (float)playerState.auxiliaryWeapon.count / (float)InteractionManager.instance.weaponAcquiredDurability[playerState.auxiliaryWeapon.name];
    }

    void CheckState()   //현재 플레이어 상태를 확인하여 플레이어 스탯 UI 업데이트
    {
        HPBar.value = playerState.currentHP / playerState.maxHP;
        EnergyBar.value = playerState.currentEnergy / playerState.maxEnergy;
        switch (playerState.currentHunger)  //허기 값에 따라 허기 UI 상태 수정
        {
            case 0:
                HungerBar[0].SetActive(false);
                HungerBar[1].SetActive(false);
                break;
            case 1:
                HungerBar[0].SetActive(true);
                HungerBar[1].SetActive(false);
                break;
            case 2:
                HungerBar[0].SetActive(true);
                HungerBar[1].SetActive(true);
                break;
        }
    }

}
