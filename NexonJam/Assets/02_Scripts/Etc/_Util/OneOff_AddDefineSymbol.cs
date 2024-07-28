using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OneOff_AddDefineSymbol : MonoBehaviour
{
#if UNITY_EDITOR
    public bool isLogicEnd = false;
    public string targetSymbol = "DISABLESTEAMWORKS";

    private void Reset()
    {
        //로직
        Logic();

        //제거
        StartCoroutine(CoDestroy());
    }

    private void Awake()
    {
        Debug.LogError("의미없는 컴포넌트 존재");

    }

    public void Logic()
    {
        AddDefineSymbols();

        isLogicEnd = true;
    }

    public void AddDefineSymbols()
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);

        if (!definesString.Contains(targetSymbol))
        {
            definesString += $";{targetSymbol}";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, definesString);
            Debug.Log("DISABLESTEAMWORKS define added.");
        }

    }

    IEnumerator CoDestroy()
    {
        yield return new WaitUntil(() => isLogicEnd);
        DestroyImmediate(this);
    }
#endif
}
