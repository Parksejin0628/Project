using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class story : MonoBehaviour
{
    public GameObject qa1;
    public Texture2D[] images;  // 이미지 배열
    private int currentIndex = 0;  // 현재 이미지 인덱스
    public RawImage panel;  // 이미지를 표시할 UI 요소
    int a = 0;

    void Start()
    {
        // 이미지 배열이 비어있는 경우 경고 출력
        if (images.Length == 0)
        {
            Debug.LogWarning("Image array is empty!");
        }
        else
        {
            // UI 요소 찾기
            panel = panel.GetComponent<RawImage>();

            // 초기 장면에 첫 번째 이미지를 표시
            DisplayImage(currentIndex);
        }
    }

    void Update()
    {
        // 스페이스바를 누르면 다음 장면으로 이동
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 현재 인덱스를 증가시키고 배열 범위를 초과하는 경우 0으로 되돌림
            currentIndex = (currentIndex + 1) % images.Length;
            DisplayImage(currentIndex);
            a++;
            Debug.Log(a);
        }
        if(a== 7)
        {
            SceneManager.LoadScene(2);
        }

        if(images.Length <4 && a == 3)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
            
        }

    }

    void DisplayImage(int index)
    {
        // 현재 인덱스에 해당하는 이미지를 UI 요소에 설정
        panel.texture = images[index];

        Debug.Log("Displaying image: " + index);
    }
}
