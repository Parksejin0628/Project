using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject ground;
    public GameObject player;
    Transform transform;
    Camera camera;
    Vector3 originPos;
    float X;
    float Y;
    public float cameraSpeed = 0.1f;
    float cameraHeight;
    float cameraWidth;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        camera = GetComponent<Camera>();
        cameraHeight = camera.orthographicSize;
        cameraWidth = cameraHeight * 16.0f / 9.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        originPos = Vector3.Lerp(transform.position, player.transform.position, cameraSpeed);
        X = Mathf.Clamp(originPos.x, ground.transform.position.x - ground.transform.localScale.x / 2 + cameraWidth, ground.transform.position.x + ground.transform.localScale.x / 2 - cameraWidth);
        Y = Mathf.Clamp(originPos.y, ground.transform.position.y - ground.transform.localScale.y / 2 + cameraHeight, ground.transform.position.y + ground.transform.localScale.y / 2 - cameraHeight);
        transform.position = new Vector3(X, Y, -10f);
    }
}
