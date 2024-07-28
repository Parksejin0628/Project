using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour {
	public int price = 0; //가격
	public int powerUp = 2; //데미지상승량
    public float Aspeed = 0; //공격속도
	public int itemTurn = 1;
	public void SpecialAblity() //아이템의 특수능력
	{
		
	}

	private void Update()
	{
		if(this.gameObject.name == "DamageUp")
		{
			switch(itemTurn)
			{
				case 1:
					price = 50;
					break;
				case 2:
					price = 100;
					break;
				case 3:
					price = 180;
					break;
				case 4:
					price = 240;
					break;
				case 5:
					price = 400;
					break;
				case 6:
					price = 700;
					break;
				case 7:
					price = 1000;
					break;
				case 8:
					price = 9999;
					break;
			}
		}
		else if(this.gameObject.name == "AttackSpeedUp")
		{
			switch(itemTurn)
			{
				case 1:
					price = 400;
					break;
				case 2:
					price = 700;
					break;
				case 3:
					price = 1800;
					break;
				case 4:
					price = 9999;
					break;
			}
		}
	}
}
