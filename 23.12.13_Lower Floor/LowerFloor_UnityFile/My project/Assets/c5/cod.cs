using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeMovement : MonoBehaviour
{
    float speed = 0;
    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            float swipeLength = endPos.x - this.startPos.x;

            // Calculate the speed based on the swipe length
            if (swipeLength >= 0)
            {
                this.speed = swipeLength / 500f;
                GetComponent<AudioSource>().Play();
            }// Adjust the multiplier as needed
            
        }

        // Move the object
        transform.Translate(this.speed, 0, 0);

        // Apply a deceleration factor
        this.speed *= 0.98f;
    }
}
