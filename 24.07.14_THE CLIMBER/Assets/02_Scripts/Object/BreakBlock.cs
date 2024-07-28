using DefaultSetting;
using DefaultSetting.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어 태그를 가진 오브젝트가 충돌했을 때
        if (collision.gameObject.layer == Define.Layer.Player.ToInt())
        {
            Debug.Log("Player collided with the object.");
            // 오브젝트가 활성화 상태를 유지합니다.
            gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어 태그를 가진 오브젝트가 충돌 영역을 벗어났을 때
        if (collision.gameObject.layer == Define.Layer.Player.ToInt())
        {
            Debug.Log("Player exited the collision area.");
            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
