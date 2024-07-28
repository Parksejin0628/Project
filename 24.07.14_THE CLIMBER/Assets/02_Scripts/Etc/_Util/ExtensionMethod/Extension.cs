using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//HCLibarary
namespace DefaultSetting.Utility
{
    public static class Extension
    {
        #region ScriptUtil
#if UNITY_EDITOR
        /// <summary>
        /// 찾을 파일의 이름을 받아 파일의 경로를 반환하는 함수
        /// </summary>
        /// <param name="targetFile"> 예시 :TestB </param>
        public static string Load_FileName2Path(string targetFile)
        {
            var findAssets = AssetDatabase.FindAssets(targetFile, new string[] { "Assets" }).Where(e => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(e)) == targetFile).ToArray();
            switch (findAssets.Length)
            {
                case 0:
                    Debug.LogError($"타겟 파일명 [{targetFile}]와 일치하는 파일을 찾지 못했습니다.\n");
                    findAssets = null;
                    break;
                case 1:
                    break;
                default:
                    Debug.LogError($"타겟 파일명 [{targetFile}]와 동일한 이름의 파일이 {findAssets.Length}개 존재합니다.\n");
                    findAssets = null;
                    break;
            }
            if (findAssets == null)
                return null;

            var path = AssetDatabase.GUIDToAssetPath(findAssets[0]);
            return path;
        }
#endif

        /// <summary>
        /// 폴더 경로와 확장자를 받아 폴더 내의 자료를 찾아 넘겨주는 함수
        /// </summary>
        /// <param name="targetFolder"> 예시 : Application.dataPath + TARGET_FOLDER_PATH("/_Test"); </param>
        /// <param name="extension"> 예시 : "*.cs" </param>
        /// <returns></returns>
        public static string[] Load_FolderPath2Files(string targetFolder, string extension)
        {
            try
            {
                string[] findScriptsPath = null;
                findScriptsPath = Directory.GetFiles(targetFolder, extension, SearchOption.AllDirectories);
                Debug.Log($"{findScriptsPath.Length}개의 파일 발견\n");
                return findScriptsPath;
            }
            catch (Exception)
            {
                Debug.LogError($"다이렉트 호출 실패\nTargetFolder:{targetFolder}");
                return null;
            }
        }
        /// <summary>
        /// Path에서 targetString의 뒷부분만 추출해서 리턴하는 함수
        /// targetString이 포함되어 리턴된다.
        /// </summary>
        /// <returns></returns>
        public static string SubPath(this string path, string targetString)
        {
            return path.Substring(path.IndexOf(targetString));
        }

        #endregion

        #region GameObject
        //GetOrAdd
        public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }

        public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_Base.BindEvent(go, action, type);
        }
        #endregion

        #region Transform
        public static void Reset(this Transform tr)
        {
            tr.position = Vector3.zero;
            tr.rotation = Quaternion.identity;
            tr.localScale = Vector3.zero;
        }

        // 두 번째 인자가 true면 글로벌 좌표로 변경, false면 로컬 좌표로 변경됩니다.
        public static void SetX(this Transform tr, float value, bool GlobalPos = true)
        {
            if (GlobalPos)
            {
                Vector3 v = new Vector3(value, tr.position.y, tr.position.z);
                tr.position = v;
            }
            else
            {
                Vector3 v = new Vector3(value, tr.localPosition.y, tr.localPosition.z);
                tr.localPosition = v;
            }
        }
        public static void SetY(this Transform tr, float value, bool GlobalPos = true)
        {
            if (GlobalPos)
            {
                Vector3 v = new Vector3(tr.position.x, value, tr.position.z);
                tr.position = v;
            }
            else
            {
                Vector3 v = new Vector3(tr.localPosition.x, value, tr.localPosition.z);
                tr.localPosition = v;
            }
        }
        public static void SetZ(this Transform tr, float value, bool GlobalPos = true)
        {
            if (GlobalPos)
            {
                Vector3 v = new Vector3(tr.position.x, tr.position.y, value);
                tr.position = v;
            }
            else
            {
                Vector3 v = new Vector3(tr.localPosition.x, tr.localPosition.y, value);
                tr.localPosition = v;
            }
        }

        public static void AddX(this Transform tr, float value, bool GlobalPos = true)
        {
            if (GlobalPos)
            {
                Vector3 v = tr.position + new Vector3(value, 0, 0);
                tr.position = v;
            }
            else
            {
                Vector3 v = tr.localPosition + new Vector3(value, 0, 0);
                tr.localPosition = v;
            }
        }
        public static void AddY(this Transform tr, float value, bool GlobalPos = true)
        {
            if (GlobalPos)
            {
                Vector3 v = tr.position + new Vector3(0, value, 0);
                tr.position = v;
            }
            else
            {
                Vector3 v = tr.localPosition + new Vector3(0, value, 0);
                tr.localPosition = v;
            }
        }
        public static void AddZ(this Transform tr, float value, bool GlobalPos = true)
        {
            if (GlobalPos)
            {
                Vector3 v = tr.position + new Vector3(0, 0, value);
                tr.position = v;
            }
            else
            {
                Vector3 v = tr.localPosition + new Vector3(0, 0, value);
                tr.localPosition = v;
            }
        }

        public static void AddVec(this Transform tr, Vector3 value, bool GlobalPos = true)
        {
            if (GlobalPos)
            {
                Vector3 v = tr.position + value;
                tr.position = v;
            }
            else
            {
                Vector3 v = tr.localPosition + value;
                tr.localPosition = v;
            }
        }

        public static void ClearChild(this Transform tr)
        {
            foreach (Transform child in tr)
                UnityEngine.Object.Destroy(child.gameObject);
        }
        #endregion

        #region GameUtil
        /// <summary>
        /// targetPos가 화면 범위를 벗어났는지 확인하는 함수
        /// </summary>
        /// <returns>True = 화면을 벗어남 False = 화면을 안에 존재함</returns>
        public static bool IsPositionOffScreen(Vector3 targetPos)
        {
            bool returnValue = false;

            //뷰포트 범위 확인
            Vector3 bottomLeftPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 topRightPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

            //x위치 체크
            if (targetPos.x < bottomLeftPos.x || targetPos.x > topRightPos.x)
            {
                returnValue = true;
            }

            if (targetPos.y < bottomLeftPos.y || targetPos.y > topRightPos.y)
            {
                returnValue = true;
            }

            return returnValue;
        }

        /// <summary>
        /// 카메라 테두리가 두 지점 사이에 존재하는 경우
        /// 카메라 테두리의 좌표를 반환합니다.
        /// </summary>

        //TODO : 중복된 함수가 많아 리펙토링 필요
        public static Vector2 CalcCameraIntersection(Vector3 startPos, Vector3 endPos)
        {
            float checkLength = 5000f;

            // boundaryCollider의 왼쪽 아래, 오른쪽 위 좌표 가져오기
            Vector3 bottomLeftPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 bottomRightPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
            Vector3 topLeftPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
            Vector3 topRightPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

            bottomLeftPos.z = 0;
            bottomRightPos.z = 0;
            topLeftPos.z = 0;
            topRightPos.z = 0;


            Vector2 xIntersectionPoint = default;
            Vector2 yIntersectionPoint = default;
            Vector2 returnPoint = default;

            //1. 방향을 구한다.
            Vector3 direction = endPos - startPos;
            //Debug.DrawLine(startPos, endPos, Color.green);

            //2. 방향과 맞는 두 선분의 교점을 구한다.
            if (direction.x >= 0 && direction.y >= 0) //1사분면
            {
                //x축
                LineIntersection(
                    startPos, endPos,
                    topLeftPos, topLeftPos + Vector3.right * checkLength,
                    out xIntersectionPoint);
                //Debug.DrawLine(topLeftPos, topLeftPos + Vector3.right * checkLength, Color.red);

                //y축
                LineIntersection(
                    startPos, endPos,
                    bottomRightPos, bottomRightPos + Vector3.up * checkLength,
                    out yIntersectionPoint);
                //Debug.DrawLine(bottomRightPos, bottomRightPos + Vector3.up * checkLength, Color.red);
            }
            else if (direction.x <= 0 && direction.y >= 0) //2사분면
            {
                //x축
                LineIntersection(
                    startPos, endPos,
                    topRightPos, topRightPos + Vector3.left * checkLength,
                    out xIntersectionPoint);
                //Debug.DrawLine(topRightPos, topRightPos + Vector3.left * checkLength, Color.red);

                //y축
                LineIntersection(
                    startPos, endPos,
                    bottomLeftPos, bottomLeftPos + Vector3.up * checkLength,
                    out yIntersectionPoint);
                //Debug.DrawLine(bottomLeftPos, bottomLeftPos + Vector3.up * checkLength, Color.red);

            }
            else if (direction.x <= 0 && direction.y <= 0) //3사분면
            {
                //x축
                LineIntersection(
                    startPos, endPos,
                    bottomRightPos, bottomRightPos + Vector3.left * checkLength,
                    out xIntersectionPoint);
                //Debug.DrawLine(bottomRightPos, bottomRightPos + Vector3.left * checkLength, Color.red);

                //y축
                LineIntersection(
                    startPos, endPos,
                    topLeftPos, topLeftPos + Vector3.down * checkLength,
                    out yIntersectionPoint);
                //Debug.DrawLine(topLeftPos, topLeftPos + Vector3.down * checkLength, Color.red);
            }
            else if (direction.x >= 0 && direction.y <= 0) //4사분면
            {
                //x축
                LineIntersection(
                    startPos, endPos,
                    bottomLeftPos, bottomLeftPos + Vector3.right * checkLength,
                    out xIntersectionPoint);
                //Debug.DrawLine(bottomLeftPos, bottomLeftPos + Vector3.right * checkLength, Color.red);

                //y축
                LineIntersection(
                    startPos, endPos,
                    topRightPos, topRightPos + Vector3.down * checkLength,
                    out yIntersectionPoint);
                //Debug.DrawLine(topRightPos, topRightPos + Vector3.down * checkLength, Color.red);
                //Debug.DrawLine(topRightPos, topRightPos + Vector3.down * checkLength, Color.red);
            }

            //3. 거리가 짧은 교점을 선택한다
            //3-1. 이 때 둘 중 하나가 0이면 다른것을 선택한다.
            float xLength = xIntersectionPoint == default ? 0 : Vector3.Distance(startPos, xIntersectionPoint);
            float yLength = yIntersectionPoint == default ? 0 : Vector3.Distance(startPos, yIntersectionPoint);

            if (xLength == 0)
            {
                returnPoint = yIntersectionPoint;
            }
            else if (yLength == 0)
            {
                returnPoint = xIntersectionPoint;
            }
            else
            {
                if (xLength < yLength)
                {
                    returnPoint = xIntersectionPoint;
                }
                else
                {
                    returnPoint = yIntersectionPoint;
                }
            }

            if (returnPoint == default)
                returnPoint = Vector3.up * 10000;

            //4. 교차점 벡터를 반환한다.
            return returnPoint;
        }
        #endregion

        #region Color
        public static Color GetChangeAlpha(this Color color, float value_0_1)
        {
            return new Color(color.r, color.g, color.b, value_0_1);
        }

        //TODO : 컴포넌트가 더 나으려나?
        public static Color ObjectGetColor(UnityEngine.Object obj)
        {
            Color color = default;
            switch (obj.GetType().Name)
            {
                case nameof(Image):
                    Image image = (Image)obj;
                    color = image.color;
                    break;
                case nameof(Text):
                    Text txt = (Text)obj;
                    color = txt.color;
                    break;
                case nameof(TextMesh):
                    TextMesh txtMesh = (TextMesh)obj;
                    color = txtMesh.color;
                    break;
                case nameof(TextMeshProUGUI):
                    TextMeshProUGUI tmpUGUI = (TextMeshProUGUI)obj;
                    color = tmpUGUI.color;
                    break;
                case nameof(SpriteRenderer):
                    SpriteRenderer spriteRenderer = (SpriteRenderer)obj;
                    color = spriteRenderer.color;
                    break;
                default:
                    break;
            }
            return color;
        }

        public static void ObjectSetColor(UnityEngine.Object obj, Color color)
        {
            switch (obj.GetType().Name)
            {
                case nameof(Image):
                    Image image = (Image)obj;
                    image.color = color;
                    break;
                case nameof(Text):
                    Text txt = (Text)obj;
                    txt.color = color;
                    break;
                case nameof(TextMesh):
                    TextMesh txtMesh = (TextMesh)obj;
                    txtMesh.color = color;
                    break;
                case nameof(TextMeshProUGUI):
                    TextMeshProUGUI tmpUGUI = (TextMeshProUGUI)obj;
                    tmpUGUI.color = color;
                    break;
                case nameof(SpriteRenderer):
                    SpriteRenderer spriteRenderer = (SpriteRenderer)obj;
                    spriteRenderer.color = color;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Enum
        // Enum 타입의 int 값 반환 확장 메소드
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
        #endregion

        #region String
        public static string GetSecond2Timer(float secondTime)
        {
            int minute = (int)secondTime / 60 % 60;
            float second = secondTime % 60f;
            int millisecond = Mathf.FloorToInt((secondTime - Mathf.Floor(secondTime)) * 1000);

            return string.Format("{0:D2}:{1:00}:{2:000}", minute, (int)secondTime % 60, millisecond);
        }

        public static string GetEnum2Str(object key)
        {
            return Enum.GetName(key.GetType(), key);
        }

        /// <summary>
        /// 검색할 단어와 리스트를 넘겨주면
        /// 검색된 리스트를 반환하는 함수
        /// </summary>
        public static List<string> SearchTextNormalize(string inputText, List<string> searchGroup)
        {
            List<string> findGroup = new List<string>();

            //inputText와 searchGroup의 NDF 변환
            var inputTextNfd = inputText.Normalize(System.Text.NormalizationForm.FormKD);
            List<string> searchNfd = new List<string>();
            foreach (var item in searchGroup)
            {
                searchNfd.Add(item.Normalize(System.Text.NormalizationForm.FormKD));
            }

            //검색
            int idx = -1;
            for (int i = 0; i < searchNfd.Count; i++)
            {
                idx = searchNfd[i].IndexOf(inputTextNfd, StringComparison.Ordinal);
                if (idx != -1)
                    findGroup.Add(searchGroup[i]);
            }

            return findGroup;
        }
        #endregion

        #region Production
        //TODO : 현재 구조는 들어가는 값이 많아 작성이 어려움,
        //일반 클래스로 만들어서
        //필수값들은 생성자로 받고,
        //그 이외의 요소는 DoTween과 같은 체인 형태로 연결한다면 좋지 않을까?

        /// <summary>
        /// 페이드 인 아웃
        /// </summary>
        public static IEnumerator Co_FadePlay(Action callback, UnityEngine.Object obj, Ease ease, float prodTime, float startAlpha, float endAlpha, float delayTime = 0f, bool isRealTime = false)
        {
            if (delayTime != 0)
            {
                if (!isRealTime)
                {
                    yield return new WaitForSeconds(delayTime);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(delayTime);
                }
            }

            float t = 0f;
            Color fadecolorImage = default;

            while (t < 1)
            {
                if (!isRealTime)
                {
                    t += Time.deltaTime / prodTime;
                }
                else
                {
                    t += Time.unscaledDeltaTime / prodTime;
                }

                fadecolorImage = ObjectGetColor(obj);
                fadecolorImage.a = Mathf.Lerp(startAlpha, endAlpha, GetEaseValue(t, ease));
                ObjectSetColor(obj, fadecolorImage);

                if (!isRealTime)
                {
                    yield return 0;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
                }
            }

            //사후조건
            fadecolorImage.a = endAlpha;
            ObjectSetColor(obj, fadecolorImage);
            callback?.Invoke();
        }

        /// <summary>
        /// 색 변경 함수
        /// </summary>
        public static IEnumerator Co_ColorChange(Action callback, UnityEngine.Object obj, Ease ease, float prodTime, Color startColor, Color endColor, float delayTime = 0)
        {
            if (delayTime != 0)
                yield return new WaitForSeconds(delayTime);

            float t = 0f;
            Color tempColor;
            Color tempAlphaColor;

            while (t < 1)
            {
                t += Time.deltaTime / prodTime;
                tempAlphaColor = ObjectGetColor(obj);
                tempColor = Color.Lerp(startColor, endColor, GetEaseValue(t, ease));
                tempColor.a = tempAlphaColor.a;
                ObjectSetColor(obj, tempColor);
                yield return 0;
            }

            //사후조건
            callback?.Invoke();
        }

        /// <summary>
        /// 값 변경 함수
        /// </summary>
        public static IEnumerator Co_ValueAtoB(Action callback, UnityEngine.Object obj, float startValue, float endValue, Ease ease, float prodTime = 1, float delayTime = 0)
        {
            yield return new WaitForSeconds(delayTime);

            //current -> 목표치
            float t = 0;
            float currentValue = startValue;

            float tempValue = 0;

            //TODO : tempValue = 레벨업 프로그래스 용도 임시값 / 수정 필요
            while (t < 1 && tempValue < 1)
            {
                t += Time.deltaTime / prodTime;

                tempValue = Mathf.Lerp(currentValue, endValue, GetEaseValue(t, ease));
                SetValue(tempValue);

                if (tempValue > 1)
                    break;
                yield return 0;
            }

            //사후조건
            callback?.Invoke();

            void SetValue(float value)
            {
                switch (obj.GetType().Name)
                {
                    case nameof(Slider):
                        Slider slider = (Slider)obj;
                        slider.value = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public static IEnumerator Co_MoveAtoB(Action callback, Transform targetTr, Vector3 startPos, Vector3 endPos, Ease ease, float prodTime = 1, float delayTime = 0, bool isGlobalPos = true)
        {
            if (delayTime != 0)
                yield return new WaitForSeconds(delayTime);

            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / prodTime;
                if (isGlobalPos)
                    targetTr.position = Vector3.Lerp(startPos, endPos, GetEaseValue(t, ease)); // Lerp 함수로 이동
                else
                    targetTr.localPosition = Vector3.Lerp(startPos, endPos, GetEaseValue(t, ease)); // Lerp 함수로 이동
                yield return 0;
            }

            //사후조건
            callback?.Invoke();
        }

        public static IEnumerator Co_ScaleAtoB(Action callback, Transform targetTr, Vector3 startScale, Vector3 endScale, Ease ease, float prodTime = 1, float delayTime = 0)
        {
            if (delayTime != 0)
                yield return new WaitForSeconds(delayTime);

            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / prodTime;
                targetTr.localScale = Vector3.Lerp(startScale, endScale, GetEaseValue(t, ease)); // Lerp 함수로 이동
                yield return 0;
            }

            //사후조건
            callback?.Invoke();
        }

        #endregion

        #region Math
        #region EasingFunction
        //ease https://easings.net/ko
        public enum Ease
        {
            None,
            Linear,
            EaseInSine,
            EaseInCirc,
            EaseOutSine,
            EaseOutCubic,
        }

        //230319 수정 : t값이 0~1사이값으로 제한되도록 수정
        public static float GetEaseValue(float t, Ease ease)
        {
            float returnValue = -1f;
            t = Mathf.Clamp01(t);
            switch (ease)
            {
                case Ease.None:
                    break;
                case Ease.Linear:
                    returnValue = t;
                    break;
                case Ease.EaseInSine:
                    returnValue = EaseInSine(t);
                    break;
                case Ease.EaseInCirc:
                    returnValue = EaseInCirc(t);
                    break;
                case Ease.EaseOutSine:
                    returnValue = EaseOutSine(t);
                    break;
                case Ease.EaseOutCubic:
                    returnValue = EaseOutCubic(t);
                    break;
                default:
                    break;
            }
#if UNITY_EDITOR
            if (returnValue == -1f)
                Debug.LogError("에러");
#endif
            return returnValue;
        }

        public static float EaseInSine(float x)
        {
            return (1 - Mathf.Cos((x * Mathf.PI) / 2));
        }

        public static float EaseInCirc(float x)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
        }

        //TODO : 테스트 안해봄
        public static float EaseOutSine(float x)
        {
            return Mathf.Sin((x * Mathf.PI) / 2);
        }

        public static float EaseOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }


        #endregion
        #region Random
        /// <summary>
        /// 랜덤 가중치 함수
        /// </summary>
        /// <returns>Index</returns>
        public static int RandomWeightedIndex(float[] randomValues)
        {
            float totalWeight = 0.0f;
            foreach (var value in randomValues)
            {
                if (value < 0.0f)
                {
                    throw new System.ArgumentOutOfRangeException("SpawnPercent should be greater than or equal to 0.");
                }
                totalWeight += value;
            }

            if (totalWeight <= 0.0f)
            {
                throw new System.ArgumentException("Total weight should be greater than 0.");
            }

            float randomValue = Random.Range(0.0f, 1.0f) * totalWeight;

            float accumulatedWeight = 0.0f;
            for (int i = 0; i < randomValues.Length; i++)
            {
                accumulatedWeight += randomValues[i];
                if (randomValue < accumulatedWeight)
                {
                    return i;
                }
            }

            // 랜덤 인덱스를 찾을 수 없으면 첫 번째 인덱스를 반환한다.
            Debug.Log("A");
            return 0;
        }
        #endregion
        #region Else
        /// <summary> 두 선분의 교차점을 구하는 함수 </summary>
        public static bool LineIntersection(Vector2 aStartPos, Vector2 aEndPos, Vector2 bStartPos, Vector2 bEndPos, out Vector2 intersection)
        {
            intersection = Vector2.zero;
            Vector2 b = aEndPos - aStartPos;
            Vector2 d = bEndPos - bStartPos;
            float bDotDPerp = b.x * d.y - b.y * d.x;

            // b와 d의 평행 여부를 검사
            if (Mathf.Approximately(bDotDPerp, 0))
            {
                return false;
            }

            Vector2 c = bStartPos - aStartPos;
            float t = (c.x * d.y - c.y * d.x) / bDotDPerp;

            // 교차점이 두 선분 내부에 있는지 검사
            if (t < 0 || t > 1)
            {
                return false;
            }

            float u = (c.x * b.y - c.y * b.x) / bDotDPerp;

            // 교차점이 두 선분 내부에 있는지 검사
            if (u < 0 || u > 1)
            {
                return false;
            }

            intersection = aStartPos + t * b;

            return true;
        }

        /// <summary> Angle을 Direction으로 변환하는 함수 </summary>
        public static Vector2 AngleToDirection(float angle)
        {
            // 각도를 라디안으로 변환
            float angleInRadians = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(angleInRadians);
            float y = Mathf.Sin(angleInRadians);

            x = x > 0.001f ? x : 0;
            y = y > 0.001f ? y : 0;

            // 각도로부터 방향 벡터 계산
            Vector2 direction = new Vector2(x, y);

            // 결과 출력 (디버깅용)
            Debug.Log("Direction: " + direction);
            return direction;
        }

        /// <summary> Direction을 Angle로 변환하는 함수 </summary>
        public static float DirectionToAngle(Vector2 dir)
        {
            dir = dir.normalized;
            float angleInRadians = Mathf.Atan2(dir.y, dir.x);
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
            return angleInDegrees;
        }

        #endregion
        #endregion
    }
}