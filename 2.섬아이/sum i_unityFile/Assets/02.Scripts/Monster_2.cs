using System.Collections;
using UnityEngine;

public class Monster_2 : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator m2_anim;
    public float moveSpeed;
    public float pauseTime = 2f;
    public LayerMask groundLayer; 
    bool isPaused = false;
    bool isFacingRight = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        m2_anim = GetComponent<Animator>();
        StartCoroutine(RandomPause());
    }

    void Update()
    {
        if (!isPaused)
        {
            Move();
        }
    }

    void Move()
    {
        m2_anim.SetBool("isRun", true);

        float moveDirection = isFacingRight ? 1 : -1;
        Vector3 movement = new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);

        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, groundLayer);

        if (!groundCheck.collider)
        {
            RaycastHit2D nearbyGroundCheck = Physics2D.Raycast(transform.position + new Vector3(moveDirection * 0.5f, 0, 0), Vector2.down, 2.0f, groundLayer);

            if (!nearbyGroundCheck.collider)
            {
                Flip();
            }
        }

        transform.Translate(movement);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = isFacingRight;
    }

    IEnumerator RandomPause()
    {
        while (true)
        {
            Flip();
            m2_anim.SetBool("isRun", false);
            isPaused = !isPaused;
            yield return new WaitForSeconds(Random.Range(1f, pauseTime));
        }
    }
}
