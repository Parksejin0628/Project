using System.Collections;
using TMPro;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    public float starttime;
    private string text;
    public TMP_Text targetText;
    private float delay = 0.125f;
    public Vector3 randomPositionOffset;
    public Vector3 randomRotationAngles;

    public RectTransform rectTransform;
    private bool isDelay = false;
    void Start()
    {
        StartCoroutine(delayed(starttime)); //사용시

  
        
        ApplyRandomTransform();
        text = targetText.text.ToString();
        targetText.text = " ";

        rectTransform = GetComponent<RectTransform>();

        StartCoroutine(RandomTextPrint());
    }

    void Update()
    {

    }

    IEnumerator delayed(float time)
    {
        isDelay = true;
        Debug.Log("왜안멈춤");
        yield return new WaitForSeconds(time);
        Debug.Log("코루틴 완료");
        isDelay = false;
    }
    IEnumerator RandomTextPrint()
    {
        while (true)
        {
            while (isDelay)
            {
                yield return null;
            }


            yield return StartCoroutine(textPrint(delay));
       
            // Generate a random delay for the next iteration
            float randomDelay = Random.Range(12f, 20f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    IEnumerator textPrint(float d)
    {
        ApplyRandomTransform();
        int count = 0;

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                targetText.text += text[count].ToString();
                count++;
            }

            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(1.5f);

        targetText.text = "";

        yield return new WaitForSeconds(1f);
    }

    void ApplyRandomTransform()
    {
    randomPositionOffset = new Vector3(Random.Range(-850f, 850f), Random.Range(-500f, 500f), 0f);
    randomRotationAngles = new Vector3(0f, 0f, Random.Range(-40f, 40f));

    rectTransform.anchoredPosition = randomPositionOffset;
    rectTransform.localRotation = Quaternion.Euler(randomRotationAngles);
}
}
