using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting
{
    public class OneOff_AddTransform : MonoBehaviour
    {
        /// <summary>
        /// 사용법
        /// 1. 원하는 로직 함수 Logic에 작성
        /// 2. 저장, 컴파일 후 아무 오브젝트에 추가
        /// 3. 곧바로 로직 실행 후 스스로 삭제
        /// </summary>

    #if UNITY_EDITOR
        public bool isLogicEnd = false;

        private void Reset()
        {
            //로직
            Logic();

            //제거
            StartCoroutine(CoDestroy());
        }

        private void Awake()
        {
            Debug.LogError("의미없는 컴포넌트 존재");

        }

        public void Logic()
        {
            AddTransform();

            isLogicEnd = true;
        }

        public void AddTransform()
        {
            ////움직일 포지션 값 작성
            Vector3 addVec = new Vector3(-4.08f, -24.3f, 0);

            transform.position += addVec;
        }

        IEnumerator CoDestroy()
        {
            yield return new WaitUntil(() => isLogicEnd);
            DestroyImmediate(this);
        }
    #endif
    }
}
