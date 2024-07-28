using DefaultSetting.Utility;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting
{
    public class OneOff_ScObjExReset : MonoBehaviour
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
            ScObjExReset();

            isLogicEnd = true;
        }

        string TARGET_FOLDER_PATH = "/Resources/Data";
        string TargetFolder { get { return Application.dataPath + TARGET_FOLDER_PATH; } }
        public void ScObjExReset()
        {
            //로드할 PATH 작성
            string[] findScriptsPath = null;
            findScriptsPath = Extension.Load_FolderPath2Files(TargetFolder, "*.asset");

            foreach (var scObjExPath in findScriptsPath)
            {
                string testString = scObjExPath.Replace(Application.dataPath.Replace("/Assets/", ""), "Assets/");
                Debug.Log(testString);

                ScriptableObjectEx tempSc = AssetDatabase.LoadAssetAtPath<ScriptableObjectEx>(testString);
                tempSc.AutoFind();
            }
        }

        IEnumerator CoDestroy()
        {
            yield return new WaitUntil(() => isLogicEnd);
            Debug.Log("제거 완료");
            DestroyImmediate(this);
        }
#endif
    }
}
