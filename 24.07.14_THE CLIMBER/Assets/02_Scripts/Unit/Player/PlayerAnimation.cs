using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
    public class PlayerAnimation : MonoBehaviour
    {
        PlayerController controller;
        Rigidbody2D rig;
        SpriteRenderer spriteRenderer;
        Animator animator;

        public bool isAnimation = true; //애니메이션 실행 여부
        public bool isFlip = true; //플립 실행 여부
        public bool isShadow = true; //그림자 실행 여부

        public bool anim_IsGround { get { return controller.Move.isGround; } }

        public bool anim_CheckSkying;
        public bool anim_IsSkying { get { return !anim_IsGrabWall && !anim_TriggerDash && !anim_IsGround; } } //????????????????? 고민해보자


        public bool anim_CheckGrabWall;
        public bool anim_IsGrabWall { get { return controller.Move.IsGrabWall; } }

        public bool anim_CheckGrappling;

        public int anim_GrappleState = 0; // 0 = ropeOn, 1 = isGrappling

        public bool anim_IsJumping { get { return controller.Move.isJumping; } }
        public bool anim_IsDashing { get { return controller.Move.isDashing; } }
        public bool anim_IsInvincibilityTime { get { return controller.State.IsInvincibility; } }
        public bool anim_IsInabilityActTime { get { return controller.State.IsInabilityActTime; } }

        //trigger 변수
        public bool anim_TriggerHit; //데미지를 입음
        public bool anim_TriggerAttack;
        public bool anim_TriggerJump; //점프 시
        public bool anim_TriggerDash;

        public float currentSpeed { get { return GetComponentInParent<Rigidbody2D>().velocity.magnitude; } }

        [Header("그림자 관련")]
        public float normalDelayShadowTime = 0.05f;
        float currentDelayShadowTime = 0;
        public float shadowDestroyTime = 0.3f;
        private Color shadowNormalColor = new Color(100 / 255f, 100 / 255f, 100 / 255f);
        private Color shadowDashColor = new Color(50 / 255f, 255 / 255f, 255 / 255f);
        public float startShadowSpeed = 70f;
        public float coStopShadowTime = 1f;
        public Coroutine coMakeShadow = null;
        public Coroutine coStopShadow = null;

        public enum AnimationState
        {
            Grounded,
            Jump,
            Falling,
            GrabWall,
            Grappling,
            Dash,
            Hit,
        }

        public void MyAwake()
        {
            controller = GetComponent<PlayerController>();
            rig = controller.GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponentInChildren<Animator>();
        }

        public void MyFixedUpdate()
        {
            if (!controller.isAlive)
                return;

            CheckAnimation();
            CheckLookAtDirection();
            MakeShadow();

            SubVariableDeltaTime();
        }


        //캐릭터 사망 시 이거 실행
        public void OnUnitDie()
        {
            if (!isAnimation)
                return;

            animator.SetTrigger("isDie");
        }

        void CheckAnimation()
        {
            if (!isAnimation)
                return;

            animator.SetBool("isDashing", anim_IsDashing);
            animator.SetBool("isGround", anim_IsGround);
            animator.SetBool("isGrabWall", anim_IsGrabWall);
            animator.SetBool("isJumping", anim_IsJumping);
            animator.SetBool("isSkying", anim_IsSkying);
            animator.SetBool("isInvincibilityTime", anim_IsInvincibilityTime);
            animator.SetBool("isInabilityActTime", anim_IsInabilityActTime);

            animator.SetFloat("moveSpeed", currentSpeed);

            if (anim_TriggerHit)
            {
                anim_TriggerHit = false;
                animator.SetTrigger("isHit");
                return;
            }

            if (anim_TriggerAttack)
            {
                anim_TriggerAttack = false;
                animator.SetTrigger("isAttack");
                return;
            }

            if (anim_TriggerJump)
            {
                anim_TriggerJump = false;
                animator.SetTrigger("isJump");
                return;
            }

            if (anim_TriggerDash)
            {
                anim_TriggerDash = false;
                animator.SetTrigger("isDash");
                return;
            }
        }

        void CheckLookAtDirection()
        {
            if (!isFlip)
                return;

            //1순위
            if (anim_IsDashing)
            {
                if (controller.Move.LookDir.x == 1)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;
                return;
            }

            //3순위
            if (!controller.Move.isGround && controller.Move.isGrabLeftWall)
            {
                spriteRenderer.flipX = false;
                return;
            }
            else if (!controller.Move.isGround && controller.Move.isGrabRightWall)
            {
                spriteRenderer.flipX = true;
                return;
            }

            //4순위
            if (controller.Move.LookDir.x == 1)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }


        public bool immediateMakeShadow = false;
        private void MakeShadow()
        {
            if (!CheckMakeShadowCondition())
                return;

            //시간이 조금 지났으면 생성 못하도록
            if (coMakeShadow == null)
                coMakeShadow = StartCoroutine(Co_MakeShadow());
        }

        private bool CheckMakeShadowCondition()
        {
            if (immediateMakeShadow == true)
            {
                immediateMakeShadow = false;
                return true;
            }

            //진입 조건
            if (!isShadow)
                return false;

            //특정 속도 이하 들어오지 못하게
            if (rig.velocity.magnitude < startShadowSpeed)
            {
                if (coMakeShadow != null && coStopShadow == null)
                    coStopShadow = StartCoroutine(Co_StopShadow()); //진입 금지! 멈출 준비해!

                return false;
            }

            return true;
        }

        IEnumerator Co_MakeShadow()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                if (!controller.State.IsDashing)
                    continue;

                if (currentDelayShadowTime > 0)
                    continue;

                //사전 조건
                currentDelayShadowTime = normalDelayShadowTime;
                GameObject shadowGo = Managers.Resource.Instantiate("Unit/PlayerShadowImage");
                SpriteRenderer shadowSr = shadowGo.GetComponent<SpriteRenderer>();

                //로직
                shadowGo.transform.position = transform.position;
                shadowSr.sprite = spriteRenderer.sprite;
                if (controller.State.IsDashing)
                    shadowSr.color = shadowDashColor;
                else
                    shadowSr.color = shadowNormalColor;
                shadowSr.flipX = spriteRenderer.flipX;
                StartCoroutine(Extension.Co_FadePlay(null, shadowSr, Extension.Ease.EaseInSine, shadowDestroyTime, 1, 0));

                //사후 조건
                Destroy(shadowGo, shadowDestroyTime + 0.1f);
            }
        }

        IEnumerator Co_StopShadow()
        {
            yield return new WaitForSeconds(coStopShadowTime);
            if (coMakeShadow != null)
            {
                StopCoroutine(coMakeShadow);
                coMakeShadow = null;
            }
            coStopShadow = null;
        }

        private void SubVariableDeltaTime()
        {
            currentDelayShadowTime -= Time.fixedDeltaTime;
        }
    }
}