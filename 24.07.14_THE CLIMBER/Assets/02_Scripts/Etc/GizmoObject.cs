using UnityEditor;
using UnityEngine;

namespace DefaultSetting
{
    public class GizmoObject : MonoBehaviour
    {
        [SerializeField] private bool isShow = true;
        [SerializeField] private float radius = 1;
        [SerializeField] private Color gizmoColor = Color.white;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!isShow)
                return;

            Handles.color = gizmoColor;
            Handles.DrawSolidArc(transform.position, Vector3.back, transform.right, 360, radius);
        }
#endif
    }
}
