using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectMoveType
{
    None, Circle, Between
}
public class MoveObject : MonoBehaviour
{
    public ObjectMoveType objectMoveType;
    public float moveSpeed = 10;

    [Header("원 운동 변수")]
    public float circleRadius = 5;
    public float StartDeg = 0.0f;
    private float deg = 0;
    private Vector3 Center;

    [Header("왕복 운동 변수")]
    public GameObject movingTargetA;
    public GameObject movingTargetB;
    public float stayTime;
    public float arrivalJudgeDistance = 0.1f;
    private Vector3 TargetAPos;
    private Vector3 TargetBPos;
    private Vector3 nowTarget;
    private float nowStayTime;
    private int targetIndex = 0;


    void Start()
    {
        if (objectMoveType == ObjectMoveType.Circle)
        {
            Center = transform.position;
            deg = StartDeg;
        }
        
        if(objectMoveType == ObjectMoveType.Between)
        {
            TargetAPos = movingTargetA.transform.position;
            TargetBPos = movingTargetB.transform.position;
            nowTarget = TargetAPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(objectMoveType)
        {
            case ObjectMoveType.Circle:
                MoveCircle();
                break;
            case ObjectMoveType.Between:
                MoveBetween();
                break;
        }
    }

    public void MoveCircle()
    {
        deg += Time.deltaTime * moveSpeed;
        deg %= 360;

        float x = Center.x + circleRadius * Mathf.Cos(deg * Mathf.Deg2Rad);
        float y = Center.y + circleRadius * Mathf.Sin(deg * Mathf.Deg2Rad);

        transform.position = new Vector3(x, y, transform.position.z);
    }
    
    public void MoveBetween()
    {
        if(nowStayTime >= 0)
        {
            nowStayTime -= Time.deltaTime;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, nowTarget, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(nowTarget, transform.position) <= arrivalJudgeDistance)
        {
            nowStayTime = stayTime;
            targetIndex++;
            if(targetIndex%2 == 0)
            {
                nowTarget = TargetAPos;
            }
            else if(targetIndex%2 == 1)
            {
                nowTarget = TargetBPos;
            }
        }


    }
}
