using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorChildMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    void OnDisable()
    {
        Selector parentController = GetComponentInParent<Selector>();
        if (parentController != null)
        {
            parentController.ChooseItem();
        }
    }
}
