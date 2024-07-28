using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    public class PlayerStat : Stat
    {
        [SerializeField] private float maxStamina = 10f;

        [SerializeField] private float _stamina = 0;
        public float Stamina
        {
            get => _stamina;
            set => _stamina = Mathf.Clamp(value, 0, maxStamina);
        }

        [field: SerializeField] public bool HasMove { get; private set; } = true;
        [field: SerializeField] public bool HasJump { get; private set; } = true;
        [field: SerializeField] public bool HasDoubleJump { get; private set; } = true;
        [field: SerializeField] public bool HasWallJump { get; private set; } = true;
        [field: SerializeField] public bool HasDash { get; private set; } = true;
        [field: SerializeField] public bool HasAttack { get; private set; } = true;
        [field: SerializeField] public bool HasGrappling { get; private set; } = true;
        [Header("스탯 자연감소량")]
        [field: SerializeField] public float hungerDecayRate = 1.0f;
        [field: SerializeField] public float hungerDecayMultiplier = 1.0f;
        [field: SerializeField] public float hungerDecayInterval = 1.0f;
        [field: SerializeField] public float thirstDecayRate = 1.0f;
        [field: SerializeField] public float thirstDecayMultiplier = 1.0f;
        [field: SerializeField] public float thirstDecayInterval = 1.0f;
        private float nowHpDecayInterval = 0.0f;
        private float nowHungerDecayInterval = 0.0f;
        private float nowThirstDecayInterval = 0.0f;

        //플레이어는 따로 매니저가 세팅해주므로 행동X
        protected override void SetUnitState() { SetPlayerStat(); }

        //매니저가 이 친구를 실행해서 기본 스텟들을 세팅해준다.라는 개념으로 접근하자.
        public void SetPlayerStat()
        {
            Hp = maxhp;
            Hunger = maxhunger;
            Thirst = maxthirst;
            nowHungerDecayInterval = hungerDecayInterval;
            nowThirstDecayInterval = thirstDecayInterval;
        }

        protected override void OnUnitDie()
        {
            PlayerController controller = GetComponent<PlayerController>();
            if (controller.isAlive)
                controller.OnUnitDie();
        }


        public void MyFixedUpdate()
        {
            PlayerController controller = GetComponent<PlayerController>();
            nowHpDecayInterval -= Time.deltaTime;
            nowHungerDecayInterval -= Time.deltaTime;
            nowThirstDecayInterval -= Time.deltaTime;

            if (nowHungerDecayInterval <= 0.0f)
            {
                Hunger -= hungerDecayRate * hungerDecayMultiplier;
                nowHungerDecayInterval = hungerDecayInterval;
            }
            if (nowThirstDecayInterval <= 0.0f)
            {
                Thirst -= thirstDecayRate * thirstDecayMultiplier;
                nowThirstDecayInterval = thirstDecayInterval;
            }
            if (Hunger <= 0)
            {
                controller.Move.moveForceMultiplier = 0.3f;
            }
            else
            {
                controller.Move.moveForceMultiplier = 1.0f;
            }
            if (Thirst <= 0)
            {
                controller.Move.jumpPowerMultiplier = 0.5f;
                controller.Move.dashPowerMultiplier = 0.5f;
                controller.Move.wallMoveForceMultiplier = 0.5f;
            }
            else
            {
                controller.Move.jumpPowerMultiplier = 1.0f;
                controller.Move.dashPowerMultiplier = 1.0f;
                controller.Move.wallMoveForceMultiplier = 1.0f;
            }

        }
    }
}