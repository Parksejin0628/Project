using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting
{
    public class ExcelImporter : AssetPostprocessor
    {
        //변경 시 ExcelAssetScriptMenu도 수정 필요
        public static readonly string EXCEL_NAME = "Excel";
        public static readonly string SCRIPT_NAME = "Script";
        public static readonly string ASSET_NAME = "Asset";

        class ExcelAssetInfo
        {
            public Type AssetType { get; set; }
            public ExcelAssetAttribute Attribute { get; set; }
            public string OriginalName
            {
                get
                {
                    return string.IsNullOrEmpty(Attribute.ExcelName) ? AssetType.Name.Replace(SCRIPT_NAME, "") : Attribute.ExcelName;
                }
            }
        }

        static List<ExcelAssetInfo> cachedInfos = null; // Clear on compile.

        //진입점
        //임의의 수의 자산 가져오기가 완료된 후 호출됩니다(자산 진행률 표시줄이 끝에 도달한 경우).
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool imported = false; //엑셀 임포트 확인 변수
            foreach (string path in importedAssets) //임포트 체크
            {
                if (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx") //Path의 확장명이 엑셀이라면
                {
                    // static변수는 컴파일할 때마다 초기화된다.
                    if (cachedInfos == null)
                        cachedInfos = FindExcelAssetInfos(); //[ExcelAsset] Attribute가 달려있는 스크립트를 찾아 리스트에 저장

                    //??? 디버그 상에서 범위에 없다고 뜨는 이유가 무엇일까?
                    //경로에서 확장자가 없는 파일의 이름만 가져온다. Ex)MstItems
                    var excelName = Path.GetFileNameWithoutExtension(path); ;
                    if (excelName.StartsWith("~$"))//실행중인 엑셀을 체크한 경우 continue
                        continue;

                    //캐시된 곳에서 현재 임포트한 엑셀의 path로 불러온 이름과 같은 ExcelAssetInfo를 가져온다.
                    ExcelAssetInfo info = cachedInfos.Find(i => i.OriginalName == excelName.Replace(EXCEL_NAME, ""));
                    if (info == null) //찾은 게 없으면 continue
                        continue;

                    ImportExcel(path, info); // 실질적으로 값을 변경하는 곳
                    imported = true;
                }
            }

            //임포트한 것이 Excel이라면
            //foreach에서 Excel 값에 맞게 스크립터블 파일 수정해주고
            //바뀐게 있는 경우 다시 Refresh 해준다.
            if (imported)
            {
                //이 때에는 importedAssets에 스크립터블이 들어가 있기에 foreach의 if문에 접근하지 않는다.
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        //전체 파일에서 [ExcelAsset] Attribute가 달려있는 스크립트를 찾아 리스트에 저장
        static List<ExcelAssetInfo> FindExcelAssetInfos()
        {
            var list = new List<ExcelAssetInfo>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) //어셈블리 탐색
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attributes = type.GetCustomAttributes(typeof(ExcelAssetAttribute), false); // 타입에서 [ExcelAsset] Attribute가 달려있는지 확인
                    if (attributes.Length == 0) continue;
                    var attribute = (ExcelAssetAttribute)attributes[0]; //갖고와서
                    var info = new ExcelAssetInfo() // 새 인스턴스를 만들어 값을 집어넣고
                    {
                        AssetType = type,
                        Attribute = attribute
                    };

                    list.Add(info); // 리스트에 추가
                }
            }
            return list;
        }

        //기존 스크립터블 찾고 없으면 생성해서 리턴
        static UnityEngine.Object LoadOrCreateAsset(string assetPath, Type assetType)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(assetPath)); //폴더 생성 ??? 이미 있는 경우에도 계속 생성하려 하지 않을까

            var asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType); //기존에 생성된 거 찾기

            if (asset == null)  //없으면 생성해서 리턴시키기
            {
                asset = ScriptableObject.CreateInstance(assetType.Name); //??? 메모리 상에만 올리는 건가?
                AssetDatabase.CreateAsset((ScriptableObject)asset, assetPath); //??? 경로에 생성한다.
                asset.hideFlags = HideFlags.NotEditable; //인스펙터 수정 금지
            }

            return asset;
        }

        //엑셀 파일 불러오기
        static IWorkbook LoadBook(string excelPath)
        {
            //using이 없어도 잘 작동한다.
            //FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //if (Path.GetExtension(excelPath) == ".xls")
            //    return new HSSFWorkbook(stream);
            //else
            //    return new XSSFWorkbook(stream);

            using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (Path.GetExtension(excelPath) == ".xls")
                    return new HSSFWorkbook(stream);
                else
                    return new XSSFWorkbook(stream);
            }
        }

        //header의 왼쪽부터 공백이 나오기 전까지 리스트에 담아서 리턴시킨다.
        static List<string> GetFieldNamesFromSheetHeader(ISheet sheet)
        {
            IRow headerRow = sheet.GetRow(0); //첫 번째 열을 가져와

            var fieldNames = new List<string>();

            //첫 번째 열의 필드 이름들을 담는다.
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                var cell = headerRow.GetCell(i); //첫 번째 열 0번부터 비교하면서
                if (cell == null || cell.CellType == CellType.Blank) break; //빈칸이면 나가고
                fieldNames.Add(cell.StringCellValue); //아닌 경우 추가한다.
            }
            return fieldNames;
        }

        //셀 값을 오브젝트 타입으로 가져온다
        static object CellToFieldObject(ICell cell, FieldInfo fieldInfo, bool isFormulaEvalute = false)
        {
            //셀의 타입을 가져와서
            var type = isFormulaEvalute ? cell.CachedFormulaResultType : cell.CellType;

            if (fieldInfo.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                string assetPath = cell.StringCellValue;
                if (assetPath == "Not Load")
                    return null;

                UnityEngine.Object asset = Resources.Load(assetPath, fieldInfo.FieldType);
                if (asset == null)
                    Debug.LogWarning($"asset 로드에 실패했습니다.\nPath: {assetPath}\n");

                return asset;
            }
            else if (fieldInfo.FieldType == typeof(Vector2))
            {
                string[] split = Regex.Replace(cell.StringCellValue, "[()]", "").Split(',');
                if (split.Length != 2)
                    throw new FormatException("Input string is not in the correct format for a Vector2: " + cell.StringCellValue);

                if (float.TryParse(split[0].Trim(), out float x) && float.TryParse(split[1].Trim(), out float y))
                    return new Vector2(x, y);
                else
                    throw new FormatException("One or both coordinates could not be converted to float: " + cell.StringCellValue);
            }
            else if (fieldInfo.FieldType == typeof(Vector3))
            {
                string[] split = Regex.Replace(cell.StringCellValue, "[()]", "").Split(',');
                if (split.Length != 3)
                    throw new FormatException("Input string is not in the correct format for a Vector3: " + cell.StringCellValue);

                if (float.TryParse(split[0].Trim(), out float x) && float.TryParse(split[1].Trim(), out float y) && float.TryParse(split[2].Trim(), out float z))
                    return new Vector3(x, y, z);
                else
                    throw new FormatException("One or more coordinates could not be converted to float: " + cell.StringCellValue);
            }
            else if (fieldInfo.FieldType == typeof(Color))
            {
                ICellStyle cellStyle = cell.CellStyle;
                if (cellStyle is XSSFCellStyle)
                {
                    XSSFCellStyle xssfCellStyle = (XSSFCellStyle)cellStyle;
                    XSSFColor xssfColor = xssfCellStyle.FillForegroundColorColor as XSSFColor;
                    if (xssfColor != null && xssfColor.RGB != null)
                    {
                        byte[] rgb = xssfColor.RGB;
                        if (rgb.Length == 3) // RGB 색상만 있는 경우
                        {
                            return new Color(rgb[0] / 255f, rgb[1] / 255f, rgb[2] / 255f);
                        }
                        else if (rgb.Length == 4) // ARGB 색상인 경우
                        {
                            return new Color(rgb[1] / 255f, rgb[2], rgb[3] / 255f, rgb[0] / 255f);
                        }
                    }
                }
            }

            //타입에 맞는 값을 리턴시켜준다.
            switch (type)
            {
                case CellType.String:
                    if (fieldInfo.FieldType.IsEnum) return Enum.Parse(fieldInfo.FieldType, cell.StringCellValue);
                    else return cell.StringCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    return Convert.ChangeType(cell.NumericCellValue, fieldInfo.FieldType);
                case CellType.Formula:
                    if (isFormulaEvalute) return null;
                    return CellToFieldObject(cell, fieldInfo, true);
                default:
                    if (fieldInfo.FieldType.IsValueType)
                    {
                        return Activator.CreateInstance(fieldInfo.FieldType);
                    }
                    return null;
            }
        }

        // 액셀의 값을 불러와 받아온 entityType에 맞는 형태로 값을 세팅해준다
        static object CreateEntityFromRow(IRow row, List<string> columnNames, Type entityType, string sheetName)
        {
            var entity = Activator.CreateInstance(entityType); //인스턴스 생성

            //column 개수만큼 반복
            for (int i = 0; i < columnNames.Count; i++)
            {
                //대강 느낌만
                //columnName과 일치하는 Field(변수)를 가져온다 | 여기까진 클래스 그 자체
                FieldInfo entityField = entityType.GetField(
                    columnNames[i],
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );
                if (entityField == null) continue;
                if (!entityField.IsPublic && entityField.GetCustomAttributes(typeof(SerializeField), false).Length == 0) continue;

                ICell cell = row.GetCell(i);//셀을 불러와
                if (cell == null) continue;

                try
                {
                    object fieldValue = CellToFieldObject(cell, entityField); //셀 타입에 맞게 변환하여 값을 가져온 다움
                    entityField.SetValue(entity, fieldValue); //위에서 생성한 entity instance의 Field에 값을 집어넣어준다.
                }
                catch
                {
                    throw new Exception(string.Format("Invalid excel cell type at row {0}, column {1}, {2} sheet.\n", row.RowNum, cell.ColumnIndex, sheetName));
                }
            }
            return entity;
        }

        //정보를 종합하여 오브젝트 형태로 담는다.
        static object GetEntityListFromSheet(ISheet sheet, Type entityType)
        {
            List<string> excelColumnNames = GetFieldNamesFromSheetHeader(sheet); //시트 리스트 가져오기

            Type listType = typeof(List<>).MakeGenericType(entityType); //받아온 타입으로 List<type>을 생성한다.

            //???
            //List.Add 메소드를 reflaction으로 가져오고 인수 Type배열 안의 타입이 존재하면 메소드를, 없으면 null을 반환한다
            //리스트는 제네릭이라 null이 반환될 일은 없을 것 같고
            // 조사식으로 확인했더니 listAddMethod의 값 = "Void Add(MstItemEntity1)" 이 들어간다.
            //https://learn.microsoft.com/ko-kr/dotnet/api/system.type.getmethod?view=net-6.0#system-type-getmethod(system-string-system-type())
            MethodInfo listAddMethod = listType.GetMethod("Add", new Type[] { entityType });
            //이 친구는 배열도 아니고 리스트도 아니다? 그러면 뭐지????
            //int 배열 = int[]
            //int List = System.Collections.Generic.List<int>
            //아래 list = object
            object list = Activator.CreateInstance(listType);


            // row of index 0 is header
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                /* i번째 열을 가져와서 없으면 파괴
                 * ■■■
                 * □□□
                 * □□□
                 */
                IRow row = sheet.GetRow(i);
                if (row == null) break;


                /* 현재 열의 첫 번째 셀을 가져와서
                 * 비어있으면 파괴
                 * ■□□
                 * □□□
                 * □□□
                 */
                ICell entryCell = row.GetCell(0);
                if (entryCell == null || entryCell.CellType == CellType.Blank) break;

                //값이 존재하지만 #으로 시작하면 continue
                if (entryCell.CellType == CellType.String && entryCell.StringCellValue.StartsWith("#")) continue;

                var entity = CreateEntityFromRow(row, excelColumnNames, entityType, sheet.SheetName); // 값을 불러와서
                listAddMethod.Invoke(list, new object[] { entity });
                // 첫 인수 list에 두 번째 인수 0번 인덱스의 entity를 내용을 추가시킨다.
                // 근데 위에서는 Type[]인데 여기서는 왜 object[]인 것일까?

                // 조사식으로 확인했더니 listAddMethod의 값 = "Void Add(MstItemEntity1)" 
                // list의 조사식을 확인하니 object 안에 MstItemEntity1 배열로 들어가 있다.


            }
            return list;
        }

        //Excel에서 값을 추출해서 스크립터블에 세팅해준다.
        static void ImportExcel(string excelPath, ExcelAssetInfo info)
        {

            string assetPath = "";
            //TODO : 에셋의 파일명 지정
            string assetName = info.OriginalName + ASSET_NAME + ".asset"; //파일명 지정

            //info.Attribute.AssetPath = "\\HAHA"; //???경로를 정해주면 생성이 안되는 이유가 뭐지

            //TODO : 스크립트를 바탕으로 에셋 생성
            if (string.IsNullOrEmpty(info.Attribute.AssetPath)) //??? 경로가 지정되어 있지 않다면 엑셀 경로에 스크립터블 생성한다 이런느낌인가
            {
                //엑셀 경로와 동일하게
                string basePath = Path.GetDirectoryName(excelPath); // 스크립터블을 생성할 경로 지정
                assetPath = Path.Combine(basePath, assetName);// 경로 뒤에 생성할 파일명 붙이기
            }
            else
            {
                var path = Path.Combine("Assets", info.Attribute.AssetPath);
                assetPath = Path.Combine(path, assetName);
            }
            UnityEngine.Object asset = LoadOrCreateAsset(assetPath, info.AssetType); //변경할 스크립터블 획득

            //엑셀 파일 불러오기
            IWorkbook book = LoadBook(excelPath);

            //ExcelAssetInfo에서 필드(시트 정보 포함 / "Entities1", "Entities2")의 정보를 가져온다
            //형식 : System.Reflection.FieldInfo[]
            var assetFields = info.AssetType.GetFields(
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            int sheetCount = 0; //로그용 변수

            //필드, 즉 변수들을 불러와 foreach로 체크한 후 적용한다.
            foreach (var assetField in assetFields)
            {
                ISheet sheet = book.GetSheet(assetField.Name); // 필드 이름과 일치하는 시트를 가져와서
                if (sheet == null) continue;

                Type fieldType = assetField.FieldType; //필드 타입을 가져와서

                //무결성 검사
                //1. 필드의 타입이 제네릭 타입인가
                //2. 필드의 타입이 리스트인지 체크
                if (!fieldType.IsGenericType || (fieldType.GetGenericTypeDefinition() != typeof(List<>))) continue;

                Type[] types = fieldType.GetGenericArguments(); //스크립터블 필드의 리스트 타입을 가져온다.
                Type entityType = types[0]; //리스트는 제네릭이 하나이므로 0번 하나밖에 없다.

                object entities = GetEntityListFromSheet(sheet, entityType); //정보를 종합하여 오브젝트 형태로 담은 후
                assetField.SetValue(asset, entities); //스크립터블 필드에 값을 집어넣어준다
                sheetCount++;
            }

            if (info.Attribute.LogOnImport)
            {
                Debug.Log(string.Format("Imported {0} sheets form {1}.", sheetCount, excelPath));
            }

            EditorUtility.SetDirty(asset);
        }
    }
}