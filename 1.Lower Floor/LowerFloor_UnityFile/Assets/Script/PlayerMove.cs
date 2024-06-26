using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    public GameObject elev1;
    public GameObject elev2;

    public CutSceneController UI;

    private State state;
    private Monster_info monster;
    //private Skill skill;
    private bool dashpress = false;


    public UI_CoolTime coolTimeUI; // UI_CoolTime 클래스의 인스턴스에 접근하기 위한 변수



    private Rigidbody2D playerRb; //리자드바디2d를 playerRb로 선언
    private Animator myAnim;

    //원래 이동속도
    float walkspeed;
    public float playerMoveSpeed;
    
    private bool isRun; //달리기 여부 
    public bool canMove = true;
    //대쉬 변수 
    private bool canDash = true; //대쉬가능여부
    private bool isDashing; //대쉬여부
    private float dashingPower = 10f; //대쉬이동거리
    private float dashingTime = 0.2f; //대쉬이동시간
    private float dashingCooldown = 0f; //대쉬 쿨타임
    //공격변수 
    private bool isAttack = false; //공격확인 플래그(중복 공격 방지)
    private bool Attacking = false;  //공격키 입력 확인 변수
    public GameObject bloodprefab;
    //스킬 관련 변수 
    int skillnum;
    float skillCoolTIme = 3f;
    float lastskillTime = 0;
    float origin_playerdamage;
    float attackDamageRatio = 1.0f;
    //혈취처방
    bool Trigger_skill1 = false;
    public GameObject R_sword;
    public GameObject L_sword;
    //엑스레이
    public GameObject r_xray;
    public GameObject l_xray;
    bool Trigger_skill3 = false;

    //심호흡
    public GameObject healpart;
    private Map_Unit map;


    [SerializeField] private TrailRenderer tr;
    
    public  bool nodeal = false; //무!!!적!!!!!!!!판!!!정!~!!!기  <- 미쳐버린 서경식
    [SerializeField]
    public GameObject 혈취처방;
    //상호작용과 관련된 변수들
    private GameObject interactionObject;   //상호작용하는 오브젝트
    private int interactionObjectCount;     //상호작용 범위에 들어온 오브젝트 개수, 상호작용 범위 안에 여러 개의 상호작용 오브젝트가 있는 경우를 처리하기 위함
    public GameObject Image_PressF;         //F키를 눌러달라는 UI 오브젝트
    //아이템 버리기와 관련된 변수들
    private bool[] isDump = { false, false, false };    //현재 버리기 버튼을 누르고 있는지 확인하는 변수
    public const float dumpTime = 1.25f;                      //버리기까지 버튼을 눌러야 하는 시간
    //아이템 회복량과 관련된 변수들
    private const float bandage_HPRecoveryPercent = 0.1f;
    private const float painkiller_energyRecoveryPercent = 0.5f;
    private const float epinephrine_energyRecoveryPercent = 1.0f;
    private const int can_hungerRecoveryAmount = 1;
    private const int cupramen_hungerRecoveryAmount = 2;
    //무기와 관련된 변수들
    public Dictionary<InteractionObjectName, GameObject> projectile = new Dictionary<InteractionObjectName, GameObject>();
    public GameObject[] projectile_object;
    public InteractionObjectName[] projectile_name;
    public bool canUsePipe;
    private int pipeCount;
    private bool canUseBloodpack = true;
    public float bloodPackDelay = 0.1f;
    private bool canUseFireextinguisher = true;
    public float fireextinguisherDelay = 0.1f;
    private bool weaponButtonDown = false;

    public GameObject floor1to2;
    public GameObject floor2to3;
    public GameObject floor3to4;


    public GameObject floor4to3;
    public GameObject floor3to2;
    public GameObject floor2to1;
    public GameObject floor1to0;


    public Vector3 floor1to2_position = new Vector3(-108.1226f, -48f, 0f);
    public Vector3 floor2to3_position = new Vector3(65.94f, 30.11f, 0f);
    public Vector3 floor3to4_position = new Vector3(-11f, -0.48f, 0f);

    public Vector3 floor4to3_position = new Vector3(139.61f, 28.44f, 0f);
    public Vector3 floor3to2_position = new Vector3(-44f, -66f, 0);
    public Vector3 floor2to1_position = new Vector3(298.2f, 53f, 0);
    public Vector3 floor1to0_position = new Vector3(-130.99f, 18.55f, 0);


    private SpriteRenderer spriteRenderer;


    public Material newMaterial;
    public Material origin_Material;

    private void Awake()
    {
        
        
        map = GameObject.Find("GameManager").GetComponent<Map_Unit>();
        playerRb = GetComponent<Rigidbody2D>(); //리자드바디 컴포넌트
        playerRb.velocity = Vector3.zero;
        myAnim = GetComponent<Animator>(); //애니메이터 컴포넌트
        state = GetComponent<State>(); // 스탯 스크립트 연결
        //skill = GetComponent<Skill>();

        origin_playerdamage = state.PlayerAttackDamage;
       
        Image_PressF.SetActive(false);      //'F키를 누르시오' UI 비활성화 시켜놓기
        //투사체 오브젝트 정보 딕셔너리 구조로 저장
        for (int i = 0; i < projectile_object.Length; i++)
        {
            //ItemSprite 딕셔너리 자료구조 초기화
            projectile.Add(projectile_name[i], projectile_object[i]);
        }
        canUsePipe = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
       walkspeed = playerMoveSpeed;
    }

    private void Update()
    {
        if (!CutSceneController.instance.CanMove)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }
        SkillInput();
        checkInput();       //입력한 버튼이 있는지 확인
                            //객체끼리 충돌시 밀리지 않기 가속도 = 0
                            // GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
    private void checkInput()       //입력한 버튼이 있는지 확인하는 함수
    {
        //마우스 좌클릭 + Attack 애니메이션이 진행중이지 않을 때,
        if ((Input.GetMouseButtonDown(0)) && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            attackDamageRatio = 1.0f;
            Attacking = true;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetPlayerDirection(mousePos - new Vector2(transform.position.x, transform.position.y));


        }


        //F키를 누르면 상호작용
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (interactionObjectCount > 0)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.SlotFill);
                InteractionManager.instance.Interact(interactionObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && (canDash) && !isDashing)
        {
            StartCoroutine(Dash());  // 대쉬 코루틴 실행
        }
        //숫자 1, 2, 3번을 누르면 각각 아이템의 버리기 카운팅을 시작한다. dumpTime동안 떼지 않으면 아이템을 버린다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            isDump[0] = true;
            StartCoroutine(DumpItem(0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            isDump[1] = true;
            StartCoroutine(DumpItem(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            
            isDump[2] = true;
            StartCoroutine(DumpItem(2));
        }
        //숫자 1, 2, 3번을 떼면 버리기 카운팅을 중단하고 해당 아이템을 사용한다.
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            
            isDump[0] = false;
            UseItem(0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            isDump[1] = false;
            UseItem(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            isDump[2] = false;
            UseItem(2);
        }
        if(Input.GetMouseButton(1))/*우클릭*/
        {
            UseWeapon();
        }
        if(Input.GetMouseButtonUp(1))
        {
            weaponButtonDown = false;
        }
    }
    private void FixedUpdate() 
    {
        if (!CutSceneController.instance.CanMove)
        {
            return;
        }
        Move();
    }

    private void Move()
    {
        if (!canMove)
        {
            playerRb.velocity = Vector2.zero;
            return;
        }
        if (isDashing)
        {
            return;
        }


        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime; //사용자 방향키 입력받아 이동속도 계산
                                                                                                                                                          //Debug.Log(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerMoveSpeed * Time.fixedDeltaTime);



        myAnim.SetFloat("MoveX", playerRb.velocity.x);             //파라미터 선언
        myAnim.SetFloat("MoveY", playerRb.velocity.y);


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));                    //마지막으로 이동한 방향 확인하기 위한 파라미터 선언
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }

        if (dashpress && canDash && !isDashing)
        {
            StartCoroutine(Dash());

            dashpress = false; // 대쉬 입력을 처리한 후에는 리셋
        }
        if (Attacking)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.BasicAttackWind);
            myAnim.SetTrigger("isAttack"); //공격애니메이션 실행
            isAttack = false; //공격플래그 비활성화
            Attacking = false; //공격모션 비활성화
        }
    }




    private void SkillInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SkillEvenet(state.active_shift);     

        }
        if (Input.GetKeyDown(KeyCode.E)){
            SkillEvenet(state.active_e);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (coolTimeUI != null && coolTimeUI.remainingTime == 0)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.UseUlt1);
                coolTimeUI.Trigger_Skill();
                Vector3 healEffectPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                GameObject healEffect = Instantiate(healpart, healEffectPosition, Quaternion.identity);
                Destroy(healEffect, 0.3f);
                StartCoroutine(UltraIncreaseEnergyOverTime(10f));


            }
        }
    }

    private void SetPlayerDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;
        direction.Normalize();
        myAnim.SetFloat("AttackX", direction.x);
        myAnim.SetFloat("AttackY", direction.y);
        myAnim.SetFloat("LastMoveX", direction.x);
        myAnim.SetFloat("LastMoveY", direction.y);

    }

    private IEnumerator Dash()
    {

        SoundManager.instance.PlaySfx(SoundManager.Sfx.UseDash);


        state.SetEnergy(state.currentEnergy - 10f);

        canDash = false;
      
        isDashing = true;
        nodeal = true;
    
        float originalGravity = playerRb.gravityScale; //중력 수치 
        playerRb.gravityScale = 0f; //중력 0으로 만듬 


        float dashDirectionX = Input.GetAxisRaw("Horizontal");
        float dashDirectionY = Input.GetAxisRaw("Vertical");
    
        Vector2 dashDirection = new Vector2(dashDirectionX, dashDirectionY).normalized;
        playerRb.velocity = dashDirection * dashingPower;
        
        tr.emitting = true;


        yield return new WaitForSeconds(dashingTime); //해당 변수만큼 기다림
        tr.emitting = false; //이펙트 종료 
        playerRb.gravityScale = originalGravity; 
        isDashing = false;
        nodeal = false;
    
        yield return new WaitForSeconds(dashingCooldown);  //해당 변수만큼 기다림        
        canDash = true;


    }

    private IEnumerator UltraIncreaseEnergyOverTime(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {

            // 시간이 흐를수록 currentEnergy를 증가시킴 (예시로 10초 동안 0에서 100까지 증가)
            state.currentEnergy = Mathf.Lerp(0f, 100f, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            spriteRenderer.material = newMaterial;
            state.currentEnergy = 100f;

            yield return null; // 한 프레임 대기
        }

        state.currentEnergy = 100f;
        spriteRenderer.material = origin_Material;
    }

    //스킬1
    void SkillEvenet(int skillnum)
    {
        


        //집단복부절개
        if (skillnum == 2 && state.currentEnergy >= 15f)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.BasicAttackWind);
            state.SetEnergy(state.currentEnergy - 15f);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetPlayerDirection(mousePosition - new Vector2(transform.position.x, transform.position.y));
            myAnim.SetTrigger("isdao");
            isAttack = false;
            attackDamageRatio = 2.0f;
        }
        //심호흡
        if (skillnum == 3 && state.currentEnergy >= 30f)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UseActive4);
            state.SetEnergy(state.currentEnergy - 30f);
            state.SetHP(state.currentHP + 25f);
            Vector3 healEffectPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);


            GameObject healEffect = Instantiate(healpart, healEffectPosition, Quaternion.identity);
            Destroy(healEffect, 0.3f);
        }
        //혈취처방
        if (skillnum == 5 && state.currentEnergy >=  10f) {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UseActive1);
            state.SetEnergy(state.currentEnergy - 10f);
            Trigger_skill1 = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetPlayerDirection(mousePosition - new Vector2(transform.position.x, transform.position.y));
            Attacking = true;

        }

        //엑스레이
        if(skillnum == 6 && state.currentEnergy >= 25f)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UseActive3);
            state.SetEnergy(state.currentEnergy - 25f);
            Trigger_skill3 = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetPlayerDirection(mousePosition - new Vector2(transform.position.x, transform.position.y));
            Attacking = true;
            spriteRenderer.material = newMaterial;
            attackDamageRatio = 6.0f;
            StartCoroutine(originsprite(0.8f));

        }

    }


    
    private void L_throw()
    {
        if (Trigger_skill1)
        {

            L_sword.SetActive(true);
        }
    }
    private void del_L_throw()
    {
        
        L_sword.SetActive(false);
        Trigger_skill1 = false;

    }
    private void R_throw()
    {
        if (Trigger_skill1)
        {

            R_sword.SetActive(true);
        }
    }
    private void del_R_throw()
    {
        R_sword.SetActive(false);
        Trigger_skill1 = false;

    }




    private void L_xray()
    {
        if (Trigger_skill3)
        {

            l_xray.SetActive(true);
        }
    }
    private void del_L_xray()
    {

        l_xray.SetActive(false);
        Trigger_skill3 = false;
        state.PlayerAttackDamage = origin_playerdamage;

    }
    private void R_xray()
    {
        if (Trigger_skill3)
        {

            r_xray.SetActive(true);
        }
    }
    private void del_R_xray()
    {

        r_xray.SetActive(false);
        Trigger_skill3 = false;
        state.PlayerAttackDamage = origin_playerdamage;

    }
    private Vector2 GetPlayerDirection()
    {
        //플레이어 방향 리턴
        return new Vector2(myAnim.GetFloat("LastMoveX"), myAnim.GetFloat("LastMoveY")).normalized;
    }

    private void SpawnBloodEffect(Vector3 position, Transform parent)
    {
        GameObject bloodEffect = Instantiate(bloodprefab, position, Quaternion.identity);
        bloodEffect.transform.parent = parent;
    }

    private void MovePlayerToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        
        // 충돌된 태그가 몬스터이고, 공격 모션이 실행되지 않았을 때, 공격 플래그가 활성화되지 않았을 때
        if (other.CompareTag("Monster") && !isAttack)
        {
            Debug.Log("몬스터가 닿았습니다");
            // 태그된 객체의 몬스터 인포 컴포넌트를 가져옴
            Monster_info monster = other.GetComponent<Monster_info>();
            if(monster.isBoss == true)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.BasicAttack);
                monster.Monster_HP -= state.PlayerAttackDamage * attackDamageRatio;
                isAttack = true;
                return;
            }
            if (monster != null)
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.BasicAttack);
                monster.my_anim.SetTrigger("isHurt");
                // 몬스터 스크립트에 몬스터 체력에 state 스크립트의 공격값을 뺌
                monster.Monster_HP -= state.PlayerAttackDamage * attackDamageRatio;
                
                Debug.Log(state.PlayerAttackDamage);
                // 공격 플래그 활성화
                isAttack = true;
                SpawnBloodEffect(monster.transform.position, monster.transform);

            }
        }

        if (other.CompareTag("Elevator")){
            Debug.Log("엘베 탔슴");
            SoundManager.instance.PlaySfx(SoundManager.Sfx.ElevatorMove);
            //if (other.gameObject == floor1to2)
            //{
            //    MovePlayerToPosition(floor1to2_position);
            //    map.floor = 2;
            //    map.set_help();
            //}
            //else if (other.gameObject == floor2to3)
            //{
            //    MovePlayerToPosition(floor2to3_position);
            //    map.floor = 3;
            //    map.set_help();
            //}
            //else if (other.gameObject == floor3to4)
            //{
            //    MovePlayerToPosition(floor3to4_position);
            //    map.floor = 4;
            //    map.set_help();
            //}
            if (other.gameObject == floor4to3)
            {
                MovePlayerToPosition(floor4to3_position);
                map.floor = 3;
                map.set_help();
                SoundManager.instance.PlayBgm(SoundManager.Bgm.VIP, false);
                SoundManager.instance.PlayBgm(SoundManager.Bgm.Dungeon, true);
            }
            else if (other.gameObject == floor3to2)
            {
                MovePlayerToPosition(floor3to2_position);
                map.floor = 2;
                map.set_help();
            }
            else if (other.gameObject == floor2to1)
            {
                SoundManager.instance.PlayBgm(SoundManager.Bgm.Dungeon, false);
                MovePlayerToPosition(floor2to1_position);
                map.floor = 1;
                map.set_help();
                SoundManager.instance.PlayBgm(SoundManager.Bgm.Done, true);
            }
            else if (other.gameObject == floor1to0)
            {
                
                MovePlayerToPosition(floor1to0_position);
                map.floor = 0;
                SoundManager.instance.PlayBgm(SoundManager.Bgm.Done, false);
                map.boss_sceen1.SetActive(true);
                map.boss_sceen2.SetActive(true);
            }


        }
       
    
        //InteractionObject 범위 안에 들어간 경우
        if(other.CompareTag("InteractionObject"))
        {
            //아직 상호작용 범위 안에 들어간 오브젝트가 없었던 경우에만 pressF UI 출력
            if(interactionObjectCount == 0)
            {
                Image_PressF.SetActive(true);
            }
            interactionObjectCount++;
            interactionObject = other.gameObject;   //InteractionObject를 현재 접촉한 오브젝트로 변경
        }
        if (other.gameObject == elev1)
        {
            Vector3 newPosition = new Vector3(-84, -71, 0);
            transform.position = newPosition;

        }
        else if (other.gameObject == elev2)
        {
            Vector3 newPosition = new Vector3(164, -17, 0);
            transform.position = newPosition;

        }

        if (other.CompareTag("RecyclingObject"))
        {
            Destroy(other.gameObject);
            ref Item usingWeapon = ref state.auxiliaryWeapon;
            if(usingWeapon.name == InteractionObjectName.PIPE)
            {
                canUsePipe= true;
                usingWeapon.count = pipeCount;
            }
        }

        if(other.CompareTag("Shock"))
        {
            if (state.active_e == 4 || state.active_shift == 4)
            {
                BossCtrl.instance.GetComponent<Monster_info>().Monster_HP -= state.PlayerAttackDamage;
            }
            if (state.active_e == 0 || state.active_shift == 0)
            {
                state.SetHP(state.currentHP - 50f * 0.8f);
                
            }
            else
            {
                state.SetHP(state.currentHP - 50f);
            }
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Dust"))
        {
            if (state.active_e == 1 || state.active_shift == 1)
            {
                attackDamageRatio = 2f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //상호작용 오브젝트의 상호작용 범위 밖으로 나갈 경우 pressF UI를 닫는다.
        if(other.CompareTag("InteractionObject"))
        {
            interactionObjectCount--;
            if(interactionObjectCount == 0)
            {
                //Debug.Log("패널 닫음");
                Image_PressF.SetActive(false);
            }
        }
    }


    private void UseItem(int itemIndex)     //아이템을 사용하는 함수이다. 아이템 종류에 따른 기능을 수행한다.
    {
        ref Item usingItem = ref state.item[itemIndex];
        //사용하려는 아이템의 개수가 없는 경우
        if(usingItem.count <= 0)
        {
            Debug.Log(itemIndex + 1 + "번째 아이템 남은 개수가 없음");
            return;
        }
        usingItem.count--;
        if(usingItem.name == InteractionObjectName.BANDAGE)  //붕대
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.ItemUse);
            state.SetHP(state.currentHP + state.maxHP * bandage_HPRecoveryPercent);
        }
        else if (usingItem.name == InteractionObjectName.PAINKILLER) //진통제
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.ItemUse);
            state.SetEnergy(state.currentEnergy + state.maxEnergy * painkiller_energyRecoveryPercent);
        }
        else if (usingItem.name == InteractionObjectName.EPINEPHRINE)    //에피네프린
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.Adrenaline);
            state.SetEnergy(state.currentEnergy + state.maxEnergy * epinephrine_energyRecoveryPercent);
        }
        else if (usingItem.name == InteractionObjectName.CAN)    //통조림
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.ItemUse);
            state.SetHunger(state.currentHunger + can_hungerRecoveryAmount);
        }
        else if (usingItem.name == InteractionObjectName.CUPRAMEN)   //컵라면
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.ItemUse);
            state.SetHunger(state.currentHunger + cupramen_hungerRecoveryAmount);
        }
    }

    
    private void UseWeapon() 
    {
        ref Item usingWeapon = ref state.auxiliaryWeapon;
        GameObject weaponProjectileClone; //Inspector에서 투사체 프리팹 설정
        //마우스 방향으로 투사체를 던지기 위한 계산
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 projectileDirection = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        if (usingWeapon.count <= 0)
        {
            return;
        }
        //메스 구현
        if (usingWeapon.name == InteractionObjectName.SCALPEL)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.Scalpel);
            usingWeapon.count--;
            weaponProjectileClone = Instantiate(projectile[usingWeapon.name], transform.position, Quaternion.Euler(0, 0, angle));
            weaponProjectileClone.GetComponent<ProjectileCtrl>().Init(usingWeapon.name, projectileDirection);
        }
        //파이프 구현
        else if(usingWeapon.name == InteractionObjectName.PIPE)
        {
            if(canUsePipe == false)
            {
                return;
            }
            SoundManager.instance.PlaySfx(SoundManager.Sfx.BasicAttackWind);
            usingWeapon.count--;
            canUsePipe = false;
            pipeCount = (int)usingWeapon.count;
            usingWeapon.count = 0;
            weaponProjectileClone = Instantiate(projectile[usingWeapon.name], transform.position, Quaternion.Euler(0, 0, angle));
            weaponProjectileClone.GetComponent<ProjectileCtrl>().Init(usingWeapon.name, projectileDirection);
        }
        //혈액팩 구현
        else if(usingWeapon.name == InteractionObjectName.BLOODPACK)
        {
            if(canUseBloodpack == false)
            {
                return;
            }
            if(!weaponButtonDown)
            {
                weaponButtonDown = true;
                SoundManager.instance.PlaySfx(SoundManager.Sfx.BloodPack);
            }
            canUseBloodpack = false;
            StartCoroutine(BloodpackCoolTime(bloodPackDelay));
            usingWeapon.count -= bloodPackDelay;
            weaponProjectileClone = Instantiate(projectile[usingWeapon.name], transform.position, Quaternion.Euler(0, 0, angle));
            weaponProjectileClone.GetComponent<ProjectileCtrl>().Init(usingWeapon.name, projectileDirection);
        }
        //소화기 구현
        else if(usingWeapon.name == InteractionObjectName.FIREEXTINGUISHER)
        {
            if (canUseFireextinguisher == false)
            {
                return;
            }
            if (!weaponButtonDown)
            {
                weaponButtonDown = true;
                SoundManager.instance.PlaySfx(SoundManager.Sfx.FireExtinguisher);
            }
            canUseFireextinguisher = false;
            StartCoroutine(FireextinguisherCoolTime(fireextinguisherDelay));
            usingWeapon.count -= fireextinguisherDelay;
            weaponProjectileClone = Instantiate(projectile[usingWeapon.name], transform.position, Quaternion.Euler(0, 0, angle));
            weaponProjectileClone.GetComponent<ProjectileCtrl>().Init(usingWeapon.name, projectileDirection);
        }
    }
    
    private IEnumerator DumpItem(int itemIndex)     //아이템 버리는 시간을 카운팅하는 코루틴 함수이다.
    {
        yield return new WaitForSeconds(dumpTime);
        if (isDump[itemIndex] == true)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.ItemDump);
            state.item[itemIndex].count = 0;
        }
    }

    private IEnumerator BloodpackCoolTime(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        canUseBloodpack = true;
    }

    private IEnumerator FireextinguisherCoolTime(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        canUseFireextinguisher = true;
    }
    private IEnumerator originsprite(float delay)
    {
        yield return new WaitForSeconds(delay);


        spriteRenderer.material = origin_Material;
    }

}