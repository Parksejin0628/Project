using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    
    public GameObject gameManager;
    public GameObject puzzleUI;
    public float baseSpeed = 5f;
    public float speedMultiples = 1.0f;
    public float finallySpeed = 0.0f;
    public bool dog = false;

    GameObject interationObject;
    Rigidbody2D myRigid;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool isRunning = false;

    void Awake()
    {
        puzzleUI.SetActive(false);
        myRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        interationObject = null;
        
    }

    void FixedUpdate()
    {
        Move();
        Interation();
    }
    //ĳ���� �����Ӱ� ���õ� �Լ�
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

        
        finallySpeed = baseSpeed * speedMultiples * (isRunning ? 2:1);
        myRigid.velocity = moveVector * finallySpeed;
    }
    //��ȣ�ۿ��� �ϴ� �Լ�
    private void Interation()
    {
        if (interationObject != null)
        {
            //Debug.Log(interationObject + "�� ��ȣ�ۿ� ����");
        }
        if(dog && Input.GetButton("Jump") && interationObject != null)
        {
            SceneManager.LoadScene(3);
        }
        else if (Input.GetButton("Jump") && interationObject != null)
        {

            //Debug.Log(interationObject + "�� ��ȣ�ۿ� �Ϸ�");
            
            gameManager.GetComponent<GameManager>().onPuzzleUI();

        }

        return;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Object")
        { 
            interationObject = coll.gameObject;
            //Debug.Log(coll + "�� ��ȣ�ۿ� ����!");
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll == interationObject)
        {
            interationObject = null;
            //Debug.Log("��ȣ�ۿ� ����");
        }
        
    }

}
