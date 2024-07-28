using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultSetting.Utility
{
    //카골님 피드백으로 넣을 것
    //[완] print 전용 StringBuilder 만들기
    //2. [System.Diagnostics.Conditional("Test")]


    /// <summary>
    /// 일반 클래스의 경우 ToString을 오버라이드하면 원하는 형태로 출력할 수 있습니다.
    /// </summary>
    public static class DebugUtility
    {
        public static readonly string Null_TEXT = "Data is Null";
        public static StringBuilder staticSB = new StringBuilder(); //공용 StringBuilder

        private static StringBuilder printStaticSB = new StringBuilder(); //print 전용 StringBuilder

        //[System.Diagnostics.Conditional("A")]
        public static void Print<T>(this T data, string Label = null)
        {
            printStaticSB.Clear();
            AppendSB(data, Label, printStaticSB);
            Debug.Log(printStaticSB);
        }

        public static void AppendSB<T>(this T data, string Label = null, StringBuilder sb = null)
        {
            if (sb == null)
                sb = staticSB;

            if (Label != null)
                sb.AppendLine($"[{Label}]");

            if (data == null)
            {
                MakeStringAndAppendEntity(data, sb);
                return;
            }

            //Unity Class
            if (data is Array array)
            {
                AppendArray(array, sb);
            }
            else if (data is List<T> list)
            {
                AppendList(list, sb);
            }
            else if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var type = data.GetType();
                var listType = typeof(List<>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugUtility).GetMethod(nameof(AppendList), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
            }
            else if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var type = data.GetType();
                var dictType = typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugUtility).GetMethod(nameof(AppendDictionary), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
            }
            else if (data is IEnumerable enumerable && !(data is string))
            {
                Type dataType = data.GetType();
                Type elementType = dataType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))?.GetGenericArguments()[0];

                if (elementType != null)
                {
                    var method = typeof(DebugUtility).GetMethod(nameof(AppendIEnumerable), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                    method.Invoke(null, new object[] { enumerable, sb });
                }
                else
                {
                    sb.AppendLine("Unable to determine the element type of the IEnumerable.");
                }
            }
            else
            {
                MakeStringAndAppendEntity(data, sb);
            }
            sb.AppendLine();
        }

        //DebugExtension.ClearSB();
        public static void ClearSB()
        {
            staticSB.Clear();
        }

        // DebugExtension.PrintSB();
        public static void PrintSB()
        {
            Debug.Log($"{staticSB}");
        }

        #region Append
        private static void AppendArray(System.Array array, StringBuilder sb)
        {
            switch (array.Rank)
            {
                case 1:
                    foreach (var item in array)
                    {
                        MakeStringAndAppendEntity(item, sb);
                    }
                    break;
                case 2:
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            MakeStringAndAppendEntity(array.GetValue(i, j), sb);
                        }
                        sb.AppendLine();
                    }
                    break;
                default:
                    sb.Append("3차원 배열 이상: ");
                    foreach (var item in array)
                    {
                        MakeStringAndAppendEntity(item, sb);
                    }
                    break;
            }
            sb.AppendLine();
        }

        //private static void AppendList<T>(T list, StringBuilder sb) where T : List;
        private static void AppendList<T>(List<T> list, StringBuilder sb)
        {
            foreach (var item in list)
            {
                MakeStringAndAppendEntity(item, sb);
            }
            sb.AppendLine();
        }

        private static void AppendDictionary<T1, T2>(Dictionary<T1, T2> dict, StringBuilder sb)
        {
            foreach (KeyValuePair<T1, T2> keyValuePair in dict)
            {
                MakeStringAndAppendEntity($"key: {keyValuePair.Key}, value: {keyValuePair.Value}", sb);
            }
            sb.AppendLine();
        }

        private static void AppendIEnumerable<T>(IEnumerable<T> iEnum, StringBuilder sb)
        {
            foreach (var item in iEnum)
            {
                MakeStringAndAppendEntity(item, sb);
            }
            sb.AppendLine();
        }

        //TODO: 위 함수에도 데이터 담는걸 이걸로 통합해야 함.
        private static void MakeStringAndAppendEntity<T>(T data, StringBuilder sb)
        {
            if (data == null)
            {
                AppendString($"{Null_TEXT}\n", sb);
                return;
            }

            //담을 요소가 리스트인 경우
            if (data.GetType().IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var type = data.GetType();
                var listType = typeof(List<>).MakeGenericType(type.GetGenericArguments());
                var method = typeof(DebugUtility).GetMethod(nameof(AppendList), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(type.GetGenericArguments());
                genericMethod.Invoke(null, new object[] { data, sb });
                return;
            }
            else if (data is IEnumerable enumerable && !(data is string))
            {
                Type dataType = data.GetType();
                Type elementType = dataType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))?.GetGenericArguments()[0];

                if (elementType != null)
                {
                    var method = typeof(DebugUtility).GetMethod(nameof(AppendIEnumerable), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                    method.Invoke(null, new object[] { enumerable, sb });
                }
                else
                {
                    sb.AppendLine("Unable to determine the element type of the IEnumerable.");
                }
            }

            //Unity Class
            else if (data is GameObject obj)
            {
                AppendString($"{obj.name}, {obj.transform.position} ", sb);
                return;
            }
            else if (data is Transform tr)
            {
                AppendString($"{tr.name} ", sb);
                return;
            }
            //Unity Struct
            else if (data is RaycastResult raycastResult)
            {
                AppendString($"RaycastResult: {raycastResult.gameObject.name} ", sb);
                return;
            }
            else if (data is RaycastHit raycastHit)
            {
                AppendString($"RaycastHit: {raycastHit.transform.name} ", sb);
                return;
            }
            //Override ToString
            else if (data is Vector2 vec)
            {
                AppendString($"{{{vec.x}, {vec.y}}} ", sb);
                return;
            }
            //Default ToString
            else
            {
                AppendString($"{data} ", sb);
                return;
            }
        }

        private static void AppendString(string str, StringBuilder sb)
        {
            //길면 띄어쓰기
            if (str.Length > 15)
                sb.AppendLine(str);
            //그렇지 않으면 그대로 배치
            else
                sb.Append(str);
        }
        #endregion
    }
}