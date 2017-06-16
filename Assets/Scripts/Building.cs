using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building
{
	Province parentProvince;
	GameObject UI;	//The gameojbect that encapsulates the UI associated with this building.
	public Transform trans;
	public Button activateButton;

	/*0: Economy, 1: Food, 2: Iron, 3: Coal*/
	private int[] modifiers = new int[4] {0, 0, 0, 0};	//Amt by which province values are changed.
	private int[] cost = new int[4] {0, 5, 0, 0};

	public Building(GameObject setUI)
	{
		UI = setUI;
		trans = UI.GetComponent<Transform>();

		activateButton = UI.transform.GetChild(2).GetComponent<Button>();
	}

	public void setParentProvince(Province province)
	{
		parentProvince = province;
		activateButton.onClick.AddListener(delegate{parentProvince.activateBuilding(this, true);});
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

	public void onBuildingDeactivated()
	{
		for (int i = 0; i < modifiers.Length; i++)
		{
			parentProvince.modifiers[i] -= modifiers[i];	//Removes the modifiers given to the province.
		}
	}

	/*Called when the building is activated, will evaluate the cost and if it evaluates to true
	will adjust the province's modifiers and return true.*/
	public bool onBuildingActivated()
	{
		if (evaluateCost())	//If the province can afford this building.
		{
			adjustProvinceModifiers();	//Add this building's modifiers onto the province's modifiers.
			Debug.Log(this.trans.name + " activated on " + parentProvince.name);
			return true;
		}
		else	//If the province cannot afford this building.
		{
			Debug.Log("Insufficient resources to build " + this.trans.name + " on " + parentProvince.name);
			return false;
		}
	}

	/*Adds the modifiers from this building onto the given province's modifiers.*/
	private void adjustProvinceModifiers()
	{
		for (int i = 0; i < modifiers.Length; i++)
		{
			parentProvince.modifiers[i] += modifiers[i];
		}
	}

	private bool evaluateCost()
	{
		int isAffordable = 0;	//Will be incremented for every affordable value.

		for (int i = 0; i < cost.Length; i++)
		{
			/*For every province value that is more than the cost, increment isAffordable by 1. */
			if (cost[i] <= parentProvince.values[i])
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
				parentProvince.values[i] -= cost[i];
			}

			return true;
		}
		else
		{
			return false;
		}
	}
}
