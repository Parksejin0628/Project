using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stroya : MonoBehaviour
{
    public Text textUI;
    public string[] texts; // 텍스트 배열

    private int currentIndex; // 현재 인덱스

    void Start()
    {
        currentIndex = 0; // 초기 인덱스 설정
        UpdateText(); // 텍스트 업데이트
    }

    void Update()
    {
        // 스페이스바를 누르면 다음 텍스트로 업데이트
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentIndex++;
            UpdateText();
        }
    }

    // 텍스트 업데이트 함수
    void UpdateText()
    {
        if (currentIndex < texts.Length)
        {
            textUI.text = texts[currentIndex];
        }
        else
        {
            textUI.text = "End of Texts";
        }
    }
}

