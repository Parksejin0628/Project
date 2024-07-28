using DefaultSetting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public Define.Scene targetScene;

    public void OnSygnal()
    {
        Managers.Scene.LoadScene(targetScene);
    }
}
