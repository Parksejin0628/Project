using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public bool interationObject = false;
    public float zoomSize = 1.25f;
    Transform transform;
    Vector3 originScale;
    
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        originScale = transform.parent.GetComponent<Transform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player" && interationObject)
        {
            transform.parent.GetComponent<Transform>().localScale = originScale * zoomSize;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player"&& interationObject)
        {
            transform.parent.GetComponent<Transform>().localScale = originScale;
        }
    }
}
