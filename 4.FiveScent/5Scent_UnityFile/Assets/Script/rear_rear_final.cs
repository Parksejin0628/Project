using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rear_rear_final : MonoBehaviour
{
    public GameObject endingUI;
    public GameObject gameManager;
    public GameObject obj;
    public Texture2D[] texture = new Texture2D[6];
    public int[] answer = new int[5];
    private int index = 0;

    private void Awake()
    {
        endingUI.SetActive(false);
    }

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(index>=5)
        {
            gameManager.GetComponent<GameManager>().puzzleCount();
            endingUI.SetActive(true);
        }
    }

    public void sign(int num)
    {
        if (answer[index++] == num)
        {
            obj.GetComponent<RawImage>().texture = texture[num];
           
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                obj.GetComponent<RawImage>().texture = texture[0];
            }
            index = 0;
        }
    }
}
