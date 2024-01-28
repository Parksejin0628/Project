using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class sound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip clip;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(clip);
    }
}
