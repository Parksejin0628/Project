using UnityEngine;

public class Map : MonoBehaviour
{
    private Renderer renderer;
    private Color originalColor;
    private bool playerInside;

    private void Start()
    {
        // 게임 오브젝트의 Renderer 컴포넌트를 가져오기
        renderer = GetComponent<Renderer>();

        // 원래 색상 저장
        originalColor = renderer.material.color;

        // 초기에는 원래 색상으로 설정
        renderer.material.color = originalColor;

        // 플레이어가 맵 안에 없는 초기 상태
        playerInside = false;
    }

    private void Update()
    {
        // 플레이어가 맵 안에 있을 때만 반투명 처리
        if (playerInside)
        {
            Color newColor = originalColor;
            newColor.a = 0.5f;
            renderer.material.color = newColor;
        }
        else
        {
            // 플레이어가 맵 안에 없을 때는 원래 색상으로 복원
            renderer.material.color = originalColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 맵에 들어왔을 때
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 맵을 나갔을 때
            playerInside = false;
        }
    }
}
