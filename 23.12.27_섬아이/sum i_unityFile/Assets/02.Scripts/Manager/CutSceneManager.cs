using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class CutSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentCutIndex = 0;
    public string nextSceneName;
    public GameObject[] cutScenes;
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.anyKeyDown && currentCutIndex < cutScenes.Length)
        {
            AudioManager.Inst.PlaySFX("button_push");
            cutScenes[currentCutIndex].SetActive(false);
            currentCutIndex++;
        }
        if (currentCutIndex == cutScenes.Length)
        {
            StartCoroutine(GameManager.instance.Fadein(GameObject.Find("WhiteScene")));
            
            Invoke("NextScene", 2.5f);

            AudioManager.Inst.PlaySFX("transition");
            AudioManager.Inst.StopBGM();
            currentCutIndex++;
        }

    }

    void NextScene()
    {
        
        SceneManager.LoadScene(nextSceneName);
    }
}
