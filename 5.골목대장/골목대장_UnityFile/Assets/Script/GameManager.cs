using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public GameObject timeText; //시간을 알려주는 Text를 저장하는 변수
	public GameObject moneyText; //돈의양을 알려주는 Text를 저장하는 변수
	public GameObject hpZone;
	public GameObject hpText;
	public GameObject hpBar;
	public AudioSource soundSource;
	public AudioSource backgroundMusic;
	public bool FightingBool = false; //싸움을 판단하는 변수

	public GameObject player; //플레이어 게임오브젝트를 저장하는 변수
	private GameObject target; //전투대상
	private GameObject[] monsters; //몬스터들
	private Vector3[] monstersPosition; //몬스터의 위치들
	private float[] monstersResponTime; //몬스터의 리스폰시간들
	private int monstersCount; //몬스터의 수
	private int money; //돈
	private int hp;
	private int hpMax;
	private bool[] monstersSurvive; //몬스터 생존판단
	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("PLAYER"); //PLAYER태그를 가진 오브젝트 탐색 후 저장
		hpZone.SetActive(false);
		monstersCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;
		monsters = GameObject.FindGameObjectsWithTag("MONSTER");
		monstersPosition = new Vector3[monstersCount];
		monstersResponTime = new float[monstersCount];
		monstersSurvive = new bool[monstersCount];
		//Debug.Log(monstersCount);
		for(int a=0; a<monstersCount; a++)
		{
			monstersPosition[a] = monsters[a].transform.position;
			monstersResponTime[a] = monsters[a].GetComponent<MonsterCtrl>().responTime;
			monstersSurvive[a] = true;
			//Debug.Log(monstersPosition[a] + "+" + monstersResponTime[a]);
		}

	}

	private void Update()
	{
		money = player.GetComponent<PlayerCtrl>().money; //플레이어의 money값을 불러옴
		moneyText.GetComponent<Text>().text = "Money : " + money; //money양을 출력
		//Debug.Log(FightingBool);
		HpManager();
		MonsterResponManager();
	}

	public void BattleManager(GameObject coll)
	{
		hpZone.SetActive(true);
		target = coll;
		hpMax = coll.GetComponent<MonsterCtrl>().hpMax;
		StartCoroutine(TimeManage(coll.GetComponent<MonsterCtrl>().limitTime)); // TimeManager라는 코루틴함수를 발동 이때 시간은 다른 스크립트에서 받은 시간
		FightingBool = true;
	}
	public void EndFight()
	{
		hpZone.SetActive(false);
		FightingBool = false;
	}
	public void HpManager()
	{
		if(!FightingBool)
		{
			return;
		}
		else
		{
			hp = target.GetComponent<MonsterCtrl>().hp;
			hpText.GetComponent<Text>().text = hp + " / " + hpMax;
			hpBar.GetComponent<Image>().fillAmount = ((float)hp / hpMax);
		}
	}
	public void MonsterDeath(GameObject monster)
	{
		for(int a = 0 ; a<monstersCount; a++)
		{
			if(monster==monsters[a])
			{
				monstersSurvive[a] = false;
			}
		}
	}
	public void MonsterResponManager()
	{
		for (int a = 0; a < monstersCount; a++)
		{
			if(monstersSurvive[a]==false)
			{
				monstersResponTime[a] -= 1.0f * Time.deltaTime;
			}
			if(monstersSurvive[a]==false&&monstersResponTime[a]<=0)
			{
				//Debug.Log(monsters[a] + "부활");
				monstersSurvive[a] = true;
				monsters[a].SetActive(true);
				monsters[a].transform.position = monstersPosition[a];
				monstersResponTime[a] = monsters[a].GetComponent<MonsterCtrl>().responTime;
				monsters[a].GetComponent<MonsterCtrl>().StartAutoMove();
			}
		}
	}
	public void SoundPlay(AudioClip sound,float highPitch, float lowPitch)
	{
		soundSource.clip = sound;
		soundSource.pitch = Random.Range(lowPitch, highPitch);
		soundSource.Play();
	}
	IEnumerator TimeManage(float time) //시간 감소 및 시간표시, 전투종료시 플레이어에게 제어권을 다시 주는 코루틴 함수
	{
		while(time>0)
		{
			time -= 1*Time.deltaTime;
			timeText.GetComponent<Text>().text = "Time : " + Mathf.Round(time*100)*0.01;
			yield return new WaitForSeconds(0.01f*Time.deltaTime);
			if(!FightingBool)
			{
				break;
			}

		}
		time = 0f;
		timeText.GetComponent<Text>().text = "Time : " + time;
		player.GetComponent<PlayerCtrl>().controllBool = true;
	}

}
