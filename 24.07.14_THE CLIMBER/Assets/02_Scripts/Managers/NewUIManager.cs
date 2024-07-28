using DefaultSetting;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewUIManager : MonoBehaviour
{
    private PlayerController Player;

    private float playerMaxHp;
    private float playerHp;
    private float playerMaxHunger;
    private float playerHunger;
    private float playerMaxThirst;
    private float playerThirst;
    private ItemName playerItem;

    public Slider hpBar;
    public GameObject hpBarImage;
    public Slider hungerBar;
    public GameObject hungerBarImage;
    public Slider thirstBar;
    public GameObject thirstBarImage;

    public Image itemImage;

    public Sprite EmergencyFoodSprite;
    public Sprite BottleWaterSprite;
    public Sprite BandageSprite;
    public Sprite BrickSprite;
    public Sprite WaterMelonSprite;
    public Sprite HardtackSprite;
    public Sprite PainkillersSprite;
    public Sprite MysteriousPillSprite;
    public Sprite RawFishSprite;
    public Sprite SeaWaterSprite;
    public Sprite ShellFishSprite;
    public Sprite StarFishSprite;
    public Sprite EngineOilSprite;
    public Sprite MysteriousBoxSprite;
    public Sprite OrangePillSprite;
    public Sprite AppleSprite;
    public Sprite RiverWaterSprite;
    public Sprite OrangeMushroomSprite;
    public Sprite RockSprite;
    public Sprite SparklingWaterSprite;
    public Sprite BluePillSprite;



    private void Start()
    {
        Player = Managers.Game.Player;
        playerMaxHp = Player.GetComponent<PlayerStat>().maxhp;
        playerMaxHunger = Player.Stat.maxhunger;
        playerMaxThirst = Player.Stat.maxthirst;
    }

    private void LateUpdate()
    {
        UpdateStatusUI();
        UpdateItemUI();

    }

    private void UpdateStatusUI()
    {
        playerHp = Player.GetComponent<PlayerStat>().Hp;
        playerHunger = Player.Stat.Hunger;
        playerThirst = Player.Stat.Thirst;

        hpBar.value = playerHp / playerMaxHp;
        hungerBar.value = playerHunger / playerMaxHunger;
        thirstBar.value = playerThirst / playerMaxThirst;

        hpBarImage.SetActive(hpBar.value == 0 ? false : true);
        hungerBarImage.SetActive(hungerBar.value == 0 ? false : true);
        thirstBarImage.SetActive(thirstBar.value == 0 ? false : true);

    }
    private void UpdateItemUI()
    {
        playerItem = Player.myItem.name;
        if (playerItem == ItemName.None)
        {
            itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 0);
            itemImage.sprite = null;
            return;
        }
        itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 255);
        switch (playerItem)
        {
            case ItemName.EmergencyFood:
                itemImage.sprite = EmergencyFoodSprite;
                break;
        }
    }
}