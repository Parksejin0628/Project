using System.Collections;
using UnityEngine;

//출처 : https://www.youtube.com/watch?v=5E5_Fquw7BM&ab_channel=Brackeys
namespace DefaultSetting
{
    public class Parallaxing : MonoBehaviour
    {
        public Transform[] backgroundGroup;
        public int backgroundOrder = -1000;
        private float[] parallaxScales;
        public float smoothing = 1f;

        private Transform cam;
        private Vector3 previousCamPos;

        private void Reset()
        {
            int idx = 0;
            backgroundGroup = new Transform[transform.childCount];
            foreach (Transform item in transform)
            {
                backgroundGroup[idx++] = item;
            }

            SpriteRenderer[] srArr = GetComponentsInChildren<SpriteRenderer>();
            float currentX = srArr[0].transform.position.x;
            float standardY = srArr[0].transform.position.y;
            float standardZ = srArr[0].transform.position.z;

            //리셋 시 크기에 맞게 위치 지정
            foreach (var item in srArr)
            {
                item.sortingOrder = backgroundOrder;
                item.transform.position = new Vector3(currentX, standardY, standardZ);
                currentX += item.bounds.size.x;
            }
        }

        void Awake()
        {
            cam = Camera.main.transform;
        }

        // Use this for initialization
        void Start()
        {
            // The previous frame had the current frame's camera position
            previousCamPos = cam.position;

            // asigning coresponding parallaxScales
            parallaxScales = new float[backgroundGroup.Length];
            for (int i = 0; i < backgroundGroup.Length; i++)
            {
                parallaxScales[i] = backgroundGroup[i].position.z * -1;
            }

            StartCoroutine(CoParallaxingBackground());
        }

        GameObject go;

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator CoParallaxingBackground()
        {
            while (true)
            {
                // for each background
                for (int i = 0; i < backgroundGroup.Length; i++)
                {
                    // the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
                    float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScales[i];
                    float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScales[i];

                    // set a target x position which is the current position plus the parallax
                    float backgroundTargetPosX = backgroundGroup[i].position.x + parallaxX;
                    float backgroundTargetPosY = backgroundGroup[i].position.y + parallaxY;

                    // create a target position which is the background's current position with it's target x position
                    Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgroundGroup[i].position.z);

                    // fade between current position and the target position using lerp
                    backgroundGroup[i].position = Vector3.Lerp(backgroundGroup[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
                }

                // set the previousCamPos to the camera's position at the end of the frame
                previousCamPos = cam.position;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
