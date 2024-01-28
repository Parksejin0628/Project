using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_final : MonoBehaviour
{
    public GameObject gameObject;
    public GameObject[] line = new GameObject[5];
    public Texture2D[] unlinkedLine = new Texture2D[5];
    public Texture2D[] linkedLine = new Texture2D[5];
    public bool[] is_link = new bool[5];
    public int[] answer = new int[5];
    private int index = 0;

    
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if (is_link[i])
            {
                //Debug.Log("check1");
                line[i].GetComponent<RawImage>().texture = linkedLine[i];
            }
            else
            {
                //Debug.Log("check2");
                line[i].GetComponent<RawImage>().texture = unlinkedLine[i];
            }
        }
        //Debug.Log("index" + index);
        if (index == 5)
        {
            gameObject.GetComponent<GameManager>().puzzleCount();
            index = 0;
            
        }
    }

    public void linkLine(int line)
    {
        if (answer[index++] == line)
        {
            is_link[line] = true;
        }
        else
        {
            for(int i=0; i<5; i++)
            {
                is_link[i] = false;
            }
            index = 0;
        }
    }
}
