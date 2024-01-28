using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject credit;

    void Update()
    {
        if (Input.anyKeyDown&&credit.activeSelf == true)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.MiniButton);
            credit.SetActive(false);
        }
    }
    public void OnClickNewGame()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.MenuSelect);
        SceneManager.LoadScene("seomap");
    }

    public void OnClickCredit()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.MenuSelect);
        credit.SetActive(true);
    }

    public void OnClickExit()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.MenuSelect);
        Application.Quit();
        Debug.Log("Click");
    }
}
