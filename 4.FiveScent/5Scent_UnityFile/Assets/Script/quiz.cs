using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class WirePuzzleGame : MonoBehaviour
{
    private GameObject start;

    private GameObject end;

    private List<Vector2Int> wireConnections = new List<Vector2Int>();

    // 전선을 그리는 함수
    private void DrawWire(Vector2Int position)
    {
        wireConnections.Add(position);
    }

    // 전선이 닫혀 있는지 확인하는 함수
    private bool IsWireLoopClosed()
    {
        if (wireConnections.Count < 2)
        {
            return false;
        }

        // 시작 지점과 끝 지점 비교
        return wireConnections[0] == wireConnections[wireConnections.Count - 1];
    }

    // 전선 연결 상태를 확인하여 승리 조건을 검사하는 함수
    private bool CheckWinCondition()
    {
        // 게임 보드 상태에 따라 전선 연결 여부를 확인하는 코드를 작성해야 합니다.
        // 예를 들어, 모든 그리드 셀이 전선으로 연결되어 있는지 검사합니다.
        // 연결 상태를 기록하는 배열 등을 사용하여 구현합니다.

        return false; // 승리 조건을 만족하지 않는 경우
    }

    // 전선 그리기 및 게임 상태 검사 예시
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));

            DrawWire(gridPosition);

            if (IsWireLoopClosed())
            {
                if (CheckWinCondition())
                {
                    // 게임 승리 처리
                    Debug.Log("You Win!");
                }
                else
                {
                    // 게임을 계속 진행합니다.
                }
            }
        }
    }
}