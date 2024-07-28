using DefaultSetting.Utility;
using UnityEngine;
using UnityEngine.UI;

public class ExScrollRect : ScrollRect
{
    [Header("[Extended Variable]")]
    [SerializeField] public bool isAutoChangeElasticAndClamped = true;

    protected override void Awake()
    {
        base.Awake();

        if (isAutoChangeElasticAndClamped)
        {
            var sensor = content?.gameObject?.GetOrAddComponent<OnRectTransformDimensionsChangeSensor>();
            if (sensor != null)
            {
                sensor.AddAction(CheckScroll);
                CheckScroll();
                Debug.Log("A");
            }
        }
    }

    private void CheckScroll()
    {
        var viewportHeight = viewport.rect.height;
        var contentHeight = content.rect.height;
        movementType = viewportHeight > contentHeight ? MovementType.Clamped : MovementType.Elastic;
    }
}
