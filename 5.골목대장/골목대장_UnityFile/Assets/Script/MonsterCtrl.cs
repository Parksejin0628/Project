using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour {
	public float speed=0.05f; //몬스터 속도
	public float limitTime = 2.0f; //제한시간
	public float responTime = 0.0f; //부활시간
	public int moveTime = 40; //몬스터 이동시간, 1당 0.01초
	public int hp = 0; //몬스터체력
	public int hpMax = 0; //몬스터의 원래체력
	public bool controllBool = true; //전투, 비전투 판단용 변수
	public GameObject trophy; //몬스터의 전리품
	public GameObject gameManager; //게임매니저 오브젝트를 받는 변수
	public GameObject winZone;
	public int getMoney = 50;
	



	private Transform monTr; //몬스터 Transform값을 받는 변수
	private GameObject player; //플레이어 게임오브젝트를 받는 변수
	private bool moveBool = true;
	private int MoveDir=0;
	private int MoveDir_=0;
	// Use this for initialization
	void Start () {
		winZone.SetActive(false);
		monTr = GetComponent<Transform>();
		StartCoroutine(AutoMove()); //AutoMove라는 코루틴함수 실행
		player = GameObject.FindGameObjectWithTag("PLAYER"); //PLAYER태그를 가진 오브젝트 탐색 후 변수에 저장
		gameManager = GameObject.Find("GameManager");
		//Debug.Log(player);
		hpMax = hp;
	}
	
	// Update is called once per frame
	void Update () {
		controllBool = player.GetComponent<PlayerCtrl>().controllBool; //controllBool의 true/false값은 PlayerCtrl스크립트의 controllBool값을 따른다.
		if(hp<=0) //만약 몬스터의 체력이 0이하가된다면
		{
			if(this.gameObject.name=="bear")
			{
				winZone.SetActive(true);
				player.GetComponent<PlayerCtrl>().controllBool = false;
			}
			//Instantiate(trophy,monTr.position,monTr.rotation); //미리설정한 전리품이 생성
			gameManager.GetComponent<GameManager>().EndFight();
			gameManager.GetComponent<GameManager>().MonsterDeath(this.gameObject);
			player.GetComponent<PlayerCtrl>().money += getMoney;
			hp = hpMax;
			this.gameObject.SetActive(false); //몬스터 비활성
		}
		else if(hp<hpMax&&controllBool)
		{
			hp = hpMax;
			gameManager.GetComponent<GameManager>().EndFight();
		}
	}

	public void StartAutoMove()
	{
		StartCoroutine(AutoMove());
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		//Debug.Log(coll+"hit");
		if(coll.gameObject.CompareTag("OBSTACLE")||coll.gameObject.CompareTag("PORTAL"))
		{
			moveBool = false;
			MoveDir_ = MoveDir;
			//Debug.Log(coll);
		}
		else if(coll.gameObject.CompareTag("DEATH"))
		{
			hp = 0;
		}
	}

	private void OnTriggerExit2D(Collider2D coll)
	{
		if(coll.gameObject.CompareTag("OBSTACLE"))
		{
			MoveDir_ = 0;
		}
	}

	IEnumerator AutoMove()
	{
		int num = 0;
		while (true)
		{
			MoveDir = Random.Range(1, 4 + 1);
			if (MoveDir_ == MoveDir)
			{
				while (MoveDir_ == MoveDir)
				{
					MoveDir = Random.Range(1, 4 + 1);
				}
			}
			//Debug.Log(MoveDir);
			num = 0;
			while (num < moveTime)
			{
				if (controllBool)
				{
					//Debug.Log(MoveDir);
					if (MoveDir == 1)
					{
						monTr.Translate(Vector2.right * speed, Space.Self);
					}
					else if (MoveDir == 2)
					{
						monTr.Translate(Vector2.left * speed, Space.Self);
					}
					else if (MoveDir == 3)
					{
						monTr.Translate(Vector2.up * speed, Space.Self);
					}
					else if (MoveDir == 4)
					{
						monTr.Translate(Vector2.down * speed, Space.Self);
					}
					num++;
				}
				else if (!controllBool)
				{
					break;
				}
				if (!moveBool)
				{
					while (MoveDir_ == MoveDir)
					{
						MoveDir = Random.Range(1, 4 + 1);
					}
					moveBool = true;
				}
				yield return new WaitForSeconds(0.01f);
			}
			
			yield return new WaitForSeconds(Random.Range(0.5f,3.0f)); //몬스터 대기시간(다음 움직임까지)렌덤지정
		}
	} //몬스터 자동움직임 코루틴 함수
}
