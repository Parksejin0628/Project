using UnityEngine;
using System.Collections;

public class enemy_AI : MonoBehaviour
{
    public float speed = 1.0f; // 이동 속도
    public float moveDistance = 2.0f; // 이동 거리
    public float changeDirectionInterval = 2.0f; // 방향 변경 간격
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 currentDirection;
    private bool isMoving = true;

    void Start()
    {
        SetMove();
 
    }

    void Update()
    {
        SelfMove();
    }

    void SetMove()
    {
        startPosition = transform.position;
        currentDirection = RandomDirection();
        targetPosition = startPosition + currentDirection * moveDistance;
        StartCoroutine(ChangeDirection());
    }
    void SelfMove()
    {
        if (isMoving)
        {
            transform.position += currentDirection * speed * Time.deltaTime;

            // 현재 위치와 목표 위치 사이의 거리를 계산
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget < 0.1f)
            {
                // 목표 위치에 도달하면 방향을 반대로 변경하고 목표 위치를 시작 위치로 설정
                currentDirection = -currentDirection;

                if (currentDirection == Vector3.left)
                    targetPosition = new Vector3(startPosition.x - moveDistance, startPosition.y, startPosition.z);
                else if (currentDirection == Vector3.right)
                    targetPosition = new Vector3(startPosition.x + moveDistance, startPosition.y, startPosition.z);
                else if (currentDirection == Vector3.up)
                    targetPosition = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);
                else if (currentDirection == Vector3.down)
                    targetPosition = new Vector3(startPosition.x, startPosition.y - moveDistance, startPosition.z);
            }
        }
    }
    IEnumerator ChangeDirection()
    {
        while (isMoving)
        {
            currentDirection = RandomDirection();
            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }

    Vector3 RandomDirection()
    {
        float horizontal = Random.Range(-1f, 1f);
        float vertical = Random.Range(-1f, 1f);
        return new Vector3(horizontal, vertical, 0).normalized;
    }
}
