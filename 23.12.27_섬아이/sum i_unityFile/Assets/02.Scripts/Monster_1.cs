using System.Collections;
using UnityEngine;

public class Monster_1 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform finder;
    public float bulletSpeed = 10f; // 총알 속도
    public float raycastDistance = 100f; // 레이캐스트 거리
    
    // 플레이어 태그
    Animator m1_anim;
    private bool isFacingRight = true;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    public Vector2 direction;

    public float attackspeed = 2f;
    void Start()
    {
        m1_anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start the shooting coroutine
        StartCoroutine(ShootEveryThreeSeconds());
    }
    void Update()
    {

    }
    IEnumerator ShootEveryThreeSeconds()
    {
        while (true)
        {

            yield return new WaitForSeconds(attackspeed);

            // Check for the player and shoot
            RaycastHit2D hitLeft = Physics2D.Raycast(finder.position, Vector2.left, raycastDistance);
            RaycastHit2D hitRight = Physics2D.Raycast(finder.position, Vector2.right, raycastDistance);

            if ((hitLeft.collider != null && hitLeft.collider.CompareTag("Player")))
            {
                direction = Vector2.left;
                m1_anim.SetTrigger("isAttack");
                AudioManager.Inst.PlaySFX("shell_spit");
                FlipToPlayer();


            }
            else if ((hitRight.collider != null && hitRight.collider.CompareTag("Player")))
            {

                direction = Vector2.right;
                m1_anim.SetTrigger("isAttack");
                AudioManager.Inst.PlaySFX("shell_spit");
                FlipToPlayer();


            }
        }
    }
    private void FlipToPlayer()
    {
        if (playerTransform.position.x > transform.position.x && isFacingRight)
        {
            Flip();
        }
        else if (playerTransform.position.x < transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        spriteRenderer.flipX = !isFacingRight;
    }

    public void ShootBullet()
    {
        // 총알을 생성하고 발사 방향을 설정
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        Destroy(bullet, 4f);
    }
}
