using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdown : MonoBehaviour
{
    [SerializeField] float setTime;
    [SerializeField] Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        countdownText.text = setTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (setTime > 0)
        {
            setTime -= Time.deltaTime;
        }
        else if (setTime <= 0)
        {
            Time.timeScale = 0.0f;
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        countdownText.text = Mathf.Round(setTime).ToString();
    }
}
