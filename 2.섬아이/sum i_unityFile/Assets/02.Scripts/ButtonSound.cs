using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Inst.PlayBGM("DoneSong__Island_Kid");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSFX()
    {
        AudioManager.Inst.PlaySFX("button_push");
    }
}
