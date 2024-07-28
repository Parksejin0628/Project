using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public bool CanMove = false;

    public static CutSceneController instance;
    public GameObject[] cutScene;
    public GameObject deadScene;
    int idx = 0;

    private void Awake()
    {
        if(CutSceneController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        CutSceneController.instance = this;
        DontDestroyOnLoad(instance);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += LoadScene;
        for(int i=0; i<cutScene.Length; i++)
        {
            cutScene[i].SetActive(true);
        }
    }

    void LoadScene(Scene scene, LoadSceneMode mode)
    {
        deadScene = GameObject.Find("DeadScene");
        SoundManager.instance.PlayBgm(SoundManager.Bgm.VIP, true);
        StartCoroutine(StartDeadScene());
        if (idx >= cutScene.Length)
        {
            CanMove = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (idx < cutScene.Length)
        {
            PressToSkip();
        }
        else if(!CanMove)
        {
            CanMove = true;
            SoundManager.instance.PlayBgm(SoundManager.Bgm.VIP, true);
        }
    }

    void PressToSkip()
    {
        if(Input.anyKeyDown) {
            cutScene[idx++].SetActive(false);
            SoundManager.instance.PlaySfx(SoundManager.Sfx.MiniButton);
        }
    }

    IEnumerator StartDeadScene()
    {
        Image deadSceneImage = deadScene.GetComponent<Image>();
        deadSceneImage.color = new Color(deadSceneImage.color.r, deadSceneImage.color.g, deadSceneImage.color.b, 1);
        yield return new WaitForSeconds(0.5f);
        for(int i=0; i<10; i++)
        {
            deadSceneImage.color = new Color(deadSceneImage.color.r, deadSceneImage.color.g, deadSceneImage.color.b, deadSceneImage.color.a - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
