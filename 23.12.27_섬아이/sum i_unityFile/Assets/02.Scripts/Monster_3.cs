using System.Collections;
using UnityEngine;

public class Monster_3 : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator m3_anim;
    Rigidbody2D Rig2;
    public float moveSpeed;
    public float jumpForce;
    public LayerMask Ground;
    public float pauseTime = 2f;
    bool isPaused = false;
    bool isFacingRight = true;
    bool isGrounded = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        m3_anim = GetComponent<Animator>();
        Rig2 = GetComponent<Rigidbody2D>();
        StartCoroutine(PauseAndMove());
    }

    void Update()
    {
        if (!isPaused && isGrounded)
        {
            m3_anim.SetBool("isRun", true);


            float moveDirection = isFacingRight ? -0.5f : 0.5f;

            Vector2 moveForce = new Vector2(moveDirection * moveSpeed, jumpForce);

            Rig2.AddForce(moveForce, ForceMode2D.Impulse);

            Flip();

            isGrounded = false;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & Ground) != 0)
        {
            StartCoroutine(GroundCheckCoroutine());
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = isFacingRight;
    }

    IEnumerator PauseAndMove()
    {
        while (true)
        {
            Flip();
            isPaused = !isPaused;
            m3_anim.SetBool("isRun", false);
            yield return new WaitForSeconds(pauseTime);
        }
    }

    IEnumerator GroundCheckCoroutine()
    {
        yield return new WaitForSeconds(1f);
        isGrounded = true;
    }
}
