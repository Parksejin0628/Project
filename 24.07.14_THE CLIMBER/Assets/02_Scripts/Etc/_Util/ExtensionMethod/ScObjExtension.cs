using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting.Utility
{
    public static class ScObjExtension
    {
        // 사용법
        // SingleUse 시
        // this.AutoLoadAsset();

        // MultiUse 시 
        // string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        // string assetName = Path.GetFileNameWithoutExtension(assetPath);
        // this.AutoLoadAsset(assetName);
#if UNITY_EDITOR
        public static void AutoLoadAsset(this UnityEngine.ScriptableObject scriptable, string assetName = null)
        {
            //스크립터블의 필드를 모두 찾고
            FieldInfo[] fields = scriptable.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //기본 타입은 제거
            fields = fields.Where(e => e.FieldType.IsPrimitive == false && e.FieldType != typeof(string)).ToArray();

            foreach (FieldInfo field in fields)
            {
                string fieldname = field.Name;

                //프로퍼티 처리
                fieldname = System.Text.RegularExpressions.Regex.Replace(fieldname, @"[<>]|k__BackingField", "");

                //언더바 제거
                if (fieldname.StartsWith("_"))
                    fieldname = fieldname.Substring(1);

                //첫 문자 대문자
                fieldname = char.ToUpper(fieldname[0]) + fieldname.Substring(1);

                //에셋 이름 구분하여 찾는 경우 추가
                string targetFile = assetName + fieldname;

                List<string> findAssetPath = AssetDatabase.FindAssets(targetFile, new string[] { "Assets" })
                    .Where(e => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(e)) == targetFile)
                    .Select(asset => AssetDatabase.GUIDToAssetPath(asset))
                    .ToList();

                switch (findAssetPath.Count)
                {
                    case 0:
                        Debug.LogError($"타겟 파일명 [{targetFile}]와 일치하는 파일을 찾지 못했습니다.\n");

                        field.SetValue(scriptable, null);
                        continue;
                    case 1: //파일 발견
                        break;
                    default:
                        Debug.LogError($"타겟 파일명 [{targetFile}]와 동일한 이름의 파일이 {findAssetPath.Count}개 존재합니다.\n");

                        StringBuilder sb = new();
                        sb.AppendLine($"타겟 파일명 [{targetFile}]와 동일한 이름의 파일이 {findAssetPath.Count}개 존재합니다.");
                        findAssetPath.ForEach(path => sb.AppendLine(path));
                        Debug.LogError(sb);

                        field.SetValue(scriptable, null);
                        continue;
                }

                object obj = AssetDatabase.LoadAssetAtPath(findAssetPath[0], field.FieldType);
                field.SetValue(scriptable, obj);
            }
        }
#endif
    }
}
