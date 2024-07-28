using System;

namespace DefaultSetting
{
    [Serializable]
    public class MstLocalizeDataEntity
    {
        public string Column_Name;
        public string ko;
        public string en;
        public string ja;

        //TODO: 얘는 DeepCopy가 필요없을 것 같은데?
        public MstLocalizeDataEntity DeepCopy()
        {
            MstLocalizeDataEntity newCopy = new MstLocalizeDataEntity();

            newCopy.Column_Name = this.Column_Name;
            newCopy.ko = this.ko;
            newCopy.en = this.en;

            return newCopy;
        }
    }
}