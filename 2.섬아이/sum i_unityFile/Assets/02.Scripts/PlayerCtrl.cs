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
    //움직임과 관련된 변수
    public float moveSpeed = 3.0f;
    public float jumpPower = 10.0f;
    public float rayDistance = 0.2f;
    public int maxJumpCount = 1;
    public int jumpCount = 1;
    //플레이어 정보
    public int maxHp = 8;
    public int currentHp = 0;
    //피격시 정보
    public float knockBackPowerX;
    public float knockBackPowerY;
    public float knockBackTime = 0.25f;
    public float invincibilityTime = 1.0f;
    //기타
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
        //귤을 모두 채우면 엔딩
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
        //점프 회복을 위한 코드
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
        //애니메이션 관련 수치 조정
        //Debug.Log(hit == true);
        anim.SetBool("isWalk", rigidbody2D.velocity.x != 0);
        anim.SetFloat("velocityY", rigidbody2D.velocity.y);
        anim.SetBool("isFloat", !hit);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //오렌지를 먹을 시 점수(hp) 회복
        if(coll.CompareTag("Orange") && currentHp<maxHp)
        {
            AudioManager.Inst.PlaySFX("equip_mandarine");
            currentHp++;
            coll.gameObject.SetActive(false);
        }
        //파도에 닿으면 게임오버
        if(coll.CompareTag("Wave"))
        {
            GameManager.instance.DeadPlayer();
        }
    }

    //피격과 관련된 부분들 구현
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
    //플레이어 움직임 조작
    void Move()
    {
        if(canMove == false)
        {
            return;
        }
        //wasdVector는 inputSystem을 이용해 OnMove를 호출하여 가져온 정보
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
    bool CheckIsGround(LayerMask layerMask, out RaycastHit2D hit) //layerMask 태크를 가진 땅인지 체크하는 함수
    {
        hit = Physics2D.BoxCast(transform.position + Vector3.down * transform.localScale.y / 4, transform.localScale / 3, 0, Vector2.down, rayDistance, layerMask);
        if (hit == true)
        {
            return true;
        }
        return false;
    }

    void OnMove(InputValue value)   //방향키를 누르거나 땔 때 호출되는 함수이다.
    {
        wasdVector = value.Get<Vector2>();     //현재 방향키의 정보를 담은 value를 Vector2로 치환한다. InputValue인 value를 사용하기 편하게 Vector2로 치환한 것이다.

        //점프도 addForce로 처리하면서 maxSpeed를 정해둔다음 점프버튼을 누른 길이에 따라 점프 높이를 바뀌도록 하면 어떨까?
    }

    void OnJump()   //스페이스바를 누르면 호출되는 함수이다.
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
    //피격시 뒤로 밀려나며 일정 시간 동안 피격 면역 상태로 만드는 코드
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
