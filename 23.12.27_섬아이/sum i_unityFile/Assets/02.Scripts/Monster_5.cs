using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_5 : MonoBehaviour
{
    Animator m5_anim;

    private void Start()
    {
        m5_anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m5_anim.SetTrigger("isAttack");
        }
    }
}
