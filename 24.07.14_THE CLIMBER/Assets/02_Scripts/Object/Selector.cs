using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public GameObject[] ItemCandidate;
    public float itemDistance = 1;

    // Start is called before the first frame update
    public void ChooseItem()
    {
        gameObject.SetActive(false);
    }


    void Start()
    {
        GameObject[] randomItems = GetRandomElements(ItemCandidate, 3);
        int index = -1;
        // 자식 오브젝트 생성
        foreach (GameObject item in randomItems)
        {
            GameObject child = Instantiate(item, transform);
            child.transform.localPosition = Vector3.left * itemDistance * index; // 부모의 위치를 기준으로 위치 설정
            index++;
            // 특정 스크립트 추가
            child.AddComponent<SelectorChildMonitor>();
        }

    }

    GameObject[] GetRandomElements(GameObject[] array, int count)
    {
        if (array.Length < count)
        {
            Debug.LogError("Array length is less than the count of elements to select.");
            return null;
        }

        // 배열 복사 및 Fisher-Yates Shuffle 알고리즘 적용
        List<GameObject> arrayList = new List<GameObject>(array);
        for (int i = arrayList.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            GameObject temp = arrayList[i];
            arrayList[i] = arrayList[randomIndex];
            arrayList[randomIndex] = temp;
        }

        // 섞인 배열의 앞부분에서 원하는 개수만큼 선택하여 반환
        return arrayList.GetRange(0, count).ToArray();
    }
}
