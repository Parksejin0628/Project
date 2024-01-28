using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TransMainScene()
	{
		SceneManager.LoadScene("MainScene"); //MainScene으로 전환
	}

	void TransMain()
	{
		SceneManager.LoadScene("Main");
	}
}
