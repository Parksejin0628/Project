using DefaultSetting;
using DefaultSetting.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TriggerName
{
    ActiveTest, TeleportTest, ChangeStateTest, ObstacleTest
}

public class TriggerBase : MonoBehaviour
{
    public TriggerName name;
    public bool isDestroy = true;
    [Header("오브젝트 활성화 관련 변수")]
    public GameObject activityObject;
    [Header("좌표 이동 관련 변수")]
    public GameObject teleportPoint;
    [Header("캐릭터 스탯 변경, 장애물 관련 변수")]
    public float stateChangeAmount = 10.0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Define.Layer.Player.ToInt())
        {
            Debug.Log("플레이어 충돌");
            PerformFunction(collision.gameObject);
            if (isDestroy)
            {
                gameObject.SetActive(false);
            }
        }
    }


    public void PerformFunction(GameObject player)
    {
        switch (name)
        {
            case TriggerName.ActiveTest:
                SetActiveObject();
                break;
            case TriggerName.TeleportTest:
                TeleportPlayer(player);
                break;
            case TriggerName.ChangeStateTest:
                ChangeState(player);
                break;
            case TriggerName.ObstacleTest:
                ApplyDamage();
                break;

        }

    }

    public void SetActiveObject()
    {
        activityObject.SetActive(true);
    }

    public void TeleportPlayer(GameObject player)
    {
        player.transform.position = teleportPoint.transform.position;
    }

    public void ChangeState(GameObject player)
    {
        player.GetComponent<PlayerStat>().Hp += stateChangeAmount;
    }

    public void ApplyDamage()
    {
        Debug.Log("플레이어 데미지 입힘");
        Managers.Game.Player.OnHitUnit(stateChangeAmount);
    }
}
