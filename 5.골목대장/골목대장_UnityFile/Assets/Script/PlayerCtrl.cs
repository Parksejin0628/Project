using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl: MonoBehaviour
{
	private int h; //Horizontal값을 받는 변수
	private int v; //Vertical값을 받는 변수
	private GameObject item;
	private GameObject DamageSkin; //데미지스킨 프리팹화
	private bool criticalBool = true;
	private Animator playerAm;

	public float speed = 5f; //플레이어의 이동속도
	public float retreatDistance = 4.0f; //전투시 몬스터와 플레이어가 떨어지는 거리
	public bool controllBool=true; //전투, 비전투 판단 변수
	public Transform playerTr; //플레이어의 좌표계(위치, 각도, 크기)를 받는값
	public GameObject camera; //카메라 오브젝트를 받는 변수
	public GameObject gameManager; //게임매니저를 받는 변수
	public GameObject targetMonster; //공격타겟이되는 오브젝트를 받는 변수
	public GameObject ChoiceZone; //선택창
	public GameObject DSkin; //데미지스킨을 받는 변수
	public GameObject attackE;
	public GameObject attackEffect; 
	public GameObject queText; //질문 텍스트
	public GameObject effText; //효과 텍스트
	public GameObject yesText;
	public GameObject inputD;
	public AudioClip attackSound; //공격효과음
	public AudioClip powerAttackSound; //치명타 효과음
	public AudioClip buySound; //구입효과음
	public AudioClip fallSound; //구입실패 효과음
	public AudioClip backgroundMusic; //배경음
	public AnimationClip idle;
	public AnimationClip right;
	public AnimationClip left;
	public AnimationClip up;
	public AnimationClip down;
	public AnimationClip attack;
	public int money = 5; //돈
	public int power = 0; //공격력
	public float attackSpeed = 1.0f;
	public bool attackBool = false; //공격성공 판단 변수
	public bool critical = false; //크리티컬


	private void Start()
	{
		//gameManager.GetComponent<GameManager>().SoundPlay(backgroundMusic, 1, 1);
		playerTr = GetComponent<Transform>(); //플레이어의 좌표계값 받기
		controllBool = true;
		ChoiceZone = GameObject.Find("ChoiceZone");
		ChoiceZone.SetActive(false);
		inputD.SetActive(false);
		playerAm = GetComponent<Animator>();
	}
	private void Update()
	{
		if (controllBool == true)
		{
			h = (int)Input.GetAxisRaw("Horizontal");
			v = (int)Input.GetAxisRaw("Vertical"); // 상하좌우방향키 입력값 받기
			if (v == 1)
			{
				playerAm.SetBool("Up", true);
				playerAm.SetBool("Right", false);
				playerAm.SetBool("Left", false);
				playerAm.SetBool("Down", false);
			}
			else if (v == -1)
			{
				playerAm.SetBool("Right", false);
				playerAm.SetBool("Left", false);
				playerAm.SetBool("Up", false);
				playerAm.SetBool("Down", true);
			}
			else if (h == 1)
			{
				playerAm.SetBool("Left", false);
				playerAm.SetBool("Up", false);
				playerAm.SetBool("Down", false);
				playerAm.SetBool("Right", true);
			}
			else if (h == -1)
			{
				playerAm.SetBool("Right", false);
				playerAm.SetBool("Up", false);
				playerAm.SetBool("Down", false);
				playerAm.SetBool("Left", true);
			}
			else if (h == 0 && v == 0)
			{
				playerAm.SetBool("Right", false);
				playerAm.SetBool("Left", false);
				playerAm.SetBool("Up", false);
				playerAm.SetBool("Down", false);
			}
			playerTr.Translate(Vector2.up * v * speed * Time.deltaTime, Space.Self);//상하이동
			playerTr.Translate(Vector2.right * h * speed * Time.deltaTime, Space.Self);//좌우이동
			camera.GetComponent<CameraCtrl>().CameraMove(); //CameraCtrl스크립트의 CameraMove함수를 발동시켜 카메라가 플레이어를 따라오게만든다.
		}
		else
		{
			playerAm.SetBool("Right", false);
			playerAm.SetBool("Left", false);
			playerAm.SetBool("Up", false);
			playerAm.SetBool("Down", false);
		}
	}
	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag("MONSTER")&&controllBool)//만약 충돌한오브젝트의 태그가 MONSTER일 경우
		{
			StartCoroutine(StartFight(coll.gameObject)); //StartFight라는 코루틴 함수를 발동
			targetMonster = coll.gameObject; //공격타겟 설정
		}
		else if(coll.gameObject==targetMonster&&!controllBool)
		{
			//Debug.Log("attack");
			gameManager.GetComponent<AudioSource>().volume = 1.0f;
			if (critical)
			{
				gameManager.GetComponent<GameManager>().SoundPlay(powerAttackSound, 0.8f, 1.2f);
				coll.gameObject.GetComponent<MonsterCtrl>().hp -= power*2;
			}
			else if (!critical)
			{
				gameManager.GetComponent<GameManager>().SoundPlay(attackSound, 0.8f, 1.2f);
				coll.gameObject.GetComponent<MonsterCtrl>().hp -= power;
			}
			criticalBool = false;
			inputD.SetActive(false);
			attackBool = true;
			attackEffect = (GameObject)Instantiate(attackE);
			attackEffect.transform.position = coll.transform.position;
			Destroy(attackEffect,0.1f);
			StartCoroutine(DamageDisPlay());
		}
		else if (coll.gameObject.CompareTag("ITEM")) //돈의양이 가격보다 클경우 선택창이뜬다.
		{
			item = coll.gameObject;
			controllBool = false;
			ChoiceZone.SetActive(true);
			yesText.GetComponent<Text>().text = "먹는다";
			if (coll.GetComponent<ItemCtrl>().Aspeed == 0)
			{
				queText.GetComponent<Text>().text = "소세지다, 먹어볼까?";
				effText.GetComponent<Text>().text = "("+item.GetComponent<ItemCtrl>().price+"원, 먹을 시 공격력 증가)";
			}
			else
			{
				queText.GetComponent<Text>().text = "시금치다, 먹어볼까?";
				effText.GetComponent<Text>().text = "("+ item.GetComponent<ItemCtrl>().price+"원, 먹을 시 공격속도 증가)";
			}
		}
	}
	private void OnTriggerStay2D(Collider2D coll)
	{
		/*if (coll.gameObject.CompareTag("ITEM") && coll.GetComponent<ItemCtrl>().price <= money && Input.GetKeyDown(KeyCode.Space)) //돈의양이 가격보다 클경우 && 접촉하면서 스페이스바를누를시 구입
		{
			money -= coll.GetComponent<ItemCtrl>().price; //가격만큼 돈의양이깍이고
			power += coll.GetComponent<ItemCtrl>().powerUp; //공격력상승략만큼 공격력이상승
			attackSpeed += coll.GetComponent<ItemCtrl>().Aspeed; //공격속도상승
			coll.gameObject.SetActive(false); //접촉한 아이템을 비활성화
			//Debug.Log(power);
		}*/
		if(coll.gameObject.CompareTag("TROPHY")&&Input.GetKeyDown(KeyCode.Space)) //전리품에 닿은채로 스페이스바를누르면
		{
			Destroy(coll.gameObject);//해당 전리품을 제거한다.
		}
	}
	public void Eatitem()
	{
		if(item.GetComponent<ItemCtrl>().price > money)
		{
			gameManager.GetComponent<AudioSource>().volume = 0.5f;
			gameManager.GetComponent<GameManager>().SoundPlay(fallSound, 0.5f, 0.5f);
			yesText.GetComponent<Text>().text = "돈이 부족하다";
			return;
		}
		gameManager.GetComponent<GameManager>().SoundPlay(buySound,1,1);
		money -= item.GetComponent<ItemCtrl>().price; //가격만큼 돈의양이깍이고
		power += item.GetComponent<ItemCtrl>().powerUp; //공격력상승략만큼 공격력이상승
		attackSpeed += item.GetComponent<ItemCtrl>().Aspeed; //공격속도상승
		item.GetComponent<ItemCtrl>().itemTurn++;
		//item.gameObject.SetActive(false); //접촉한 아이템을 비활성화
		ChoiceZone.SetActive(false);
		controllBool = true;
	}
	public void No()
	{
		ChoiceZone.SetActive(false);
		controllBool = true;
	}
	IEnumerator DamageDisPlay()
	{
		DamageSkin = Instantiate(DSkin);
		if (critical)
		{
			DamageSkin.GetComponent<GUIText>().text = "-" + (power * 2);
			DamageSkin.GetComponent<GUIText>().color = new Vector4(1, 0, 0, 1);
		}
		else if (!critical)
		{
			DamageSkin.GetComponent<GUIText>().text = "-" + power;
			DamageSkin.GetComponent<GUIText>().color = new Vector4(0, 0, 0, 1);
		}
		Vector3 DSPosition = Camera.main.WorldToViewportPoint(targetMonster.transform.position);
		DSPosition = DSPosition + (Vector3.down * (0.15f + Random.Range(-0.01f, 0.01f)))+(Vector3.right*(0.02f+Random.Range(0,0.02f)));
		DamageSkin.transform.position = DSPosition;
		for (float a = 1; a >= 0; a -= 0.02f)
		{
			
			//Debug.Log(DSPosition);
			//DamageSkin.GetComponent<GUIText>().color = new Vector4(0, 0, 0, a);
			DSPosition = DSPosition + (Vector3.up * 0.002f);
			DamageSkin.transform.position = DSPosition;
			yield return new WaitForFixedUpdate();
		}
	}
	IEnumerator StartFight(GameObject coll) //전투시작시 발동되는 코루틴 함수
	{
		int num = 0;
		controllBool = false;
		playerTr.position = coll.GetComponent<Transform>().position;
		camera.GetComponent<Transform>().position = new Vector3(((playerTr.position.x - retreatDistance) + coll.GetComponent<Transform>().position.x) / 2, playerTr.position.y, camera.GetComponent<Transform>().position.z);
		yield return new WaitForSeconds(0.05f);
		while (num < 20)
		{
			playerTr.Translate(Vector2.left * (retreatDistance / 20), Space.Self);
			num++;
			yield return new WaitForSeconds(0.01f);
		}
		yield return new WaitForSeconds(0.01f);
		Vector3 positionCenter = new Vector3((playerTr.position.x + coll.transform.position.x) / 2, playerTr.position.y, playerTr.position.z);
		//Debug.Log(positionCenter+"+"+playerTr.position+"+"+coll.transform.position);
		gameManager.GetComponent<GameManager>().BattleManager(coll); //제한시간을 해당몬스터의 제한시간으로 설정한 뒤 GameManager함수의 BattleTimeManager함수를 발동시킨다 
		StartCoroutine(Fighting(coll, positionCenter,playerTr.position,coll.transform.position));
		//controllBool = true;
	}
	IEnumerator Fighting(GameObject coll, Vector3 positionCenter, Vector3 playerPosOrg, Vector3 collPosOrg)
	{
		int a = 0;
		int b = 0;
		bool end = true;
		float attackMoveSpeed = 0.01f;
		while (!controllBool)
		{
			end = true;
			if (a > 5 - attackSpeed*2 && criticalBool)
			{
				inputD.SetActive(true);
			}
			if(Input.GetKeyDown(KeyCode.D)&&a>5-attackSpeed*2 && criticalBool)
			{
				critical = true;
				inputD.SetActive(false);
				criticalBool = false;
			}
			else if(Input.GetKeyDown(KeyCode.D)&&criticalBool)
			{
				criticalBool = false;
				inputD.SetActive(false);
			}
			if (attackBool==true)
			{
				if(playerTr.position.x > playerPosOrg.x || coll.transform.position.x < collPosOrg.x)
				{
		
					if (b == 0)
					{
						playerAm.SetTrigger("Attack");
						b = 1;
					}
					playerTr.Translate(Vector2.left * attackMoveSpeed, Space.Self);
					coll.transform.Translate(Vector2.right * attackMoveSpeed, Space.Self);
				}
				else if(playerTr.position.x <= playerPosOrg.x && coll.transform.position.x >= collPosOrg.x)
			 	{
					playerTr.position = playerPosOrg;
					coll.transform.position = collPosOrg;
					attackMoveSpeed = 0.01f;
					attackBool = false;
					a = 0;
					criticalBool = true;
					critical = false;
					b = 0;

					yield return new WaitForSeconds(1.01f-(attackSpeed/4));
					//Debug.Log(attackBool);
				}
			}
			else if (playerTr.position.x < positionCenter.x && coll.transform.position.x > positionCenter.x)
			{
				playerTr.Translate(Vector2.right * attackMoveSpeed, Space.Self);
				coll.transform.Translate(Vector2.left * attackMoveSpeed, Space.Self);
			}
			attackMoveSpeed += 0.01f+ (attackSpeed / 40);
			a++;
			yield return new WaitForSeconds(0.01f);
		}
		if(controllBool&&end)
		{
			coll.transform.position = collPosOrg;
			end = false;
		}
	}
}