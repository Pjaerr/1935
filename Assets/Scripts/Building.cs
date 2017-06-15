using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
	GameObject UI;	//The gameojbect that encapsulates the UI associated with this building.
	public Transform trans;

	/*0: Economy, 1: Food, 2: Iron, 3: Coal*/
	private int[] modifiers = new int[4] {0, 0, 0, 0};	//Amt by which province values are changed.
	private int[] cost = new int[4] {0, 0, 0, 0};

	public Building(GameObject setUI)
	{
		UI = setUI;
		trans = UI.GetComponent<Transform>();
	}

	///0: Economy, 1: Food, 2: Iron, 3: Coal
	public void setModifiers(int[] valuesToSetBy)
	{
		modifiers = valuesToSetBy;
	}
	///0: Economy, 1: Food, 2: Iron, 3: Coal
	public int[] getModifiers()
	{
		return modifiers;
	}
	///0: Economy, 1: Food, 2: Iron, 3: Coal
	public void setCost(int[] valuesToSetBy)
	{
		cost = valuesToSetBy;
	}
	///0: Economy, 1: Food, 2: Iron, 3: Coal
	public int[] getCost()
	{
		return cost;
	}
}
