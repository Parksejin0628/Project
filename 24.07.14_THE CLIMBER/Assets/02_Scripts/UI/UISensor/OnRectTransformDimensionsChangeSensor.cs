using System;
using UnityEngine.EventSystems;

public class OnRectTransformDimensionsChangeSensor : UIBehaviour
{
    Action rectTransformDimensionsChangeAction;

    public void AddAction(Action action)
    {
        rectTransformDimensionsChangeAction -= action;
        rectTransformDimensionsChangeAction += action;
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        if (rectTransformDimensionsChangeAction != null)
            rectTransformDimensionsChangeAction!.Invoke();
    }
}
