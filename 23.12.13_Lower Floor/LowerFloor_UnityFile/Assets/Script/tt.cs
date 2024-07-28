using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class tt : MonoBehaviour
{


    private Monster_info monster;
    private void Awake()
    {
        monster = FindObjectOfType<Monster_info>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            monster.Monster_HP -= 40f;
            Debug.Log("Å¸¾× ´êÀ½");
        }
    }
}


