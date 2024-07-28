using DefaultSetting.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConvertExScrollRect : MonoBehaviour
{
    [ContextMenu("Do Change Component")]
    public void DoChangeComponent()
    {
        ScrollRect scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null)
            return;

        var content = scrollRect.content;
        var horizontal = scrollRect.horizontal;
        var vertical = scrollRect.vertical;
        var movementType = scrollRect.movementType;
        var viewport = scrollRect.viewport;

        DestroyImmediate(scrollRect);

        ExScrollRect exScrollRect = gameObject.GetOrAddComponent<ExScrollRect>();
        exScrollRect.content = content;
        exScrollRect.horizontal = horizontal;
        exScrollRect.vertical = vertical;
        exScrollRect.movementType = movementType;
        exScrollRect.viewport = viewport;

#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif

        DestroyImmediate(this);
    }
}
