using DefaultSetting;
using DefaultSetting.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Define.Scene nextScene;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Define.Layer.Player.ToInt())
        {
            Debug.Log("포탈");
            Managers.Scene.LoadScene(nextScene);

        }
    }
}
