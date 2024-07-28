using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    public class PlayerState : MonoBehaviour
    {
        PlayerController controller;

        //TODO: 다른 상태들도 이곳에 저장할 예정
        //TODO: Has 친구들은 Stat으로 분류하는 것이 맞을 것 같다.

        //애니메이션은 이곳을 통해 여러 상태를 가져다가 사용할 수 있게 하자

        //TODO : 실시간으로 연동 안됨.
        [ReadOnly][SerializeField] private bool _isDashing;
        public bool IsDashing
        {
            get
            {
                _isDashing = controller.Move.isDashing;
                return _isDashing;
            }
        }
        [ReadOnly][SerializeField] private bool _isJumping;
        public bool IsJumping
        {
            get
            {
                _isJumping = controller.Move.isJumping;
                return _isJumping;
            }
        }

        //무적 상태 확인
        [Header("무적상태 확인")]
        [ReadOnly]
        public float normalInvincibilityTime = 1f;
        [ReadOnly]
        public float currentinvincibilityTime;
        public bool IsInvincibility
        {
            get
            {
                return currentinvincibilityTime > 0;
            }
        }

        //행동 불능 상태 확인
        [Header("행동불능 상태 확인")]
        [ReadOnly]
        public float normalInabilityActTime = 0.3f;
        [ReadOnly]
        public float currentInabilityActTime;
        public bool IsInabilityActTime
        {
            get
            {
                return currentInabilityActTime > 0;
            }
        }

        public void MyAwake()
        {
            controller = GetComponent<PlayerController>();
        }

        public void MyFixedUpdate()
        {
            currentinvincibilityTime -= Time.fixedDeltaTime;
            currentInabilityActTime -= Time.fixedDeltaTime;
        }

        //피격 시 무적 상태로 변경하는 함수
        public void HitInvincibility()
        {
            currentinvincibilityTime = normalInvincibilityTime;
            currentInabilityActTime = normalInabilityActTime;
        }
    }
}