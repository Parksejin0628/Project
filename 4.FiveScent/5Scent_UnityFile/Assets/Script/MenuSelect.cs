using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
    public GameObject paneltext;
    public GameObject Stage1exr;

    private static int Stage1 = 0;
    // Start is called before the first frame update
    void Start()
    {
        paneltext.SetActive(false);
        Stage1exr.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Stage1 == 2)
        {
            Stage1exr.SetActive(true);
        }
        if (Stage1 == 3)
        {
            SceneManager.LoadScene(2);
        }
        if (Stage1 == 3)
        {
            Stage1exr.SetActive(true);

        }
        if (Stage1 == 4)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("게임종료");
    }

    public void CreditShow()
    {
        paneltext.SetActive(true); // Show the credit panel
    }

    public void CreditClose()
    {
        paneltext.SetActive(false); // Show the credit panel
    }

    public void Stage2()
    {

        Stage1++;
        Debug.Log(Stage1);
    }
}