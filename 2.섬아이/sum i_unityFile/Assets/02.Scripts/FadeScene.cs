using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScene : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.5f;

    void Start()
    {
        // 시작 시 화면을 투명하게 설정
        fadeImage.color = new Color(1f, 1f, 1f, 1f);

        // 코루틴을 사용하여 페이드 인 및 페이드 아웃 실행
        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {
        // 페이드 인
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration, fadeImage));

        // 게임 로직 또는 다음 화면으로의 전환 작업 수행

        // 페이드 아웃
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration, fadeImage));
    }

    IEnumerator Fade(float startAlpha, float targetAlpha, float duration, Image image)
    {
        float currentTime = 0f;
        Color startColor = image.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;
        }
    }
}
