using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_boob : MonoBehaviour
{

    private State state;
    private void Awake()
    {
        state = FindObjectOfType<State>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            state.SetHP(state.currentHP - 15f);
            Debug.Log("Å¸¾× ´êÀ½");
        }
    }
}
