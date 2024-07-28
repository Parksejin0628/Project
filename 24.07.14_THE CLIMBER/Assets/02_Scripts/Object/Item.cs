using DefaultSetting;
using DefaultSetting.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ItemName
{
    None,
    __Start__, 
    EmergencyFood, 
    BottleWater, 
    Bandage,
    Brick,
    WaterMelon,
    Hardtack, 
    Painkillers, 
    MysteriousPill,
    __Chapter1__, RawFish, SeaWater, ShellFish, StarFish, EngineOil, MysteriousBox, OrangePill,
    __Chapter2__, Apple, RiverWater, OrangeMushroom, Rock, SparklingWater, BluePill,
    __Chapter3__, StagnantWater, AgedCheese, RedPill, Balance, BrokenBrick
}
public class Item : MonoBehaviour
{
    public ItemName name;
    public float hpChangeAmount;
    public float hungerChangeAmount;
    public float thirstChangeAmount;
    public GameObject spawnObject;
    public Vector2 spawnSize;
    public Vector3 teleportPosition;
    public Image itemImage;

    public GameObject CanGetUI;
    public GameObject SpawnedUI;
    public float UIPos;

    private void Start()
    {
        SpawnedUI = Instantiate(CanGetUI);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.layer == Define.Layer.Player.ToInt())
        {
            Debug.Log("아이템 충돌 당햇다!");
            SpawnedUI.SetActive(true);
            SpawnedUI.transform.position = transform.position + Vector3.up * 0.25f;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Define.Layer.Player.ToInt())
        {
            SpawnedUI.SetActive(false);
        }
    }


}
