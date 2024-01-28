using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum InteractionObjectCategory //상호작용하는 오브젝트의 카테고리 정보
{
    NULL = -1,
    WEAPON = 0,
    ITEM,
    NPC
}
public enum InteractionObjectName    //상호작용하는 오브젝트의 이름
{
    NULL = -1, 
    /*Item*/     BANDAGE, PAINKILLER, EPINEPHRINE, CAN, CUPRAMEN,
    /*Weapon*/   SCALPEL, PIPE, BLOODPACK, FIREEXTINGUISHER,
    /*NPC*/      ELEVATOR
}

public class InteractionManager : MonoBehaviour
{

    //어디서나 InteractionManager를 호출하기 위한 instance이다. 어디 파일이든 InteractionManager.instance를 사용하면 InteractionManager에 접근할 수 있게 된다.
    static public InteractionManager instance;
    private GameObject player;
    //각 아이템별 획득 개수
    public Dictionary<InteractionObjectName, int> itemAcquiredAmount= new Dictionary<InteractionObjectName, int>()
    {
        { InteractionObjectName.BANDAGE, 4 },
        { InteractionObjectName.PAINKILLER, 3 },
        { InteractionObjectName.EPINEPHRINE, 1 },
        { InteractionObjectName.CAN, 1 },
        { InteractionObjectName.CUPRAMEN, 1 }
    };
    //무기별 획득 내구도량
    public Dictionary<InteractionObjectName, int> weaponAcquiredDurability = new Dictionary<InteractionObjectName, int>()
    {
        { InteractionObjectName.SCALPEL, 1},
        { InteractionObjectName.PIPE, 5 },
        { InteractionObjectName.BLOODPACK, 10 },
        { InteractionObjectName.FIREEXTINGUISHER, 10 }
    };


    void Awake()
    {
        instance = this;
        player = GameObject.Find("Player");
    }

    public bool Interact(GameObject interactionObject)  //F키를 눌러 상호작용시 호출되는 함수이다.
    {
        
        //상호작용하는 오브젝트의 Interaction 스크립트 정보
        Interaction interactionInformation = interactionObject.GetComponent<Interaction>();
        Item obtainedItem = null;
        Debug.Log("check1");
        //Interaction 스크립트가 없을 경우 Log 수행
        if (interactionInformation == null)
        {
            Debug.Log(interactionObject + "에 Interaction 스크립트가 없다.");
            return false;
        }
        //카테고리에 따른 함수 수행
        switch(interactionInformation.Category)
        {
            //아이템 종류가 아닌 경우 리턴
            case InteractionObjectCategory.NULL:
                break;
            //ITEM 카테고리의 아이템을 획득한 경우 상호작용한 아이템의 정보를 GetItem으로 보낸다.
            case InteractionObjectCategory.ITEM:
                if(interactionInformation.itemName == InteractionObjectName.BANDAGE)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.BANDAGE, itemAcquiredAmount[InteractionObjectName.BANDAGE]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.PAINKILLER)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.PAINKILLER, itemAcquiredAmount[InteractionObjectName.PAINKILLER]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.EPINEPHRINE)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.EPINEPHRINE, itemAcquiredAmount[InteractionObjectName.EPINEPHRINE]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.CAN)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.CAN, itemAcquiredAmount[InteractionObjectName.CAN]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.CUPRAMEN)
                {
                    obtainedItem = new Item(InteractionObjectCategory.ITEM, InteractionObjectName.CUPRAMEN, itemAcquiredAmount[InteractionObjectName.CUPRAMEN]);
                }
                if(!player.GetComponentInChildren<State>().GetItem(obtainedItem))   //State.cs의 GetItem이 false인 경우 아이템 획득에 실패한다.
                {
                    Debug.Log("인벤토리 꽉참!");
                    return false;
                }
                break;
            //WEAPON 카테고리의 아이템을 획득한 경우
            case InteractionObjectCategory.WEAPON:
                if(interactionInformation.itemName == InteractionObjectName.SCALPEL)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.SCALPEL, weaponAcquiredDurability[InteractionObjectName.SCALPEL]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.PIPE)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.PIPE, weaponAcquiredDurability[InteractionObjectName.PIPE]);
                }
                else if(interactionInformation.itemName == InteractionObjectName.BLOODPACK)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.BLOODPACK, weaponAcquiredDurability[InteractionObjectName.BLOODPACK]);
                }
                else if (interactionInformation.itemName == InteractionObjectName.FIREEXTINGUISHER)
                {
                    obtainedItem = new Item(InteractionObjectCategory.WEAPON, InteractionObjectName.FIREEXTINGUISHER, weaponAcquiredDurability[InteractionObjectName.FIREEXTINGUISHER]);
                }
                player.GetComponentInChildren<State>().GetItem(obtainedItem);
                break;
        }
        //상호작용한 물체 삭제, 추후에 각 카테고리별 아이템이 생기면 삭제```
        interactionObject.SetActive(false);
        return true;
    }
    

    
}

[System.Serializable]
public class Item   //아이템 클래스
{

    public InteractionObjectCategory category;
    public InteractionObjectName name;
    public float count;

    public Item()
    {
        category = InteractionObjectCategory.NULL;
        name = InteractionObjectName.NULL;
        count = 0;
    }
    public Item(InteractionObjectCategory category, InteractionObjectName name, int count)
    {
        this.category = category;
        this.name = name;
        this.count = count;
    }
};
