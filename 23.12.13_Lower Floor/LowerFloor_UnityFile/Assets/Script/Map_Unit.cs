using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map_Unit : MonoBehaviour
{

    public int floor = -1;
    //ÁöÁ¤À¯´Ö 
    //public GameObject Unit5F;
    public GameObject UnitLobby;
    public GameObject Unit3F;
    public GameObject Unit2F;
    public GameObject Unit1F;
    public GameObject helper;

    public GameObject boss_sceen1;
    public GameObject boss_sceen2;

    // Start is called before the first frame update
    void Start()
    {
        //Unit5F.SetActive(false);
        //Unit4F.SetActive(false);
        Unit3F.SetActive(false);
        Unit2F.SetActive(false);
        Unit1F.SetActive(false);
        UnitLobby.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (floor) {
            case 3:
                Unit3F.SetActive(true);
                break;
            case 2:
                Unit2F.SetActive(true);
                break;
            case 1:
           
                Unit1F.SetActive(true);
                break;
            case 0:
                

                UnitLobby.SetActive(true);
                break;
            /*case 5:
                Unit5F.SetActive(true);
                break;
*/
        }
    }
    
    public void set_help()
    {
        helper.SetActive(true);
    }
    public void set_help_Not()
    {
        helper.SetActive(false);
    }


    public void delete_sceen()
    {
        if (Input.anyKeyDown)
        {
            helper.SetActive(false);
        }
    }
}
