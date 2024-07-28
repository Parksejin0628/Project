using DefaultSetting;
using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
#if INPUT_TYPE_LEGACY
    public class LegacyInputManager : MonoBehaviour
    {
        [Header("Key Code")]
        [SerializeField] private KeyCode JumpKeyCode = KeyCode.Space;
        [SerializeField] private KeyCode _testKeyCode = KeyCode.T;

        public void Init()
        {
            print("InputManager\n레거시 입력 시스템이 적용중입니다.");
        }

        public void OnUpdate()
        {
            UpdateJumpKey();

            if (Input.GetKeyDown(_testKeyCode))
            {
                OnTest();
            }
        }

        public void OnTest()
        {
            Managers.Input.OnTest();
        }

        //-------------------

        //입력 Down, Up, Pressed 확인
        [Header("점프키"), SerializeField, ReadOnly]
        private bool _waitJumpDown;
        public bool WaitJumpDown
        {
            get { return _waitJumpDown; }
            set
            {
                if (value == false)
                    StopCoroutine(coWaitJumpDown);

                _waitJumpDown = value;
            }
        }
        [SerializeField]
        private float waitJumpDownTime = 0.2f;
        Coroutine coWaitJumpDown;

        [field: SerializeField, ReadOnly]
        public bool WaitJumpPressed { get; private set; }

        [SerializeField, ReadOnly]
        private bool _waitJumpUp;
        public bool WaitJumpUp
        {
            get { return _waitJumpUp; }
            set
            {
                if (value == false)
                    StopCoroutine(coWaitJumpUp);

                _waitJumpUp = value;
            }
        }
        [SerializeField]
        private float waitJumpUpTime = 0.2f;
        Coroutine coWaitJumpUp;
        void UpdateJumpKey()
        {
            //다운
            if (Input.GetKeyDown(JumpKeyCode))
            {
                if (coWaitJumpDown != null)
                    StopCoroutine(coWaitJumpDown);

                coWaitJumpDown = StartCoroutine(CoWaitJumpDown());
            }

            //업
            if (Input.GetKeyUp(JumpKeyCode))
            {
                if (coWaitJumpUp != null)
                    StopCoroutine(coWaitJumpUp);

                coWaitJumpUp = StartCoroutine(CoWaitJumpUp());
            }

            //Pressed
            WaitJumpPressed = Input.GetKey(JumpKeyCode);

            IEnumerator CoWaitJumpDown()
            {
                _waitJumpDown = true;
                yield return new WaitForSeconds(waitJumpDownTime);
                _waitJumpDown = false;
                coWaitJumpDown = null;
            }

            IEnumerator CoWaitJumpUp()
            {
                _waitJumpUp = true;
                yield return new WaitForSeconds(waitJumpUpTime);
                _waitJumpUp = false;
                coWaitJumpUp = null;
            }
        }

    }

#else
    public class LegacyInputManager : MonoBehaviour
    {
        [Header("New Input System is Not Used")]
        [SerializeField, ReadOnly] private bool _;

        public void Init()
        {
            Debug.LogError("잘못된 접근입니다.");
        }

        public void OnUpdate()
        {
            Debug.LogError("잘못된 접근입니다.");
        }

        public void OnTest()
        {
            Debug.LogError("잘못된 접근입니다.");
        }
    }
#endif
}