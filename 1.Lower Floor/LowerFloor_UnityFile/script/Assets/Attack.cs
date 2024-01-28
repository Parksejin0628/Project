using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ArrayList myAL = new ArrayList();
        myAL.Add("hi");
        myAL.Add("welcome");
        myAL.Add("to");
        myAL.Add("c#");
        myAL.Add("world"); // hi, welcome, to, c#, world
        PrintArray(myAL);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PrintArray(ArrayList arr)
    {
        foreach (string item in arr)
        {
            Debug.Log(item);
        }
    }
}
