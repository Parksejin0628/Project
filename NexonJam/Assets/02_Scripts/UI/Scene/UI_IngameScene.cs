using DefaultSetting.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_IngameScene : UI_Scene
    {

        private PlayerController Player;
        private float playerMaxHp;
        private float playerMaxHunger;
        private float playerMaxThirst;

        public ItemName playerItem;
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

        enum Objects
        {
            HpBarImage,
            HungerBarImage,
            ThirstBarImage,
        }

        enum Sliders
        {
            HPBar,
            HungerBar,
            ThirstBar,
        }

        enum Buttons
        {
            OnSettingButton,
        }

        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<Slider>(typeof(Sliders));
            Bind<GameObject>(typeof(Objects));

            GetButton(Buttons.OnSettingButton.ToInt()).gameObject.BindEvent(_ => Managers.UI.ShowPopupUI<UI_Setting>());

            Player = Managers.Game.Player;
            playerMaxHp = Player.Stat.maxhp;
            playerMaxHunger = Player.Stat.maxhunger;
            playerMaxThirst = Player.Stat.maxthirst;

            int isTutorial = PlayerPrefs.GetInt(Define.IS_SHOW_TUTORIAL_KEY);
            if (isTutorial == 0)
            {
                Managers.UI.ShowPopupUI<UI_Tutorial>();
                PlayerPrefs.SetInt(Define.IS_SHOW_TUTORIAL_KEY, 1);
            }
        }

        private void Update()
        {
            UpdateStatusUI();
            UpdateItemUI();
        }

        private void UpdateStatusUI()
        {
            if (Player == null)
                return;

            GetSlider(Sliders.HPBar.ToInt()).value = Player.Stat.Hp / playerMaxHp;
            GetSlider(Sliders.HungerBar.ToInt()).value = Player.Stat.Hunger / playerMaxHunger;
            GetSlider(Sliders.ThirstBar.ToInt()).value = Player.Stat.Thirst / playerMaxThirst;

            GetObject(Objects.HpBarImage.ToInt()).SetActive(GetSlider(Sliders.HPBar.ToInt()).value == 0 ? false : true);
            GetObject(Objects.HungerBarImage.ToInt()).SetActive(GetSlider(Sliders.HungerBar.ToInt()).value == 0 ? false : true);
            GetObject(Objects.ThirstBarImage.ToInt()).SetActive(GetSlider(Sliders.ThirstBar.ToInt()).value == 0 ? false : true);
        }

        private void UpdateItemUI()
        {
            playerItem = Player.myItem == null ? ItemName.None : Player.myItem.name;
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
                case ItemName.BottleWater:
                    itemImage.sprite = BottleWaterSprite;
                    break;
                case ItemName.Bandage:
                    itemImage.sprite = BandageSprite;
                    break;
                case ItemName.Brick:
                    itemImage.sprite = BrickSprite;
                    break;
                case ItemName.WaterMelon:
                    itemImage.sprite = WaterMelonSprite;
                    break;
                case ItemName.Hardtack:
                    itemImage.sprite = HardtackSprite;
                    break;
                case ItemName.Painkillers:
                    itemImage.sprite = PainkillersSprite;
                    break;
                case ItemName.MysteriousPill:
                    itemImage.sprite = MysteriousPillSprite;
                    break;
                case ItemName.RawFish:
                    itemImage.sprite = RawFishSprite;
                    break;
                case ItemName.SeaWater:
                    itemImage.sprite = SeaWaterSprite;
                    break;
                case ItemName.ShellFish:
                    itemImage.sprite = ShellFishSprite;
                    break;
                case ItemName.StarFish:
                    itemImage.sprite = StarFishSprite;
                    break;
                case ItemName.EngineOil:
                    itemImage.sprite = EngineOilSprite;
                    break;
                case ItemName.MysteriousBox:
                    itemImage.sprite = MysteriousBoxSprite;
                    break;
                case ItemName.OrangePill:
                    itemImage.sprite = OrangePillSprite;
                    break;
                case ItemName.Apple:
                    itemImage.sprite = AppleSprite;
                    break;
                case ItemName.RiverWater:
                    itemImage.sprite = RiverWaterSprite;
                    break;
                case ItemName.OrangeMushroom:
                    itemImage.sprite = OrangeMushroomSprite;
                    break;
                case ItemName.Rock:
                    itemImage.sprite = RockSprite;
                    break;
                case ItemName.SparklingWater:
                    itemImage.sprite = SparklingWaterSprite;
                    break;
                case ItemName.BluePill:
                    itemImage.sprite = BluePillSprite;
                    break;
            }
        }
    }
}
