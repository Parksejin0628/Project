using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Playables;
using Cinemachine;

public class Monster_info : MonoBehaviour
{

    public enum MonsterType
    {
        human,
        runner,
        heavy,
        trap
    }


    public MonsterType monsterType;
    [SerializeField]
    public float MonsterAttack { get { return GetMonsterAttack(); } }
    [SerializeField]
    public float moveSpeed { get { return GetMoveSpeed(); } }

    public float maxHP { get { return GetMAXHP(); } }
    private bool runner_start = false;

    private State state;
    private PlayerMove dashing;

    private float attackCoolTime = 2f; // 공격 쿨타임
    private bool isAttacking = false; //공격여부
    private float lastAttackTime = 0f; //마지막 공격 


    Transform playerTransform;
    bool isfollow = false;
    public float Monster_HP = 100;
    public GameObject M_area;
    public GameObject hpBarPrefab; // HP 바 프리팹
    private Slider Monster_hpbar; // 몬스터 체력바 
    private GameObject Monster_area;
    public GameObject UIparent;


    [HideInInspector]
    public Animator my_anim;
    private Rigidbody2D Rb; //리지드바디2d를 Rb 선언

    private bool isFacingRight = true;

    SpriteRenderer spriteRenderer;


    private bool search = true;
    public float moveRange = 5f; // 몬스터의 이동 범위


    float nextTime = 0f; // 다음이동까지 시간

    float moveDuration = 2f; // 이동하는 시간
    bool isMoving = false; // 현재 이동 중인지 여부

    private bool EnterPlayer = false;

    public GameObject boob;
    public GameObject trap_mouse;

    public GameObject blood_prefab;
    Transform bloodPosition;
    //보조무기와 상호작용을 위한 변수들
    public float bleedingTick = 0.5f;
    public float bloodReleaseFollowTime = 3.0f; // 몬스터가 bloodpack의 blood를 밟을 때 어그로가 풀리는 시간
    public float dustDecreasingSpeed = 1f;
    //보스
    public bool isBoss = false;


    private void Awake()
    {
        if (isBoss)
        {
            return;
        }
        state = FindObjectOfType<State>();
        dashing = FindObjectOfType<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        my_anim = GetComponent<Animator>(); // 애니메이터 컴포넌트
        Rb = GetComponent<Rigidbody2D>(); // 리지드바디 컴포넌트
        Rb.velocity = Vector3.zero;
        bloodPosition = dashing.transform.Find("blood_position");
        UIparent = GameObject.Find("UI_parent");
    }

    void Start()
    {
        if (isBoss == true)
        {
            return;
        }
        if (hpBarPrefab != null)
        {
            Monster_hpbar = Instantiate(hpBarPrefab, Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.3f, 0)), Quaternion.identity, UIparent.transform).GetComponent<Slider>();
            Monster_hpbar.gameObject.SetActive(false);

            Monster_area = Instantiate(M_area, transform.position, Quaternion.identity);
            Monster_area.transform.parent = transform;
            Monster_area.gameObject.SetActive(true);
        }
        
        my_anim.speed = 0.5f;

        GetMonsterAttack();
        GetMoveSpeed();



    }

    private void Update()
    {
        if (isBoss == true)
        {
            return;
        }
        if (Monster_hpbar != null)
        {
            Monster_hpbar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.05f, 0.8f, 0));
        }

        UpdateHP();



    }

    private void FixedUpdate()
    {
        if (isBoss == true)
        {
            return;
        }
        if (isfollow && playerTransform != null)
        {

            if (!EnterPlayer && monsterType != MonsterType.trap)
            {
                my_anim.SetBool("isRun", true);
            }

            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Flip 설정
            if ((isFacingRight && direction.x < 0) || (!isFacingRight && direction.x > 0))
            {
                Flip();
            }
            if (monsterType != MonsterType.trap)
            {
                transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.fixedDeltaTime);
            }
        }

        if (monsterType != MonsterType.trap)
        {
            FSM_huamn();
        }

    }


    private void Flip()
    {
        // 현재 상태를 반전
        isFacingRight = !isFacingRight;

        // SpriteRenderer를 이용하여 스프라이트를 뒤집음

        spriteRenderer.flipX = !isFacingRight;
    }

    private void UpdateHP()
    {
        if (Monster_hpbar != null)
        {
            Monster_hpbar.value = (float)Monster_HP / maxHP;
        }
        if(Monster_HP < 100)
        {
            Monster_hpbar.gameObject.SetActive(true);
        }

        if (Monster_HP <= 0)
        {
            if (monsterType == MonsterType.human || monsterType == MonsterType.heavy)
            {
                BossCtrl.instance.isCall = true;
            }
            my_anim.SetTrigger("isDead");
            Invoke("delete_monster", 0.5f);

        }
    }

    private void delete_monster()
    {
        Destroy(Monster_hpbar.gameObject);
        Destroy(gameObject);
    }



    
    private float GetMonsterAttack()
    {
        switch (monsterType)
        {
            case MonsterType.human:
                return 10f;
            case MonsterType.runner:
                return 30f;
            case MonsterType.heavy:
                return 35f;
            case MonsterType.trap:
                return 0f;
            default:
                return 20f; 
        }
    }

    //플레이어 속도 
    private float GetMoveSpeed()
    {
        switch (monsterType)
        {
            case MonsterType.human:
                return 1.5f;
            case MonsterType.runner:
                return 1.2f;
            case MonsterType.heavy:
                return 0.5f;
            case MonsterType.trap:
                return 0;
            default:
                return 0;        
        }
    }

   
    private float GetMAXHP()
    {
        switch (monsterType)
        {
            case MonsterType.human:
                return 100f;
            case MonsterType.runner:
                return 50f;
            case MonsterType.heavy:
                return 250f;
            case MonsterType.trap:
                return 25;
            default:
                return 0;
        }
    }
  


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBoss == true)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            if(monsterType == MonsterType.human)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.HumanZombieDetected);
            }
            if (monsterType == MonsterType.runner && !runner_start)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.DashZombieDetected);
                my_anim.SetTrigger("isRush");
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                Rb.velocity = directionToPlayer * moveSpeed * 2f;
                runner_start = false;
            }
            if(monsterType == MonsterType.heavy)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.HeavyZombieDetected);
            }
            if (monsterType == MonsterType.trap)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.TrapZombieDetected);
            }

            isfollow = true;


            if (monsterType != MonsterType.trap)
            {
                Monster_area.SetActive(false);
            }

            Monster_hpbar.gameObject.SetActive(true);
            search = false;

        }
        else if(other.CompareTag("Blood"))
        {
            StartCoroutine(ReleaseFollow(bloodReleaseFollowTime));
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBoss == true)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player") && monsterType == MonsterType.trap)
        {
            //Debug.Log("함정 좀비 가시권 안");
            if (Time.time - lastAttackTime >= attackCoolTime)
            {
                boobing();
                SoundManager.instance.PlaySfx(SoundManager.Sfx.TrapZombieSplit);
                lastAttackTime = Time.time;
            }
        }
        else if(collision.CompareTag("Dust"))
        {
            dustDecreasingSpeed = 0.5f;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBoss == true)
        {
            return;
        }
        if (collision.CompareTag("Dust"))
        {
            dustDecreasingSpeed = 1f;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBoss == true)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            my_anim.SetTrigger("isAttack");
            if (state.active_e == 4 || state.active_shift == 4)
            {
                Monster_HP -= state.PlayerAttackDamage;
                state.SetEnergy(state.currentEnergy - 4f);


            }
            if (state.active_e == 0 || state.active_shift == 0)
            {
                state.SetHP(state.currentHP - (0.8f * MonsterAttack));
                state.SetEnergy(state.currentEnergy - 2f);
         
            }
            else
            {
                state.SetHP(state.currentHP - MonsterAttack);
            }
            
            lastAttackTime = Time.time;
             
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isBoss == true)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player") && monsterType != MonsterType.trap)
        {

            Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            my_anim.SetBool("isRun", false);
            EnterPlayer = true;

            if (Time.time - lastAttackTime >= attackCoolTime)
            {
                my_anim.SetTrigger("isAttack");
                lastAttackTime = Time.time;

                if (!dashing.nodeal)
                {
                    if(state.active_e == 4 || state.active_shift == 4){
                        Monster_HP -= MonsterAttack;
                        state.SetEnergy(state.currentEnergy - 4f);

                    }
                    if (state.active_e == 0 || state.active_shift == 0)
                    {
                        state.SetHP(state.currentHP - (0.8f * MonsterAttack));
                        state.SetEnergy(state.currentEnergy - 2f);
                    }
                    else
                    {
                        state.SetHP(state.currentHP - MonsterAttack);
                    }

                    SpawnBloodEffect(bloodPosition.position,dashing.transform);



                }


            }

        }



    }
    private void SpawnBloodEffect(Vector3 position,Transform parent)
    {
        GameObject bloodEffect = Instantiate(blood_prefab, position, Quaternion.identity);
        bloodEffect.transform.parent = parent;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isBoss == true)
        {
            return;
        }
        my_anim.SetBool("isRun", true);
        //Rb.constraints = RigidbodyConstraints2D.None;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;


    }
    

    void boobing()
    {
        float SkillSpeed = 3f;
        Vector2 skillDirection = (dashing.transform.position - transform.position).normalized;

        GameObject skill1 = Instantiate(boob, trap_mouse.transform.position + Vector3.down * 0.4f, Quaternion.identity);
        Rigidbody2D skill1_rb = skill1.GetComponent<Rigidbody2D>();
        skill1_rb.velocity = skillDirection * SkillSpeed;

        // 플레이어 방향에 따른 스킬 프리팹의 방향 전환
        float angle = Mathf.Atan2(skillDirection.y, skillDirection.x) * Mathf.Rad2Deg;
        skill1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        

        Destroy(skill1, 4f);

    }



    void FSM_huamn()
    {
        if (search && monsterType != MonsterType.trap)
        {
            if (!isMoving)
            {
                nextTime -= Time.deltaTime;

                if (nextTime <= 0f)
                {
                    // 몬스터의 현재 위치
                    Vector2 currentPosition = transform.position;

                    // 몬스터의 무작위 이동 벡터 생성
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    randomDirection.Normalize();

                    // 몬스터가 이동할 새로운 위치 계산
                    Vector2 newPosition = currentPosition + randomDirection * moveRange;

                    // 이동범위제한
                    newPosition.x = Mathf.Clamp(newPosition.x, currentPosition.x - moveRange, currentPosition.x + moveRange);
                    newPosition.y = Mathf.Clamp(newPosition.y, currentPosition.y - moveRange, currentPosition.y + moveRange);

   
                    if (randomDirection != Vector2.zero)
                    {
                        //랜덤방향이 0이 아니면 수색진행

                        my_anim.SetBool("isWalk", true);
                        isMoving = true;
                        Rb.velocity = randomDirection * moveSpeed;

                        //방향 달라질때마다 플립써서 방향전환
                        if (randomDirection.x > 0 && !isFacingRight)
                        {
                            Flip();
                        }
                        else if (randomDirection.x < 0 && isFacingRight)
                        {
                            Flip();
                        }
                    }
                    else
                    {

                        my_anim.SetBool("isWalk", false);
                        isMoving = false;
                    }


                    nextTime = Random.Range(1f, 3f); // 1초에서 3초 사이의 무작위 시간 설정
                }
            }
            else
            {
                nextTime -= Time.deltaTime;

                if (nextTime <= 0f)
                {
                    my_anim.SetBool("isWalk", false);
                    Rb.velocity = Vector2.zero;
                    isMoving = false;


                    nextTime = Random.Range(1f, 3f); // 1초에서 3초 사이의 무작위 시간 설정
                }
            }
        }
    }

    public IEnumerator Bleeded(float bleedingDamage, float bleedingTime)
    {
        Debug.Log("출혈시작 - 출혈데미지 : " + bleedingDamage + "출혈시간 : " + bleedingTime) ;
        while (bleedingTime >= 0 || Monster_HP >= 0)
        {
            Debug.Log("출혈중..");
            Monster_HP -= bleedingDamage * bleedingTick;
            bleedingTime -= bleedingTick;
            yield return new WaitForSeconds(bleedingTick);
        }
        yield break;
    }

    public IEnumerator ReleaseFollow(float duration)
    {
        my_anim.SetBool("isRun", false);
        isfollow = false;
        yield return new WaitForSeconds(duration);
        isfollow = true;
        my_anim.SetBool("isRun", true);
    }

}


