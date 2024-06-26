using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    //�����Ӱ� ���õ� ����
    public float moveSpeed = 3.0f;
    public float jumpPower = 10.0f;
    public float rayDistance = 0.2f;
    public int maxJumpCount = 1;
    public int jumpCount = 1;
    //�÷��̾� ����
    public int maxHp = 8;
    public int currentHp = 0;
    //�ǰݽ� ����
    public float knockBackPowerX;
    public float knockBackPowerY;
    public float knockBackTime = 0.25f;
    public float invincibilityTime = 1.0f;
    //��Ÿ
    public float jumpingBlockPower = 15.0f;
    public bool canMove = true;
    public bool isHit = true;
    public string nowSceneName = "Main Scene";
    public string endSceneName = "End Scene";

    Vector2 wasdVector;

    void Start()
    {
        AudioManager.Inst.PlayBGM("DoneEffect__Anxiety");
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHp < 0)
        {
            currentHp = 0;
            //GameOver
        }
        //���� ��� ä��� ����
        if(currentHp == maxHp && canMove)
        {
            canMove = false;
            AudioManager.Inst.StopAllCoroutines();
            AudioManager.Inst.PlaySFX("transition");
            
            Invoke("EndGame", 2.5f);
            StartCoroutine(GameManager.instance.Fadein(GameObject.Find("WhiteScene")));
        }
        if(currentHp>maxHp)
        {
            currentHp = maxHp;
            //GameEnding
            
        }
    }

    private void FixedUpdate()
    {
        //���� ȸ���� ���� �ڵ�
        RaycastHit2D hit;
        CheckIsGround(LayerMask.GetMask("Ground"), out hit);
        Move();
        if (rigidbody2D.velocity.y < 0 && hit == true)
        {
            jumpCount = maxJumpCount;
        }
        if(hit == true && hit.collider.CompareTag("JumpingBlock"))
        {
            AudioManager.Inst.PlaySFX("jumped");
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpingBlockPower);
        }
        //�ִϸ��̼� ���� ��ġ ����
        //Debug.Log(hit == true);
        anim.SetBool("isWalk", rigidbody2D.velocity.x != 0);
        anim.SetFloat("velocityY", rigidbody2D.velocity.y);
        anim.SetBool("isFloat", !hit);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //�������� ���� �� ����(hp) ȸ��
        if(coll.CompareTag("Orange") && currentHp<maxHp)
        {
            AudioManager.Inst.PlaySFX("equip_mandarine");
            currentHp++;
            coll.gameObject.SetActive(false);
        }
        //�ĵ��� ������ ���ӿ���
        if(coll.CompareTag("Wave"))
        {
            GameManager.instance.DeadPlayer();
        }
    }

    //�ǰݰ� ���õ� �κе� ����
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(isHit == false)
        {
            return;
        }
        if(coll.gameObject.CompareTag("Shell"))
        {
            AudioManager.Inst.PlaySFX("char_damaged");
            currentHp -= 1;
            StartCoroutine(KnockBack());
        }
        else if (coll.gameObject.CompareTag("StoneCrab"))
        {
            AudioManager.Inst.PlaySFX("char_damaged");
            currentHp -= 2;
            StartCoroutine(KnockBack());
        }
        else if (coll.gameObject.CompareTag("Mudskipper"))
        {
            AudioManager.Inst.PlaySFX("char_damaged");
            currentHp -= 1;
            StartCoroutine(KnockBack());
        }
        else if (coll.gameObject.CompareTag("Octopus"))
        {
            AudioManager.Inst.PlaySFX("char_damaged");
            currentHp -= 3;
            StartCoroutine(KnockBack());
        }
        else if (coll.gameObject.CompareTag("SolenStrictus"))
        {
            AudioManager.Inst.PlaySFX("char_damaged");
            currentHp -= 2;
            StartCoroutine(KnockBack());
        }
    }
    //�÷��̾� ������ ����
    void Move()
    {
        if(canMove == false)
        {
            return;
        }
        //wasdVector�� inputSystem�� �̿��� OnMove�� ȣ���Ͽ� ������ ����
        rigidbody2D.velocity = new Vector2(wasdVector.x * moveSpeed, rigidbody2D.velocity.y); 
        if(wasdVector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (wasdVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    bool CheckIsGround(LayerMask layerMask, out RaycastHit2D hit) //layerMask ��ũ�� ���� ������ üũ�ϴ� �Լ�
    {
        hit = Physics2D.BoxCast(transform.position + Vector3.down * transform.localScale.y / 4, transform.localScale / 3, 0, Vector2.down, rayDistance, layerMask);
        if (hit == true)
        {
            return true;
        }
        return false;
    }

    void OnMove(InputValue value)   //����Ű�� �����ų� �� �� ȣ��Ǵ� �Լ��̴�.
    {
        wasdVector = value.Get<Vector2>();     //���� ����Ű�� ������ ���� value�� Vector2�� ġȯ�Ѵ�. InputValue�� value�� ����ϱ� ���ϰ� Vector2�� ġȯ�� ���̴�.

        //������ addForce�� ó���ϸ鼭 maxSpeed�� ���صд��� ������ư�� ���� ���̿� ���� ���� ���̸� �ٲ�� �ϸ� ���?
    }

    void OnJump()   //�����̽��ٸ� ������ ȣ��Ǵ� �Լ��̴�.
    {
        if(canMove == false)
        {
            return;
        }
        if(CheckIsGround(LayerMask.GetMask("Ground"), out RaycastHit2D hit) && jumpCount == maxJumpCount)
        {
            AudioManager.Inst.PlaySFX("jumped");

            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
            jumpCount--;
        }
        else if(jumpCount > 0)
        {
            if (jumpCount == maxJumpCount)
            {
                Debug.Log("Check");
                jumpCount--;
                if(jumpCount <= 0)
                {
                    return;
                }
            }
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
            jumpCount--;
        }
    }
    //�ǰݽ� �ڷ� �з����� ���� �ð� ���� �ǰ� �鿪 ���·� ����� �ڵ�
    public IEnumerator KnockBack()
    {
        canMove = false;
        isHit = false;
        rigidbody2D.velocity = new Vector2(knockBackPowerX * (spriteRenderer.flipX ? 1 : -1), knockBackPowerY);
        yield return new WaitForSeconds(knockBackTime);
        canMove = true;
        for(int i=0; i<4; i++)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
            yield return new WaitForSeconds(invincibilityTime/8);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
            yield return new WaitForSeconds(invincibilityTime/8);
        }
        isHit = true;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(endSceneName);
    }

}
