using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class numberkey : MonoBehaviour
{

    public GameObject gameManager;
    public Text outputText;
    private string inputNumbers = "";
    public GameObject Stage5UI;

    private void Awake()
    {
        Stage5UI.SetActive(false);

    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnNumberButtonPressed(int number)
    {
        inputNumbers += number.ToString();
        outputText.text = inputNumbers;

        if (inputNumbers.Length >= 4 && inputNumbers.Substring(inputNumbers.Length - 4) == "6458")
        {
            gameManager.GetComponent<GameManager>().puzzleCount();

        }
    }
}
