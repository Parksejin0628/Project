using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private float _hp;
    [SerializeField]
    public float Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = Mathf.Clamp(value, 0, maxhp);
            if(_hp == 0)
            {
                OnUnitDie();
            }
        }
    }
    [SerializeField]
    [ReadOnly]
    private float _hunger;
    [SerializeField]
    public float Hunger
    {
        get
        {
            return _hunger;
        }
        set
        {
            _hunger = Mathf.Clamp(value, 0, maxhunger);
            /*if (_hunger == 0)
            {
                OnUnitStarve();
            }*/
        }
    }
    [SerializeField]
    [ReadOnly]
    private float _thirst;
    [SerializeField]
    public float Thirst
    {
        get
        {
            return _thirst;
        }
        set
        {
            _thirst = Mathf.Clamp(value, 0, maxthirst);
            /*if (_thirst == 0)
            {
                OnUnitDehydrate();
            }*/
        }
    }  
    [SerializeField]
    public float maxhp;
    [SerializeField]
    public float maxhunger;
    [SerializeField]
    public float maxthirst;
    [SerializeField]
    public float damage;
    [SerializeField]
    public float weight; //무게 | 무게에 따른 반동량 변동


    public void MyAwake()
    {
        SetUnitState();
    }

    protected virtual void SetUnitState()
    {
        _hp = maxhp;
        _hunger = maxhunger;
        _thirst = maxthirst;
    }

    protected virtual void OnUnitDie()
    {
    }
    protected virtual void OnUnitStarve()
    {
    }
    protected virtual void OnUnitDehydrate()
    {
    }
}
