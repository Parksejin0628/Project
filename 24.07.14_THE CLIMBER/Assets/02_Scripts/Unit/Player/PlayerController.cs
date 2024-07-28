using Unity.VisualScripting;
using UnityEngine;



namespace DefaultSetting
{
    [RequireComponent(typeof(PlayerMove))]
    [RequireComponent(typeof(PlayerAnimation))]
    [RequireComponent(typeof(PlayerStat))]
    [RequireComponent(typeof(PlayerState))]

    public class PlayerController : UnitController
    {
        private PlayerMove _move;
        public PlayerMove Move
        {
            get
            {
                if (_move == null)
                {
                    _move = GetComponent<PlayerMove>();
                }
                return _move;
            }
        }
        private PlayerAnimation _anim;
        public PlayerAnimation Anim
        {
            get
            {
                if (_anim == null)
                {
                    _anim = GetComponent<PlayerAnimation>();
                }
                return _anim;
            }
        }

        private PlayerStat _stat;
        public PlayerStat Stat
        {
            get
            {
                if (_stat == null)
                {
                    _stat = GetComponent<PlayerStat>();
                }
                return _stat;
            }
        }
        private PlayerState _state;
        public PlayerState State
        {
            get
            {
                if (_state == null)
                {
                    _state = GetComponent<PlayerState>();
                }
                return _state;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            Stat.MyAwake();

            State.MyAwake();
            Move.MyAwake();
            Anim.MyAwake();

            Managers.Input.newInputManager.interactionAction -= GetItem;
            Managers.Input.newInputManager.interactionAction += GetItem;
            Managers.Input.newInputManager.ItemAction -= UseItem;
            Managers.Input.newInputManager.ItemAction += UseItem;
        }

        protected override void Update()
        {
            if (!isAlive)
                return;

            if (Managers.Game.IsClear)
                return;
        }

        protected override void FixedUpdate()
        {
            if (!isAlive)
                return;

            if (Managers.Game.IsClear)
                return;

            Move.MyFixedUpdate();
            Anim.MyFixedUpdate();
            State.MyFixedUpdate();
            Stat.MyFixedUpdate();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isAlive)
                return;

            if (Managers.Game.IsClear)
                return;


            if (collision.CompareTag("Item"))
            {
                NearItem = collision.gameObject.GetComponent<Item>();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Item"))
            {
                if (canGetItem == false)
                {
                    canGetItem = true;
                }
            }

        }



        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Item"))
            {
                canGetItem = false;
            }
        }

        //-------------------------------------

        public override void OnHitUnit(float getDamage, bool CheckInvincibility = true)
        {
            if (CheckInvincibility && State.IsInvincibility)
                return;

            Managers.Sound.Play(Managers.Data.MstMaster.PlayerSoundData.PlayerHitSound);

            //데미지 처리
            Stat.Hp -= getDamage;

            //상태 처리
            if (State.IsJumping)
                Move.cancelJumping = true;
            if (State.IsDashing)
                Move.cancelDashing = true;

            Anim.anim_TriggerHit = true;

            State.HitInvincibility();

            Move.currentJumpCount = 1;
        }

        public override void OnUnitDie()
        {
            base.OnUnitDie();
            Move.OnUnitDie();
            Anim.OnUnitDie();

            Managers.Game.OnPlayerDie();
        }

        public bool WaitDashDown
        {
            get => Managers.Input.newInputManager.WaitInteractionDown;
            set => Managers.Input.newInputManager.WaitInteractionDown = value;
        }


        /*
         * 아이템 부분 
        */

        [Header("아이템 관련 변수")]
        public Item myItem;
        public Item NearItem;
        public bool canGetItem;
        public bool canUseItem;

        public void GetItem()
        {
            if (canGetItem)
            {
                Managers.Sound.Play(Managers.Data.MstMaster.PlayerSoundData.PlayerSelectItemSound);
                myItem = NearItem;
                NearItem.gameObject.SetActive(false);
                canUseItem = true;
            }
        }

        public void UseItem()
        {

            if (!canUseItem) return;
            Debug.Log("아이템 사용");
            switch (myItem.name)
            {
                case ItemName.Brick:
                    SpawnObject();
                    break;
                case ItemName.MysteriousPill:
                    _stat.hungerDecayMultiplier = 0;
                    _stat.thirstDecayMultiplier = 0;
                    break;
                case ItemName.MysteriousBox:
                    int temp = Random.Range(0, 2);
                    ChangeStat(-20 * temp, 20 * (1 - temp), 0);
                    break;
                case ItemName.OrangePill:
                    _stat.hungerDecayMultiplier *= 2;
                    break;
                case ItemName.Rock:
                    SpawnObject();
                    break;
                case ItemName.SparklingWater:
                    _stat.thirstDecayMultiplier *= 2;
                    break;
                case ItemName.BluePill:
                    _stat.hungerDecayMultiplier *= 2;
                    break;
                case ItemName.Balance:
                    float average = (_stat.Hp + _stat.Hunger + _stat.Thirst) / 3;
                    _stat.Hp = average;
                    _stat.Hunger = average;
                    _stat.Thirst = average;
                    break;
                case ItemName.BrokenBrick:
                    SpawnObject();
                    break;
            }
            myItem.name = ItemName.None;
            canUseItem = false;
            ChangeStat(myItem.hpChangeAmount, myItem.hungerChangeAmount, myItem.thirstChangeAmount);
        }

        private void ChangeStat(float hp, float hunger, float thirst)
        {
            _stat.Hp += hp;
            _stat.Hunger += hunger;
            _stat.Thirst += thirst;
        }

        private void SpawnObject()
        {
            Instantiate(myItem.spawnObject, transform.position + new Vector3(myItem.spawnSize.x * _move.LookDir.x, myItem.spawnSize.y, transform.position.z), Quaternion.identity);
        }



        public void OnInteraction()
        {
            Debug.Log("A");
        }
    }
}
