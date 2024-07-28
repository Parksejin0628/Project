using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace DefaultSetting
{
    public class LocalizedSetting : MonoBehaviour
    {
        [SerializeField, ReadOnly] private string localizedID = null;
        [SerializeField, ReadOnly] private TextMeshProUGUI tmp = null;

        public void SetDefaultID(string uiName)
        {
            localizedID = $"{uiName}_{gameObject.name}";
            tmp = GetComponent<TextMeshProUGUI>();

            if (tmp == null)
            {
                Debug.LogWarning("TMP 존재X");
                return;
            }
            //글꼴 변경하기
        }

        public void UpdateLocalized()
        {
            if (localizedID == null)
            {
                Debug.LogWarning("ID 설정X");
                return;
            }

            //설정된 언어에 따라 변경하기
            //설정된 언어는 게임에서 가지고 있거나 데이터에서 가지고 있는게 맞겠다.
            string tempStr = Managers.Data.MstMaster.MstLocalizeDataAsset.GetLocalizedText(localizedID);
            if (tempStr == null)
                return;

            //대괄호 포함 여부 측정
            bool isContainBrackets = ContainsBrackets(tempStr);
            if (isContainBrackets == false)
            {
                tmp.text = tempStr;
                return;
            }

            //대괄호가 있다면 키를 변경할 값으로 바꾼 후
            //그것을 replace하여 출력할 텍스트로 만들어준 다음
            string checkInBrace = GetWordsInCurlyBrackets(tempStr); //중괄호 속 키 확인
            string key2String = KeyToText(checkInBrace); //키를 문자열로
            string finallyString = ModifyStringWithBrackets(tempStr, key2String);

            //변경해준다
            tmp.text = finallyString;
        }

        //예상: 대괄호 포함 확인하는 코드
        public static bool ContainsBrackets(string input)
        {
            // 정규 표현식을 사용하여 대괄호가 포함되어 있는지 확인합니다.
            string pattern = @"\{.*?\}";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        //대괄호 안의 내용을 변경하자
        //replacement에는 변경할 키가 들어가야겠지? -> KeyToText로 미리 넣어서 가져온다.
        public static string ModifyStringWithBrackets(string input, string replacement)
        {
            if (replacement == null)
            {
                Debug.LogWarning("변경할 문자를 못받음");
                return null;
            }

            // 정규 표현식을 사용하여 대괄호 안의 내용을 찾습니다.
            string pattern = @"\{(.*?)\}";
            Regex regex = new Regex(pattern);

            // 대괄호 안의 내용을 replacement로 대체합니다.
            string result = regex.Replace(input, replacement);

            return result;
        }

        public static string GetWordsInCurlyBrackets(string input)
        {
            // 정규 표현식을 사용하여 중괄호 안에 있는 단어를 찾습니다.
            string pattern = @"\{([^}]*)\}";
            Regex regex = new Regex(pattern);

            // 중괄호 안에 있는 모든 단어를 배열로 추출합니다.
            MatchCollection matches = regex.Matches(input);
            string[] wordsInCurlyBrackets = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                wordsInCurlyBrackets[i] = matches[i].Groups[1].Value;
            }

            if (wordsInCurlyBrackets.Length == 1)
                return wordsInCurlyBrackets[0];
            else
                return null;
        }

        //내용을 변경할 함수
        public static string KeyToText(string str)
        {
            //괄호 안의 내용을 찾아서
            //대충 switch로 변경해준다.

            string returnText = null;

            //알맞는 값은 세팅해줘야함
            switch (str)
            {
                case "MoveHorizontal":
                    //returnText = Managers.Input.GetBindingKey(Define.KeyEnum.MoveHorizontal.ToString());
                    break;
                case "Jump":
                    //returnText = Managers.Input.GetBindingKey(Define.KeyEnum.Jump.ToString());
                    break;
                case "Dash":
                    //returnText = Managers.Input.GetBindingKey(Define.KeyEnum.Dash.ToString());
                    break;
                default:
                    Debug.LogWarning("키를 찾지 못함");
                    break;
            }

            return returnText;
        }
    }
}