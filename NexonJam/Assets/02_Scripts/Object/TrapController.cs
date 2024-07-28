using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    public class TrapController : MonoBehaviour
    {
        private Collider2D _trapColl;
        public Collider2D TrapColl
        {
            get
            {
                if (_trapColl == null)
                {
                    _trapColl = GetComponent<Collider2D>();
                    if (_trapColl == null)
                    {
                        _trapColl = gameObject.AddComponent<BoxCollider2D>();
                        Debug.Log($"{gameObject.name}_AutoAdd_TrapColl");
                    }
                    _trapColl.isTrigger = true;
                }
                return _trapColl;
            }
        }

        [SerializeField, ReadOnly] private float _damage;
        [SerializeField] private float _forcePower;

        private void Reset()
        {
            gameObject.layer = (int)Define.Layer.Trap;
        }

        private void Awake()
        {
            _trapColl = TrapColl;
            _damage = 10;
            _forcePower = 100;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CollFunction(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            CollFunction(collision);
        }

        private void CollFunction(Collider2D collision)
        {
            if (collision.gameObject.layer == (int)Define.Layer.Player)
            {
                if (collision.transform.name.Contains("Boss"))
                    return;

                if (collision.transform.tag != Enum.GetName(typeof(Define.Tag), Define.Tag.Body))
                    return;

                UnitController controller = collision.gameObject.GetComponent<UnitController>();
                if (controller == null)
                    return;

                PlayerController playerController = controller as PlayerController;
                if (playerController != null)
                {
                    if (!playerController.State.IsInvincibility)
                        AddforceUnit(collision);
                }

                controller.OnHitUnit(_damage, true);
            }
        }

        //TODO : UnitController 등으로 통일시킬 수 있는 방법 고민하기
        public void AddforceUnit(Collider2D collision)
        {
            //법선벡터
            Rigidbody2D playerRig = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 inverseVector = playerRig.velocity.normalized * -1;

            playerRig.velocity = Vector2.zero;
            playerRig.AddForce(inverseVector * _forcePower);
        }
    }
}
