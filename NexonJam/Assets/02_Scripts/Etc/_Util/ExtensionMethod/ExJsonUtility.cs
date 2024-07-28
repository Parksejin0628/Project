using System.IO;
using UnityEngine;

/// <summary>
/// 사용법
/// 1. Serializable Class 추가
/// 2. 원하는 파일 이름 매개 변수 작성
/// 3. 로드 및 제거
/// 
/// 주의
/// - 다른 파일을 기본 파일명으로 할 시 덮어씌워지는것 조심해야 함.
/// </summary>
namespace DefaultSetting.Utility
{
    public static class ExJsonUtility
    {
        public static string lastPath;
        public static string JsonFolderPath
        {
            get
            {
                return Application.persistentDataPath + "\\Data";
            }
        }

        [ContextMenu("To Json Data(Save)")]
        public static void SaveDataToJson<T>(T data, string fileName = "Data")
        {
            string jsonData = JsonUtility.ToJson(data);
            string path = Path.Combine(JsonFolderPath, $"{fileName}.json");

            // 파일 저장
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, jsonData);
            lastPath = path;
        }

        [ContextMenu("From Json Data(Load)")]
        public static T LoadDataFromJson<T>(string fileName = "Data")
        {
            T result = default;
            string path = Path.Combine(JsonFolderPath, $"{fileName}.json");

            // 파일 존재 여부 체크
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                result = JsonUtility.FromJson<T>(jsonData);
            }
            else
            {
                Debug.LogWarning("파일을 찾을 수 없습니다: " + path);
            }
            lastPath = path;
            return result;
        }

    }
}