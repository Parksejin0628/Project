using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    protected Rigidbody2D rig;
    protected WaitForFixedUpdate cashingWaitForFixedUpdate = new WaitForFixedUpdate();

    public bool isWalking { get { return moveDir != 0; } }
    public int moveDir;
    public float movePow = 30f;

    public virtual void MyAwake()
    {
        //Debug.Log("Not Override Not Move");
    }

    public virtual void MyFixedUpdate()
    {
        //Debug.Log("Not Override Not Move");
    }
}
