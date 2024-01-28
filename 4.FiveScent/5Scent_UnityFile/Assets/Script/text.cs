using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class storya : MonoBehaviour
{
    public Text ChatText;
    public Text CharacterName;

    public string writerText = "";

    bool isButtonClicked = false;

    void Start()
    {
        StartCoroutine(TextPractice());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 눌렀을 때 버튼 클릭으로 처리
        {
            isButtonClicked = true;
        }
    }

    IEnumerator NormalChat(string narrator, string narration)
    {
        CharacterName.text = narrator;
        writerText = "";

        for (int a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            ChatText.text = writerText;
            yield return null;
        }

        while (true)
        {
            if (isButtonClicked)
            {
                isButtonClicked = false;
                break;
            }
            yield return null;
        }
    }

    IEnumerator TextPractice()
    {
        yield return StartCoroutine(NormalChat("캐릭터1", "이것은 타이핑 효과를 통해 대사창을 구현하는 연습"));
        yield return StartCoroutine(NormalChat("캐릭터2", "안녕하세요, 반갑습니다."));
    }
}
