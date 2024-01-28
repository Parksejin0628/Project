using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {
	private Transform playerTr; //플레이어의 좌표계(위치, 각도, 크기)값을 받는 변수
	private Transform cameraTr; //카메라의 좌표계값을 받는 변수
	public GameObject player; //플레이어 오브젝트를 받는 변수
	public bool cameraBool=false;
	// Use this for initialization
	void Start () {
		playerTr = player.GetComponent<Transform>(); //플레이어의 좌표계값 받기
		cameraTr = GetComponent<Transform>(); //카메라의 좌표계값 받기
	}

	private void Update()
	{
		if(!cameraBool)
		{
			cameraTr.position = new Vector3(-49.5f, -5.6f, -8.5f);
		}
	}

	// Update is called once per frame
	public void CameraMove () {
		if (cameraBool)
		{
			cameraTr.position = new Vector3(playerTr.position.x, playerTr.position.y, -8.5f);
		}
		//카메라의 위치를 플레이어의 위치(playerTr.postion.x/y)와 일치시킨다.
	}
}
