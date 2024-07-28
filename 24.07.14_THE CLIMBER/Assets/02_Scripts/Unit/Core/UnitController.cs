using UnityEngine;

namespace DefaultSetting
{
    public class UnitController : MonoBehaviour
    {
        public bool isAlive;
        private GameObject _dieCollider = null;
        protected GameObject DieCollider
        {
            get
            {
                if (_dieCollider == null)
                {
                    Transform tempTr = transform.Find("DieCollider");
                    if (tempTr != null)
                    {
                        _dieCollider = tempTr.gameObject;
                        _dieCollider.layer = (int)Define.Layer.DieCollider;
                    }
                    else
                    {
                        GameObject go = new GameObject();
                        go.name = "AutoAdd_DieCollider";
                        go.AddComponent<CircleCollider2D>();
                        go.transform.parent = transform;
                        go.layer = (int)Define.Layer.DieCollider;
                        _dieCollider = go;
                        Debug.Log($"{gameObject.name}_AutoAdd_DieCollider");
                    }
                }
                return _dieCollider;
            }
        }
        protected virtual void Reset() { }
        protected virtual void Awake()
        {
            isAlive = true;
            DieCollider.SetActive(false);
        }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }

        //TODO: Addforce를 UnitController로 통일시킬 수 있는 방법을 고민해보자
        //Default: 방향, 힘
        //Select: 초기화 여부 체크
        //public void AddforceUnit(Collision2D collision)

        public virtual void OnHitUnit(float enemyDamage, bool CheckInvincibility = true) { Debug.Log("충돌 설정X"); }
        public virtual void OnUnitDie()
        {
            isAlive = false;
            DieCollider.SetActive(true);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
