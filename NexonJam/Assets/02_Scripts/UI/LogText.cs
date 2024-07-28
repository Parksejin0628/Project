using DefaultSetting.Utility;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    public static float targetPosZ = 0;
    public RectTransform handler;
    public Image bgImage;
    public TextMeshProUGUI logText;

    void Awake()
    {
        //조건
        if (handler == null)
        {
            Debug.LogWarning("헨들러 연결 X");
            handler = transform.Find("Handler").GetComponent<RectTransform>();
        }

        if (bgImage == null)
        {
            Debug.LogWarning("헨들러 연결 X");
            bgImage = handler.GetComponentInChildren<Image>();
        }

        if (logText == null)
        {
            Debug.LogWarning("텍스트 연결 X");
            logText = handler.GetComponentInChildren<TextMeshProUGUI>();
        }

        //로직
        StartCoroutine(Co_LogText());
    }

    void Update()
    {
    }

    public void SetLogTxt(string logMessage)
    {
        logText.text = logMessage;
    }

    IEnumerator Co_LogText()
    {
        float currentTime = 0;

        StartCoroutine(Extension.Co_FadePlay(null, bgImage, Extension.Ease.Linear, prodTime: 0.2f, bgImage.color.a, 0, 1, true));
        StartCoroutine(Extension.Co_FadePlay(null, logText, Extension.Ease.Linear, prodTime: 0.2f, logText.color.a, 0, 1, true));
        while (currentTime < 3.5f)
        {
            currentTime += Time.unscaledDeltaTime;
            handler.AddY(35 * Time.unscaledDeltaTime);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        Destroy(gameObject);
    }
}
