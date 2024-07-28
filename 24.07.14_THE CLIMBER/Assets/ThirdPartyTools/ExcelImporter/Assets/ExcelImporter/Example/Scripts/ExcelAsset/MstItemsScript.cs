using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    [ExcelAsset]
    public class MstItemsScript : ScriptableObject
    {
        public List<MstItemEntity> Entities; // Replace 'EntityType' to an actual type that is serializable.
    }
}
