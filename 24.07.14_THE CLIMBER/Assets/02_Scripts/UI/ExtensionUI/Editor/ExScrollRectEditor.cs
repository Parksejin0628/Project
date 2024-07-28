using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ExScrollRect))]
public class ExScrollRectEditor : ScrollRectEditor
{
    SerializedProperty isAutoChangeElasticAndClampedProperty;

    protected override void OnEnable()
    {
        base.OnEnable();

        isAutoChangeElasticAndClampedProperty = serializedObject.FindProperty(nameof(ExScrollRect.isAutoChangeElasticAndClamped));
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(isAutoChangeElasticAndClampedProperty);
        serializedObject.ApplyModifiedProperties();
    }
}