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
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

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

    public void DeadPlayer()
    {
        isPlayerDead = true;
        SceneManager.LoadScene(mainSceneName);
    }

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
