using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
	public GameObject exit; //출구 오브젝트를 받는 변수

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag("PLAYER") && coll.GetComponent<PlayerCtrl>().controllBool)
		{       //Debug.Log(coll);
			if (this.name == "DoorExit" && coll.gameObject.CompareTag("PLAYER"))
			{
				GameObject.Find("Main Camera").GetComponent<CameraCtrl>().cameraBool = true;
				coll.GetComponent<Transform>().position = exit.GetComponent<Transform>().position;
			}
			if (this.name == "DoorEnter" && coll.gameObject.CompareTag("PLAYER"))
			{
				GameObject.Find("Main Camera").GetComponent<CameraCtrl>().cameraBool = false;
				coll.GetComponent<Transform>().position = exit.GetComponent<Transform>().position;
			}
			else if (coll.gameObject.CompareTag("PLAYER")) //부딪힌 대상의 태그가 PLAYER일경우
			{
				coll.GetComponent<Transform>().position = exit.GetComponent<Transform>().position;
				//부딪힌 대상의 위치를 출구의 위치로 바꾼다.
			}
		}
		
	}
}
