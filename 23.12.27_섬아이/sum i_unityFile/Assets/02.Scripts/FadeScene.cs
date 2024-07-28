using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScene : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.5f;

    void Start()
    {
        // ���� �� ȭ���� �����ϰ� ����
        fadeImage.color = new Color(1f, 1f, 1f, 1f);

        // �ڷ�ƾ�� ����Ͽ� ���̵� �� �� ���̵� �ƿ� ����
        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {
        // ���̵� ��
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration, fadeImage));

        // ���� ���� �Ǵ� ���� ȭ�������� ��ȯ �۾� ����

        // ���̵� �ƿ�
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
