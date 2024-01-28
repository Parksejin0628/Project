using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public enum ProjectileName
{
    NULL, SCALPEL, PIPE, BLOODPACK, FIREEXTINGUISHER
}

public class ProjectileCtrl : MonoBehaviour
{
    const float SCALPEL_SPEED = 10;
    const float SCALPEL_RANGE = 10;
    const bool SCALPEL_PENETRATION = false;
    const float SCALPEL_DAMAGE = 12;
    const float SCALPEL_BLEEDING_DAMAGE_PER_SECOND = 2;
    const float SCALPEL_BLEEDING_TIME = 9999f;
    const float PIPE_SPEED = 10;
    const float PIPE_RANGE = 10;
    const bool PIPE_PENETRATION = true;
    const float PIPE_DAMAGE = 50;
    const float BLOODPACK_SPEED = 10;
    const float BLOODPACK_RANGE = 0;
    const bool BLOODPACK_PENETRATION = false;
    const float BLOODPACK_DAMAGE = 0;
    const float BLOODPACK_BLOOD_DURATION = 6f;
    const float FIREEXTINGUISHER_SPEED = 10;
    const float FIREEXTINGUISHER_RANGE = 2;
    const bool FIREEXTINGUISHER_PENETRATION = true;
    const float FIREEXTINGUISHER_DAMAGE = 3;
    const float FIREEXTINGUISHER_DUST_DURATION = 5f;
    //투사체 상태정보
    public ProjectileName projectileName;
    public Vector2 throwingDirection;
    public float throwingSpeed;
    public float throwingRange;
    public float projectileDamage;
    public bool isPenetration;
    public float damage;
    //투사체 파괴 후 남는 오브젝트 정보
    public GameObject recyclingPipe;
    public GameObject blood;
    public GameObject dust;
    //연산에 필요한 정보
    Transform transform;
    Vector2 throwingStartPos;
    Vector2 nowPos;

    void Start()
    {
        transform = GetComponent<Transform>();
        throwingStartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRange();
        GetComponent<Rigidbody2D>().velocity = throwingDirection * throwingSpeed;
        //날라가는거
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monster"))
        {
            Monster_info monster = other.GetComponent<Monster_info>();
            Debug.Log("투사체" + projectileName + "적중, 대상 : " + other);
            switch(projectileName)
            {
                case ProjectileName.SCALPEL:
                    SoundManager.instance.PlaySfx(SoundManager.Sfx.BasicAttack);
                    monster.Monster_HP -= SCALPEL_DAMAGE;
                    monster.StartCoroutine(monster.Bleeded(SCALPEL_BLEEDING_DAMAGE_PER_SECOND, SCALPEL_BLEEDING_TIME));
                    break;
                case ProjectileName.PIPE:
                    SoundManager.instance.PlaySfx(SoundManager.Sfx.BrokenPipe);
                    monster.Monster_HP -= PIPE_DAMAGE;
                    break;
                case ProjectileName.BLOODPACK:
  
                    break;
                case ProjectileName.FIREEXTINGUISHER:

                    monster.Monster_HP -= FIREEXTINGUISHER_DAMAGE;
                    break;
            }
            if(!isPenetration)
            {
                DestroyProjectile();
            }
        }
        else if(other.CompareTag("Map"))
        {
            DestroyProjectile();
        }
        
    }

    public void Init(InteractionObjectName name, Vector2 dir)
    {
        throwingDirection = dir;
        switch (name)
        {
            case InteractionObjectName.SCALPEL:
                projectileName = ProjectileName.SCALPEL;
                throwingSpeed = SCALPEL_SPEED;
                throwingRange = SCALPEL_RANGE;
                projectileDamage = SCALPEL_DAMAGE;
                isPenetration = SCALPEL_PENETRATION;
                break;
            case InteractionObjectName.PIPE:
                projectileName = ProjectileName.PIPE;
                throwingSpeed = PIPE_SPEED;
                throwingRange = PIPE_RANGE;
                projectileDamage = PIPE_DAMAGE;
                isPenetration = PIPE_PENETRATION;
                break;
            case InteractionObjectName.BLOODPACK:
                projectileName = ProjectileName.BLOODPACK;
                throwingSpeed = BLOODPACK_SPEED;
                throwingRange = BLOODPACK_RANGE;
                projectileDamage = BLOODPACK_DAMAGE;
                isPenetration = BLOODPACK_PENETRATION;
                break;
            case InteractionObjectName.FIREEXTINGUISHER:
                projectileName = ProjectileName.FIREEXTINGUISHER;
                throwingSpeed = FIREEXTINGUISHER_SPEED;
                throwingRange = FIREEXTINGUISHER_RANGE;
                projectileDamage = FIREEXTINGUISHER_DAMAGE;
                isPenetration = FIREEXTINGUISHER_PENETRATION;
                break;
        }
    }
    private void DestroyProjectile()
    {
        switch(projectileName)
        {
            case ProjectileName.SCALPEL:
                break;
            case ProjectileName.PIPE:
                Instantiate(recyclingPipe, transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case ProjectileName.BLOODPACK:
                GameObject bloodObject = Instantiate(blood, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(bloodObject, BLOODPACK_BLOOD_DURATION);
                break;
            case ProjectileName.FIREEXTINGUISHER:
                GameObject dustObject = Instantiate(dust, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(dustObject, FIREEXTINGUISHER_DUST_DURATION);
                break;
        }
        Destroy(gameObject);
    }
    private void CheckRange()
    {
        nowPos = transform.position;
        if((nowPos - throwingStartPos).magnitude > throwingRange)
        {
            DestroyProjectile();
        }
    }

}
