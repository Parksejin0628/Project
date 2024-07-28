using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public string mainSceneName = "Main Scene";
    public GameObject whiteScene;
    public GameObject blackScene;
    bool isPlayerDead = false;
    // Start is called before the first frame update
    void Start()
    {
        //싱글톤 구현
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }
    //게임의 메인 화면을 시작할 때 페이드인, 페이드아웃을 실행
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isPlayerDead)
        {
            Debug.Log("check black");
            blackScene = GameObject.Find("BlackScene");
            StartCoroutine(Fadeout(blackScene));
            isPlayerDead = false;
        }
        else
        {
            Debug.Log("check white");
            whiteScene = GameObject.Find("WhiteScene");
            StartCoroutine(Fadeout(whiteScene));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //플레이어 사망 시 다시 시작
    public void DeadPlayer()
    {
        isPlayerDead = true;
        SceneManager.LoadScene(mainSceneName);
    }
    //페이드 아웃
    public IEnumerator Fadeout(GameObject Scene)
    {
        Image SceneImage = Scene.GetComponent<Image>();
        SceneImage.color = new Color(SceneImage.color.r, SceneImage.color.g, SceneImage.color.b, 1);
        for (int i = 0; i < 25; i++)
        {
            SceneImage.color = new Color(SceneImage.color.r, SceneImage.color.g, SceneImage.color.b, SceneImage.color.a - (1/25f));
            yield return new WaitForSeconds(0.1f);
        }
    }
    //페이드 인
    public IEnumerator Fadein(GameObject Scene)
    {
        Image SceneImage = Scene.GetComponent<Image>();
        SceneImage.color = new Color(SceneImage.color.r, SceneImage.color.g, SceneImage.color.b, 0);
        for (int i = 0; i < 25; i++)
        {
            SceneImage.color = new Color(SceneImage.color.r, SceneImage.color.g, SceneImage.color.b, SceneImage.color.a + (1 / 25f));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
