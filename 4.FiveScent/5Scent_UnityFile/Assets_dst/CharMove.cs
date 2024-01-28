using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    float moveSpeed = 5f;

    Rigidbody2D myRigid;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool isRunning = false; // Flag to indicate if the character is running

    void Awake()
    {
        myRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 moveVector = new Vector2(moveHorizontal, moveVertical).normalized;

        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = false;
            animator.SetBool("isWalk", true);
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = true;
            animator.SetBool("isWalk", true);
        }
        else if (moveVertical < 0 || moveVertical > 0)
        {
            animator.SetBool("isWalk", true);
        }
        else if (moveHorizontal == 0 && moveVertical == 0)
        {
            animator.SetBool("isWalk", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            animator.SetBool("isRun", true);
        
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            animator.SetBool("isRun", false);
        }

        // Adjust move speed based on running state
        float currentMoveSpeed = isRunning ? moveSpeed * 2 : moveSpeed;
        myRigid.velocity = moveVector * currentMoveSpeed;
    }
}
