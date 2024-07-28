using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    [ExcelAsset]
    public class ExImporterScript : ScriptableObject
    {
        [SerializeField] private List<Example1Entity> Example1;
        [SerializeField] private List<Example2Entity> Example2;
    }
}
