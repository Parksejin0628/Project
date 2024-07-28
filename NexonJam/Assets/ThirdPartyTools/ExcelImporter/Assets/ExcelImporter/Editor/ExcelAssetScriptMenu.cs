using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using UnityEngine;

namespace DefaultSetting
{
    public class ExcelAssetScriptMenu
    {
        //변경 시 ExcelImporter도 수정 필요
        public static readonly string EXCEL_NAME = "Excel";
        public static readonly string SCRIPT_NAME = "Script";
        public static readonly string ASSET_NAME = "Asset";

        const string SCRIPT_TEMPLATE_NAME = "ExcelAssetScriptTemplete.cs.txt";
        const string FIELD_TEMPLATE = "\t//public List<EntityType> #FIELDNAME#; // Replace 'EntityType' to an actual type that is serializable.";

        [MenuItem("Assets/Create/ExcelAssetScript", false)]
        static void CreateScript()
        {
            string savePath = EditorUtility.SaveFolderPanel("Save ExcelAssetScript", Application.dataPath, ""); //파일 생성할 경로 설정
            //string savePath = "E:\\_My\\Practice\\Unity\\AssetTest\\Assets\\ExcelImporter\\Assets\\ExcelImporter\\Example\\Scripts\\ExcelAsset"; //파일 생성할 경로 설정
            if (savePath == "") return;

            var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets); //선택한 파일 가져와서

            string excelPath = AssetDatabase.GetAssetPath(selectedAssets[0]); //파일 경로 추출
            string fileName = Path.GetFileNameWithoutExtension(excelPath).Replace(EXCEL_NAME, "") + SCRIPT_NAME; //경로에서 이름 추출
            List<string> sheetNames = GetSheetNames(excelPath); //시트 이름 가져오기

            string scriptString = BuildScriptString(fileName, sheetNames); //스크립트 내용 작성

            //TODO : 엑셀을 바탕으로 한 스크립트의 파일명 변경
            string path = Path.ChangeExtension(Path.Combine(savePath, fileName), "cs"); // 패스와 파일명, 확장자로 경로 정해주기
            File.WriteAllText(path, scriptString); //경로와 내용을 인자로 넣어 파일 생성

            AssetDatabase.Refresh(); //데이터를 변경했거나 프로젝트 폴더에 추가-제거된 모든 자산을 가져오기
        }

        //선택된 아이템이 엑셀이라면 선택가능하도록 표현
        [MenuItem("Assets/Create/ExcelAssetScript", true)]
        static bool CreateScriptValidation()
        {
            var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
            if (selectedAssets.Length != 1) return false;
            var path = AssetDatabase.GetAssetPath(selectedAssets[0]);
            return Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx";
        }

        static List<string> GetSheetNames(string excelPath)
        {
            var sheetNames = new List<string>(); // 빈 리스트 생성
            using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) //using을 코드 내에서 사용할 수 있다고?
            {
                IWorkbook book = null;
                if (Path.GetExtension(excelPath) == ".xls") book = new HSSFWorkbook(stream); //확장자 체크 후 그에 맞는 객체 생성 (xls VS xlsx)
                else book = new XSSFWorkbook(stream);

                for (int i = 0; i < book.NumberOfSheets; i++) //시트 개수에 맞게 반복
                {
                    var sheet = book.GetSheetAt(i); //시트 추출
                    sheetNames.Add(sheet.SheetName); //시트의 이름을 Names 리스트에 추가
                }
            }
            return sheetNames;
        }

        static string GetScriptTempleteString()
        {
            string currentDirectory = Directory.GetCurrentDirectory(); //프로젝트 폴더 경로
            string[] filePath = Directory.GetFiles(currentDirectory, SCRIPT_TEMPLATE_NAME, SearchOption.AllDirectories); //딕셔너리 내 모든 경로 체크하여 두 번째 인자의 이름의 템플릿을 찾는다
            if (filePath.Length == 0) throw new Exception("Script template not found."); //파일을 못찾으면 예외처리

            string templateString = File.ReadAllText(filePath[0]); //템플릿 파일을 string 형태로 바꿔서
            return templateString; //반환시킨다.
        }

        static string BuildScriptString(string fileName, List<string> sheetNames)
        {
            string scriptString = GetScriptTempleteString(); //템플릿 파일을 string 형태로 받아온다.

            scriptString = scriptString.Replace("#ASSETSCRIPTNAME#", fileName); // 생성할 스크립트 이름을 엑셀 이름으로 바꾸기

            foreach (string sheetName in sheetNames) // 시트 개수만큼 반복
            {
                string fieldString = String.Copy(FIELD_TEMPLATE); // 상수로 정의한 string 템플릿을 가져온다.
                fieldString = fieldString.Replace("#FIELDNAME#", sheetName); // 필드 이름을 시트 이름으로 변경
                fieldString += "\n#ENTITYFIELDS#"; // 변수가 들어갈 위치를 추가하고
                scriptString = scriptString.Replace("#ENTITYFIELDS#", fieldString); // 변수명을 시트 이름으로 변경
            }
            scriptString = scriptString.Replace("#ENTITYFIELDS#\n", "");  //기존에 있던 EntityFields를 제거

            return scriptString;
        }
    }
}