using System.Collections.Generic;
using UnityEngine;


namespace DefaultSetting
{
    [ExcelAsset]
    public class MstLocalizeDataScript : ScriptableObject
    {
        public List<MstLocalizeDataEntity> DefaultStageDataEntities;

        public MstLocalizeDataEntity GetLocalizedData(string column_Name)
        {
            return DefaultStageDataEntities.Find(x => x.Column_Name == column_Name).DeepCopy();
        }

        public string GetLocalizedText(string column_Name)
        {
            MstLocalizeDataEntity localizedData = DefaultStageDataEntities.Find(x => x.Column_Name == column_Name);

            if (localizedData == null)
            {
                Debug.LogWarning($"{column_Name} 키 존재하지 않음");
                return null;
            }

            string str = null;
            switch (Managers.Data.currentLanguage)
            {
                case Define.Language.NotSetting:
                    break;
                case Define.Language.Korean:
                    str = localizedData.ko;
                    break;
                case Define.Language.English:
                    str = localizedData.en;
                    break;
                case Define.Language.Japanese:
                    str = localizedData.ja;
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
