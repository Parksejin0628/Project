using DefaultSetting.Utility;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultSetting
{
    public class PlayerMove : MonoBehaviour
    {
        //TODO: KEY 세팅
        public Vector3 inputMove => (Vector3)Managers.Input.newInputManager.inputMoveDir;
        public Vector3 inputDashDir => (Vector3)Managers.Input.newInputManager.inputDashDir;
        public Vector3 inputVerticalDir => (Vector3)Managers.Input.newInputManager.inputDashDir;
        public bool isMovePressed => Managers.Input.newInputManager.isMovePressed;
        public bool WaitJumpDown
        {
            get => Managers.Input.newInputManager.WaitJumpDown;
            set => Managers.Input.newInputManager.WaitJumpDown = value;
        }
        public bool WaitJumpPressed => Managers.Input.newInputManager.WaitJumpPressed;
        public bool WaitDashDown
        {
            get
            {
                if (!controller.State.IsInvincibility)
                    return Managers.Input.newInputManager.WaitDashDown;
                else
                    return false;
            }
            set => Managers.Input.newInputManager.WaitDashDown = value;
        }
        public bool WaitGrabPressed
        {
            get
            {
                if (!controller.State.IsInvincibility)
                    return Managers.Input.newInputManager.WaitGrabPressed;
                else
                    return false;
            }
        }

        PlayerController controller;
        WaitForFixedUpdate cashingWaitForFixedUpdate = new WaitForFixedUpdate();
        Collider2D coll;
        Rigidbody2D rig;

        [field: SerializeField]
        public Vector3 moveDir { get; protected set; } = new Vector3(1, 0, 0);
        [SerializeField]
        private Vector3 _lookDir;
        public Vector3 LookDir
        {
            get
            {
                return _lookDir;

            }
            set
            {
                _lookDir = value;
            }
        }

        [Header("제약 조건 변수")]
        [SerializeField] private float xWalkVelocityRange = 30;
        [SerializeField] private float xStopVelocityRange = 45;
        [SerializeField] private float _fallClamp = -70;
        [SerializeField] private LayerMask floorLayerMask;
        [SerializeField] private LayerMask wallLayerMask;

        [Header("이동 변수")]
        public float moveForce = 600;
        public float moveForceMultiplier = 1.0f;
        private float moveSoundCurrentTime = 0;
        public float wallMoveForce = 50;

        public float wallMoveForceMultiplier = 1.0f;

        [Header("점프 변수")]
        public float jumpTime = 0.4f;
        [ReadOnly] public bool isWallKickFirstJump = false;
        public float firstJumpTime_0_1 = 0.3f;
        public float remainJumpTime_0_1 = 0.5f;

        [ReadOnly] public float currentJumpCount = 1f;
        float NormalJumpCount = 1;

        public bool isJumping { get; protected set; } = false;
        [ReadOnly] public bool cancelJumping = false;
        public float groundJumpPower = 35;
        public Vector2 wallKickPower = new Vector2(300, 35);
        public float jumpPowerMultiplier = 1.0f;
        public float wallSlowdownPower = 2.5f;

        public float defaultJumpCountCoyotaTime = 0.2f;
        [ReadOnly] public float currentJumpCountCoyotaTime = 0f;

        public float defaultWallJumpCoyotaTime = 0.2f;
        [ReadOnly] public float currentWallJumpCoyotaTime = 0f;

        enum LastWallDir
        {
            NotSetting,
            LeftWall,
            RightWall,
        }
        LastWallDir lastWallDir = LastWallDir.NotSetting;

        public GameObject jumpDust;
        public float destroyJumpDustTime = 2f;

        Coroutine coJump = null;

        [field: SerializeField, Header("대쉬 변수")]
        public bool isDashing { get; protected set; } = false;
        [ReadOnly] public bool cancelDashing = false;
        //TODO: 그냥 함수 호출시키는 것이 훨씬 더 간단할 것이다.
        public bool immediateReadyDash;
        [ReadOnly] public int currentDashCount = 1;

        Coroutine coDash;
        [SerializeField] float dashingTime = 0.3f;
        [SerializeField] float dashingCooldown = 1f;
        [SerializeField] float dashPower = 20f;
        public float dashPowerMultiplier = 1.0f;

        //그래플링 변수

        [field: Header("상태 변수"), SerializeField, ReadOnly]
        public bool isGround { get; protected set; } = false;
        [SerializeField, ReadOnly]
        private bool _isUpWall = false;
        public bool IsUpWall
        {
            get
            {
                return _isUpWall;
            }
            protected set
            {
                if (value)
                {
                    cancelJumping = true;
                }

                _isUpWall = value;
            }
        }
        [field: SerializeField, ReadOnly]
        public bool isLeftWallCheck { get; protected set; } = false;
        [field: SerializeField, ReadOnly]
        public bool isRightWallCheck { get; protected set; } = false;
        public bool IsWallCheck { get { return !isGround && (isLeftWallCheck || isRightWallCheck); } }
        [field: SerializeField, ReadOnly]
        public bool isGrabLeftWall { get; protected set; } = false;
        [field: SerializeField, ReadOnly]
        public bool isGrabRightWall { get; protected set; } = false;

        public bool IsGrabWall
        {
            get
            {
                if (controller.Stat.Stamina == 0)
                    return false;

                return !isGround && (isGrabLeftWall || isGrabRightWall);
            }
        }
        public bool isSkying { get { return !isGround && !IsGrabWall; } } //하늘에 떠있는 상태

        string cachedCanGrabWallStr = null;

        [Header("스테미나 변수")]
        public float getStaminaPerSec = 2.5f;
        public float grabWallStaminaPerSec = 0.2f;
        public float wallJumpStaminaCost = 2.5f;
        public float moveGrabWallStaminaPerSec = 2.5f;


        public void MyAwake()
        {
            CashingFunction();
        }

        public void MyFixedUpdate()
        {
            if (controller.State.IsInabilityActTime)
                return;

            //상태 확인
            CheckState();

            //행동(키 입력 관련 처리들)
            Move2D();
            CheckJump();
            CheckDash();
            CheckGrabWall();

            //사후 조건
            //로프 velocity 값 범위 제약
            ClampVelocity();
            if (isGround)
            {
                controller.Stat.Stamina += getStaminaPerSec * Time.fixedDeltaTime;
            }
            else if (IsGrabWall)
            {
                controller.Stat.Stamina -= grabWallStaminaPerSec * Time.fixedDeltaTime;
            }
            SubVariableDeltaTime();

        }

        public void OnUnitDie()
        {
        }

        private void CashingFunction()
        {
            controller = GetComponent<PlayerController>();
            rig = GetComponent<Rigidbody2D>();
            coll = GetComponent<Collider2D>();
            cachedCanGrabWallStr = Enum.GetName(typeof(Define.Tag), Define.Tag.CanGrabWall);
        }

        void CheckState()
        {
            //TODO : 지형 체크를 별개의 클래스로 만들어 관리 고려, GrabWall, Grappling, WallPower 등등
            //TODO : 상태 처리를 함수로 변경, DrawRay 사용 여부를 옵션으로 표시

            RaycastHit2D hit;

            //땅 체크
            CheckGroundState();


            CheckWallState();


            if (isGround || IsGrabWall)
            {
                immediateReadyDash = true;
            }

            SetJumpCount();
            SetDashCount();

            void CheckGroundState()
            {
                if (isJumping || isDashing)
                {
                    isGround = false;
                    return;
                }

                Vector3 offset = coll.offset;

                Vector3 startPositionFront = transform.position + new Vector3(-coll.bounds.extents.x + offset.x + 0.02f, offset.y, 0);
                Vector3 startPositionBack = transform.position + new Vector3(coll.bounds.extents.x + offset.x - 0.02f, offset.y, 0);

                bool groundFrontCheck = Physics2D.Raycast(startPositionFront, Vector2.down, coll.bounds.extents.y + 0.1f, floorLayerMask);
                bool groundBackwardCheck = Physics2D.Raycast(startPositionBack, Vector2.down, coll.bounds.extents.y + 0.1f, floorLayerMask);

                isGround = groundFrontCheck || groundBackwardCheck;
            }
            void CheckWallState()
            {
                Vector3 offset = coll.offset;

                // 위쪽 벽 체크
                IsUpWall = Physics2D.Raycast(transform.position + new Vector3(0, offset.y, 0), Vector2.up, coll.bounds.extents.y + 0.1f + offset.y, wallLayerMask);

                isLeftWallCheck = false;
                isRightWallCheck = false;
                isGrabRightWall = false;
                isGrabLeftWall = false;

                if (isGround)
                    return;

                int rayCount = 5;
                float f = 0.05f; // 보정치
                float startPoint = -coll.bounds.extents.y + offset.y / 2 - f;
                float stepLength = ((coll.bounds.extents.y + offset.y / 2 + f) * 2) / (rayCount - 1);

                // 왼쪽 벽 체크

                for (int i = 0; i < rayCount; i++)
                {
                    Vector3 rayStart = transform.position - offset / 2 + new Vector3(-coll.bounds.extents.x, startPoint + stepLength * i, 0);
                    //Debug.DrawRay(rayStart, Vector3.left * 0.02f, Color.white, 0.1f);
                    RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.left, 0.02f, wallLayerMask);
                    if (hit.transform?.tag == cachedCanGrabWallStr)
                    {
                        isLeftWallCheck = true;
                        if (WaitGrabPressed)
                        {
                            lastWallDir = LastWallDir.LeftWall;
                            currentWallJumpCoyotaTime = defaultWallJumpCoyotaTime;
                            isGrabLeftWall = true;
                        }

                        break;
                    }

                    if (i == rayCount - 1)
                        isGrabLeftWall = false;
                }

                // 오른쪽 벽 체크
                for (int i = 0; i < rayCount; i++)
                {
                    Vector3 rayStart = transform.position + new Vector3(coll.bounds.extents.x + offset.x / 2, startPoint + stepLength * i, 0);
                    //Debug.DrawRay(rayStart, Vector3.right * 0.02f, Color.white, 0.1f);
                    RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.right, 0.02f, wallLayerMask);
                    if (hit.transform?.tag == cachedCanGrabWallStr)
                    {
                        isRightWallCheck = true;
                        if (WaitGrabPressed)
                        {
                            currentWallJumpCoyotaTime = defaultWallJumpCoyotaTime;
                            lastWallDir = LastWallDir.RightWall;
                            isGrabRightWall = true;
                        }

                        break;
                    }

                    if (i == rayCount - 1)
                    {
                        isGrabRightWall = false;
                    }
                }
            }
            void SetJumpCount()
            {
                if (isGround)
                {
                    currentJumpCountCoyotaTime = defaultJumpCountCoyotaTime;
                    currentJumpCount = NormalJumpCount;
                }
                else if (controller.Stat.Stamina == 0)
                {
                    currentJumpCount = 0;
                }
                else if (IsGrabWall)
                {
                    if (controller.Stat.HasWallJump)
                    {
                        //currentJumpCountCoyotaTime = defaultJumpCountCoyotaTime;
                        currentJumpCount = NormalJumpCount;
                    }
                    else
                        currentJumpCount = 0;
                }
                else if (isSkying)
                {
                    if (0 < currentJumpCountCoyotaTime)
                        currentJumpCount = 1;
                    else
                        currentJumpCount = 0;
                }
            }
            void SetDashCount()
            {
                if (isGround)
                    currentDashCount = 1;
            }
        }


        public void Move2D()
        {
            if (!controller.Stat.HasMove)
                return;

            if (isWallKickFirstJump)
                return;

            if (isDashing)
                return;

            if (!isMovePressed)
                return;

            if (IsGrabWall)
                return;

            if (isGround && moveSoundCurrentTime < 0)
            {
                moveSoundCurrentTime = Managers.Data.MstMaster.PlayerSoundData.PlayerMoveClip.length;
                Managers.Sound.Play(Managers.Data.MstMaster.PlayerSoundData.PlayerMoveClip);
            }

            float x = inputMove.x;
            if (!isGround && isLeftWallCheck)
            {
                x = Mathf.Clamp(x, 0, 1);
            }

            if (!isGround && isRightWallCheck)
            {
                x = Mathf.Clamp(x, -1, 0);
            }

            moveDir = new Vector3(x, 0, 0).normalized;
            LookDir = new Vector3(x, 0, 0).normalized;

            //반대 방향 이동 시 감속해야 함.
            if (x != 0 && Mathf.Sign(x) != Mathf.Sign(rig.velocity.x))
                rig.velocity = new Vector2(rig.velocity.x * 0.85f, rig.velocity.y);

            //addforce 이동
            if (Mathf.Abs(rig.velocity.x) < xWalkVelocityRange)
            {
                rig.AddForce(moveDir * moveForce * moveForceMultiplier);
            }
        }

        void CheckJump()
        {
            if (!(WaitJumpDown && currentJumpCount > 0))
                return;

            WaitJumpDown = false;
            currentJumpCount--;

            if (coJump != null)
                StopCoroutine(coJump);

            coJump = StartCoroutine(CoJump());

            //TODO : 벽 점프 제약, 2단 점프 제약은 나중에 하기
            //TODO : 점프 횟수를 int형이 아닌 2단점프 bool타입 변수로 설정하기
            IEnumerator CoJump()
            {
                //사전 조건
                isJumping = true;
                cancelJumping = false;
                controller.Anim.anim_TriggerJump = true;
                isWallKickFirstJump = false;
                float startSlow = -1f;
                float jumpPowerY = (IsGrabWall ? wallKickPower.y : groundJumpPower) * jumpPowerMultiplier;
                Managers.Sound.Play(Managers.Data.MstMaster.PlayerSoundData.PlayerJumpSound);

                //로직
                float currentTime = 0;
                Vector3 firstJumpDir = GetFirstJumpDir(jumpPowerY);

                float interpolationRatio = 0f;
                while (currentTime < jumpTime && WaitJumpPressed)
                {
                    if (cancelJumping)
                        break;

                    interpolationRatio = currentTime / jumpTime;

                    //초기 점프(벽점일경우 이동 불가 상태로 만들어야 함.)
                    if (interpolationRatio < firstJumpTime_0_1)
                    {
                        if (isWallKickFirstJump)
                        {
                            if (firstJumpDir.x < 0)
                                moveDir = LookDir = Vector3.left;
                            else
                                moveDir = LookDir = Vector3.right;

                            rig.velocity = firstJumpDir;
                        }
                        else
                        {
                            rig.velocity = new Vector2(rig.velocity.x, jumpPowerY);
                        }
                    }
                    else if (interpolationRatio < remainJumpTime_0_1)
                    {
                        isWallKickFirstJump = false;
                        rig.velocity = new Vector2(rig.velocity.x, jumpPowerY);
                    }
                    else
                    {
                        isWallKickFirstJump = false;

                        if (startSlow == -1f)
                            startSlow = currentTime;

                        float t = (currentTime - startSlow) / (jumpTime - startSlow);

                        float calcJumpPower = jumpPowerY - jumpPowerY * Extension.EaseOutCubic(t) * 0.9f;

                        rig.velocity = new Vector2(rig.velocity.x, calcJumpPower);
                    }

                    currentTime += Time.fixedDeltaTime;
                    yield return cashingWaitForFixedUpdate;
                }

                //사후 조건
                if (!isDashing)
                    rig.velocity = new Vector2(rig.velocity.x, 0);

                isJumping = false;
                isWallKickFirstJump = false;
                coJump = null;
            }

            Vector3 GetFirstJumpDir(float jumpPowerY)
            {
                if (currentWallJumpCoyotaTime > 0) //벽점프 코요테타임이 남아있다면
                {
                    //currentWallJumpCoyotaTime = 0;
                    //밀어내는 점프중에는 이동 입력을 막는다.

                    controller.Stat.Stamina -= wallJumpStaminaCost;
                    isWallKickFirstJump = true;

                    switch (lastWallDir)
                    {
                        case LastWallDir.LeftWall:
                            return new Vector2(wallKickPower.x, jumpPowerY);
                        case LastWallDir.RightWall:
                            return new Vector2(-wallKickPower.x, jumpPowerY);
                        default:
                            Debug.LogError("마지막 벽 설정이 되어있지 않음\n");
                            return new Vector2(wallKickPower.x, jumpPowerY);
                    }
                }
                else
                {
                    isWallKickFirstJump = false;
                    return new Vector2(rig.velocity.x, jumpPowerY);
                }
            }
        }

        void CheckDash()
        {
            if (!WaitDashDown)
                return;

            if (!controller.Stat.HasDash)
                return;

            if (currentDashCount == 0)
                return;

            if (coDash != null)
                return;

            //데쉬 실행
            currentDashCount--;
            WaitDashDown = false;
            coDash = StartCoroutine(CoDash());
            //IDE에서 추천해준 코드
            //coDash ??= StartCoroutine(CoDash());

            IEnumerator CoDash()
            {
                //print("dash Start");

                //사전조건
                isDashing = true;
                cancelDashing = false;
                cancelJumping = true;
                immediateReadyDash = false;
                controller.Anim.anim_TriggerDash = true;
                controller.Anim.immediateMakeShadow = true;
                Managers.Sound.Play(Managers.Data.MstMaster.PlayerSoundData.PlayerDashSound);

                float currentGravityScale = this.rig.gravityScale;

                if (isGrabLeftWall)
                    moveDir = LookDir = Vector3.right;
                if (isGrabRightWall)
                    moveDir = LookDir = Vector3.left;

                Vector2 calcVelocity;
                if (inputDashDir != default)
                    calcVelocity = new Vector2(transform.localScale.x * inputDashDir.x, transform.localScale.y * inputDashDir.y).normalized * dashPower * dashPowerMultiplier;
                else
                    calcVelocity = new Vector2(transform.localScale.x * LookDir.x, 0) * dashPower * dashPowerMultiplier;


                //로직
                rig.gravityScale = 0f;
                rig.velocity = calcVelocity;

                float currentTime = 0;
                //대쉬중일 때
                while (currentTime / dashingTime < 1)
                {
                    if (cancelDashing)
                        break;

                    if (rig.velocity.y != 0)
                        rig.velocity = calcVelocity;

                    currentTime += Time.fixedDeltaTime;
                    yield return cashingWaitForFixedUpdate;
                }
                rig.gravityScale = currentGravityScale;
                rig.velocity = Vector2.zero;
                isDashing = false;
                //print("dash End");

                //대쉬 쿨타임 대기
                currentTime = 0f;
                while (currentTime / dashingCooldown < 1)
                {
                    if (immediateReadyDash)
                        break;

                    currentTime += Time.fixedDeltaTime;
                    yield return cashingWaitForFixedUpdate;
                }
                //사후조건
                //print("dash cooldown End");
                coDash = null;
            }
        }

        void CheckGrabWall()
        {
            rig.gravityScale = 1;

            if (!controller.Stat.HasWallJump)
                return;

            if (!IsGrabWall)
                return;

            if (isDashing)
                return;

            if (controller.Stat.Stamina == 0)
                return;

            if (isJumping)
                return;

            Managers.Input.newInputManager.WaitGrabUpAction -= EndInput;
            Managers.Input.newInputManager.WaitGrabUpAction += EndInput;

            LookDir = isGrabLeftWall == true ? Vector3.left : Vector3.right;

            rig.velocity = new Vector3(rig.velocity.x, 0);
            rig.gravityScale = 0;

            if (inputVerticalDir != Vector3.zero)
            {
                rig.velocity = new Vector3(0, inputVerticalDir.y * wallMoveForce * wallMoveForceMultiplier);
                controller.Stat.Stamina -= moveGrabWallStaminaPerSec * Time.fixedDeltaTime;
            }

            void EndInput()
            {
                Managers.Input.newInputManager.WaitGrabUpAction -= EndInput;

                rig.velocity = Vector3.zero;
                rig.gravityScale = 1;
            }
        }

        public void ClampVelocity()
        {
            //이동 멈춤값 제어
            ControlStopMoveVelocity();

            void ControlStopMoveVelocity()
            {
                if (isWallKickFirstJump)
                    return;

                if (isDashing)
                    return;

                if (isMovePressed)
                    return;

                if (Mathf.Abs(rig.velocity.x) > xStopVelocityRange)
                    return;

                rig.velocity = Vector2.Lerp(rig.velocity, new Vector2(0, rig.velocity.y), 0.3f);
            }
        }

        private void SubVariableDeltaTime()
        {
            moveSoundCurrentTime -= Time.fixedDeltaTime;
            currentJumpCountCoyotaTime -= Time.fixedDeltaTime;
            currentWallJumpCoyotaTime -= Time.fixedDeltaTime;
        }
    }
}
