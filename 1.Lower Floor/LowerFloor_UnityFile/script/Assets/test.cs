using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour /*모든 유니티 클래스의 부모 (MonoBehaviour의 상속을 받아 
    유니티 함수를 사용할 수 있는 것임*/
{



    // Start is called before the first frame update
    void Start()
    {
        string str = "안녕";
        int a = 12313213;

        string mes = str + a;
        Debug.Log(mes);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
