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
    public float moveRange = 5f; // ������ �̵� ����

    float nextTime = 0f; // �����̵����� �ð�
    float moveDuration = 2f; // �̵��ϴ� �ð�
    bool isMoving = false; // ���� �̵� ������ ����

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

            // Flip ����
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
                    // ������ ���� ��ġ
                    Vector2 currentPosition = transform.position;

                    // ������ ������ �̵� ���� ����
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    randomDirection.Normalize();

                    // ���Ͱ� �̵��� ���ο� ��ġ ���
                    Vector2 newPosition = currentPosition + randomDirection * moveRange;

                    // �̵���������
                    newPosition.x = Mathf.Clamp(newPosition.x, currentPosition.x - moveRange, currentPosition.x + moveRange);

                    if (randomDirection != Vector2.zero)
                    {
                        //���������� 0�� �ƴϸ� ��������

                        //M_anim.SetBool("isWalk", true);
                        isMoving = true;
                       // Rb.velocity = randomDirection * moveSpeed;

                        //���� �޶��������� �ø��Ἥ ������ȯ
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


                    nextTime = Random.Range(1f, 3f); // 1�ʿ��� 3�� ������ ������ �ð� ����
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


                    nextTime = Random.Range(1f, 3f); // 1�ʿ��� 3�� ������ ������ �ð� ����
                }
            }
        }
    }

    private void Flip()
    {
        // ���� ���¸� ����
        isFacingRight = !isFacingRight;

        // SpriteRenderer�� �̿��Ͽ� ��������Ʈ�� ������

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
