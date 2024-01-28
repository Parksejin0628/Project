using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PuzzleUI;
    public GameObject NextUI;
    public GameObject NextUI_1;
    public GameObject NextUI_2;
    public GameObject FinalPuzzleUI;
    public int needNum;
    public bool final;
    private int nowNum = 0;
    public int UINum = 0;
    private static int nowStage = 3;
    // Start is called before the first frame update
    void Start()
    {
        PuzzleUI.SetActive(false);
        NextUI.SetActive(false);
        NextUI_1.SetActive(false);
        NextUI_2.SetActive(false); 
        if(final)
        {
            FinalPuzzleUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(nowNum + " " + needNum + " " + UINum);
        if (nowNum == needNum && final)
        {
            PuzzleUI.SetActive(false);
            FinalPuzzleUI.SetActive(true);
        }
        else if(nowNum >= needNum && UINum == 0)
        {
            NextUI.SetActive(true);
            NextUI_1.SetActive(true);
        }
        else if (UINum == 1)
        {
            NextUI_2.SetActive(true);
        }
        else if(UINum == 2)
        {
            SceneManager.LoadScene(++nowStage);
        }
    }

    public void onPuzzleUI()
    {
        PuzzleUI.SetActive(true);
    }

    public void puzzleCount()
    {
        nowNum++;
        //Debug.Log("nowNum : " + nowNum);
        
    }

    public void nextUI()
    {
        //Debug.Log("nowNum : " + nowNum);
        UINum++;
    }
}
