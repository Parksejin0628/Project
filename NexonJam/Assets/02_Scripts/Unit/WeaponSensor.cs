using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSensor : MonoBehaviour
{
    public LayerMask checkLayerMask;
    [HideInInspector]
    public bool checkLayerResult = false;

    protected virtual void Awake()
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        CheckLayer(collision);
        return;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        CheckLayer(collision);
        return;
    }

    private void CheckLayer(Collider2D collision)
    {
        //선택한 레이어에 맞는 오브젝트가 부딧쳤는지 확인
        if ((checkLayerMask & (1 << collision.gameObject.layer)) != 0)
            checkLayerResult = true;
        else
            checkLayerResult = false;
    }

    protected virtual void OnHitEffect(Collider2D collision) { }
}
