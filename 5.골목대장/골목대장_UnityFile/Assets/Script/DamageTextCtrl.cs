using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextCtrl : MonoBehaviour {
	public float displayTime=0.5f;
	public bool critical;
	// Use this for initialization
	void Start () {
		StartCoroutine(DisplayDamage());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DisplayDamage()
	{
		critical = GameObject.Find("Player").GetComponent<PlayerCtrl>().critical;
		yield return new WaitForSeconds(displayTime);
		if (!critical)
		{
			for (float a = 1; a >= -1; a -= 0.05f)
			{
				GetComponent<GUIText>().color = new Vector4(0, 0, 0, a);
				yield return new WaitForFixedUpdate();
			}
		}
		else if (critical)
		{
			for (float a = 1; a >= -1; a -= 0.05f)
			{
				GetComponent<GUIText>().color = new Vector4(1, 0, 0, a);
				yield return new WaitForFixedUpdate();
			}
		}
		Destroy(this.gameObject);
	}
}
