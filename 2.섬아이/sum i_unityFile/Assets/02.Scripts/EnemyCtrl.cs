using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyCtrl : MonoBehaviour
{
    public MonsterType monsterType;
    public enum MonsterType
    {
           clam,
           crab,
           goby,
           octopus,
           taste_clam
    }
    /*
    private int GetMonsterAttack()
    {
        switch (monsterType)
        {
            case MonsterType.clam:
                return 1;
            case MonsterType.crab:
                return 2;
            case MonsterType.goby:
                return 1;
            case MonsterType.octopus:
                return 3;
            case MonsterType.taste_clam:
                return 2;
        }
    }
   private int GetMoveSpeed()
    {
        switch (monsterType)
        {
            case MonsterType.clam:
                return 1;
            case MonsterType.crab:
                return 2;
            case MonsterType.goby:
                return 1;
            case MonsterType.octopus:
                return 3;
            case MonsterType.taste_clam:
                return 2;
        }
    } */

   // public int MonsterAttack { get { return GetMonsterAttack(); } }
    [SerializeField]
    //public int moveSpeed { get { return GetMoveSpeed(); } }
    private Rigidbody2D Rb;
    public GameObject M_area;
    public Animator M_anim;
    private bool isFacingRight = true;
    private bool search = true;
    bool isfollow = false;
    Transform playerTransform;
    private GameObject Monster_area;
    SpriteRenderer spriteRenderer;
    public float moveRange = 5f; // 몬스터의 이동 범위

    float nextTime = 0f; // 다음이동까지 시간
    float moveDuration = 2f; // 이동하는 시간
    bool isMoving = false; // 현재 이동 중인지 여부

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        Monster_area = Instantiate(M_area, transform.position, Quaternion.identity);
        Monster_area.transform.parent = transform;
        Monster_area.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (isfollow && playerTransform != null)
        {

            M_anim.SetBool("isRun", true);

            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Flip 설정
            if ((isFacingRight && direction.x < 0) || (!isFacingRight && direction.x > 0))
            {
                Flip();
            }
         //   transform.Translate(direction * moveSpeed * Time.fixedDeltaTime);
        }

    }


    void FSM_huamn()
    {
        if (search)
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

                    if (randomDirection != Vector2.zero)
                    {
                        //랜덤방향이 0이 아니면 수색진행

                        //M_anim.SetBool("isWalk", true);
                        isMoving = true;
                       // Rb.velocity = randomDirection * moveSpeed;

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

                        //M_anim.SetBool("isWalk", false);
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
                    //M_anim.SetBool("isWalk", false);
                    Rb.velocity = Vector2.zero;
                    isMoving = false;


                    nextTime = Random.Range(1f, 3f); // 1초에서 3초 사이의 무작위 시간 설정
                }
            }
        }
    }

    private void Flip()
    {
        // 현재 상태를 반전
        isFacingRight = !isFacingRight;

        // SpriteRenderer를 이용하여 스프라이트를 뒤집음

        spriteRenderer.flipX = !isFacingRight;
    }

    public void delete_monster()
    {
        Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            Monster_area.SetActive(false);
            search = false;
            isfollow = true;
        }
    }
}
