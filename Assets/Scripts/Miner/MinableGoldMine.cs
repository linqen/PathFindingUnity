using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinableGoldMine : MonoBehaviour
{

	public int AmountOfGold;
	public float TimeToMine;

	public bool GetGold()
	{
		if (AmountOfGold - 1 >= 0)
		{
			AmountOfGold--;
			return true;
		}
		else
		{
			return false;
		}

	}
	
}
