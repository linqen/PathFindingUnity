using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{

	public MinableGoldMine[] Mines;

	public MinableGoldMine GetMineWithGold()
	{
		for (int i = 0; i < Mines.Length; i++)
		{
			if (Mines[i].AmountOfGold > 0)
			{
				return Mines[i];
			}
		}

		//null if there is no Mine with gold
		return null;
	}
	
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
