using System.Collections;
using UnityEngine;

namespace DefaultSetting
{
    public class FollowCam3D : MonoBehaviour
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

        public float damping = 1.0f;

        public float targetOffset = 2.0f;

        private Vector3 velocity = Vector3.zero;

        void Start()
        {
            camTr = GetComponent<Transform>();

            if (targetTr == null)
            {
                Debug.Log("target이 없습니다.");

                //targetTr = Managers.Game.Player.transform;
            }

            if (executableFunction == ExecutableFunction.FixedUpdate)
                StartCoroutine(CoFixedUpdate());
        }

        void LateUpdate()
        {
            if (executableFunction == ExecutableFunction.LateUpdate)
                followCamLogic();
        }

        void followCamLogic()
        {
            Vector3 pos = targetTr.position
                          + (-targetTr.forward * distance)
                          + (Vector3.up * height);

            camTr.position = Vector3.SmoothDamp(camTr.position, // 시작 위치
                                                pos,            // 목표 위치
                                                ref velocity,   // 현재 속도
                                                damping);       // 목표 위치까지 도달할 시간


            camTr.LookAt(targetTr.position + (targetTr.up * targetOffset));
            //camTr.LookAt(targetTr.position);
        }

        IEnumerator CoFixedUpdate()
        {
            while (true)
            {
                followCamLogic();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}