using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
    public class FollowCam2D : MonoBehaviour
    {
        private enum ExecutableFunction
        {
            LateUpdate,
            FixedUpdate,
        }
        [SerializeField] private ExecutableFunction executableFunction = ExecutableFunction.LateUpdate;

        public Transform targetTr;
        private Transform camTr;

        [Range(2.0f, 20.0f)]
        public float distance = 10.0f;

        [Range(0.0f, 10.0f)]
        public float height = 2.0f;

        [Range(0.0f, 50.0f)]
        public float camSize = 24f;
        private float _lastCamSize = 0;

        [Range(1f, 10f)]
        public float smoothSpeed = 2f;  // 카메라 이동 속도 조절

        [Header("Boundary")]
        public GameObject boundarySprite; // 경계를 정의할 스프라이트 오브젝트

        [SerializeField, ReadOnly] private Vector2 minBoundary;
        [SerializeField, ReadOnly] private Vector2 maxBoundary;

        void Awake()
        {
            camTr = GetComponent<Transform>();
            CalculateBoundaries();

            if (targetTr == null)
            {
                Debug.Log("target이 없습니다.");
                targetTr = Managers.Game.Player.transform;
            }
            if (executableFunction == ExecutableFunction.FixedUpdate)
                StartCoroutine(CoFixedUpdate());
        }

        void LateUpdate()
        {
            if (executableFunction == ExecutableFunction.LateUpdate)
                FollowCamLogic();
        }


        public void CalculateBoundaries()
        {
            if (boundarySprite == null)
            {
                Debug.LogError("Boundary sprite is not assigned!");
                return;
            }

            // 스프라이트의 Renderer 컴포넌트로부터 경계를 계산
            Bounds bounds = boundarySprite.GetComponent<Renderer>().bounds;
            minBoundary = new Vector2(bounds.min.x, bounds.min.y);
            maxBoundary = new Vector2(bounds.max.x, bounds.max.y);

            // 카메라 뷰의 절반 크기를 계산
            float vertExtent = Camera.main.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            // 경계를 카메라 뷰에 맞게 조정
            minBoundary.x += horzExtent;
            minBoundary.y += vertExtent;
            maxBoundary.x -= horzExtent;
            maxBoundary.y -= vertExtent;
        }


        private void FollowCamLogic()
        {
            Vector3 targetPosition = targetTr.position
                                     + (-targetTr.forward * distance)
                                     + (Vector3.up * height);

            // Vector3.Lerp를 사용하여 카메라 위치를 부드럽게 이동
            camTr.position = Vector3.Lerp(camTr.position, targetPosition, smoothSpeed * Time.deltaTime);

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minBoundary.x, maxBoundary.x);
            pos.y = Mathf.Clamp(pos.y, minBoundary.y, maxBoundary.y);
            camTr.position = pos;

            if (_lastCamSize != camSize)
            {
                _lastCamSize = camSize;
                camTr.GetComponent<Camera>().orthographicSize = camSize;
            }
        }

        IEnumerator CoFixedUpdate()
        {
            while (true)
            {
                FollowCamLogic();
                yield return new WaitForFixedUpdate();
            }
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            // 경계를 그리기 전에 경계가 계산되었는지 확인하고, 아니면 계산합니다.
            if (boundarySprite != null && (maxBoundary == Vector2.zero && minBoundary == Vector2.zero))
            {
                CalculateBoundaries();
            }

            float vertExtent = Camera.main.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            // 경계를 그리는 데 사용할 코너 포인트를 계산
            Vector3 bottomLeft = new Vector3(minBoundary.x - horzExtent, minBoundary.y - vertExtent, 0);
            Vector3 topLeft = new Vector3(minBoundary.x - horzExtent, maxBoundary.y + vertExtent, 0);
            Vector3 bottomRight = new Vector3(maxBoundary.x + horzExtent, minBoundary.y - vertExtent, 0);
            Vector3 topRight = new Vector3(maxBoundary.x + horzExtent, maxBoundary.y + vertExtent, 0);

            // 사각형 경계를 그립니다.
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }
    }
}
