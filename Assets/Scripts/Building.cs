using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building
{
	private GameObject UI;	//The gameojbect that encapsulates the UI associated with this building.
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
	public void setCost(int[] valuesToSetBy)
	{
		cost = valuesToSetBy;
	}

	public void onBuildingDeactivated(Province province)
	{
		for (int i = 0; i < modifiers.Length; i++)
		{
			province.modifiers[i] -= modifiers[i];	//Removes the modifiers given to the province.
		}
	}

	/*Called when the building is activated, will evaluate the cost and if it evaluates to true
	will adjust the province's modifiers and return true.*/
	public bool onBuildingActivated(Province province)
	{
		if (evaluateCost(province))	//If the province can afford this building.
		{
			adjustProvinceModifiers(province);	//Add this building's modifiers onto the province's modifiers.
			Debug.Log(this.trans.name + " activated on " + province.name);
			return true;
		}
		else	//If the province cannot afford this building.
		{
			Debug.Log("Insufficient resources to build " + this.trans.name + " on " + province.name);
			return false;
		}
	}

	/*Adds the modifiers from this building onto the given province's modifiers.*/
	private void adjustProvinceModifiers(Province province)
	{
		for (int i = 0; i < modifiers.Length; i++)
		{
			province.modifiers[i] += modifiers[i];
		}
	}

	private bool evaluateCost(Province province)
	{
		int isAffordable = 0;	//Will be incremented for every affordable value.

		for (int i = 0; i < cost.Length; i++)
		{
			/*For every province value that is more than the cost, increment isAffordable by 1. */
			if (cost[i] <= province.values[i])
			{
				isAffordable++;
			}
		}

		/*If isAffordable has been incremented for every cost check, this means
		that the building is affordable.*/
		if (isAffordable == cost.Length)
		{
			/*When it is affordable, deduct the costs from the province's values
			and set the return boolean to true to indicate the building has been 'purchased'*/
			for (int i = 0; i < cost.Length; i++)
			{
				province.values[i] -= cost[i];
			}

			return true;
		}
		else
		{
			return false;
		}
	}
}
