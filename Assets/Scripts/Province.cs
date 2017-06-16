using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
	public string name;
	public Transform trans;
	Transform provincePoint;

	/*These are the data values. value[0] is the actual value and value[1] is the amount
	by which the first value will be changed every so often where:
	0 = Economy, 1 = Food, 2 = Iron, 3 = Coal, 4 = Happiness, 5 = Population*/
	public int[] values = new int[] {0, 0, 0, 0, 0, 0};
	public int[] modifiers = new int[] {0, 0, 0, 0, 0, 0};

	public List<Building> inactiveBuildings = new List<Building>();
	public List<Building> activeBuildings = new List<Building>();

	/*The object constructor. Takes a transform which will be assigned as this provinces
	transform, it will automatically grab the provinces province point if it exists*/
	public Province(Transform provinceTransform, List<Building> defaultBuildings)
	{
		trans = provinceTransform;

		if (provinceTransform.childCount > 0)
		{
			provincePoint = provinceTransform.GetChild(0);
		}

		name = trans.name;

		/*Grab the buildings from the ProvinceManagement::defaultBuildings list and assign
		them to this province's inactiveBuildings list.*/
		for (int i = 0; i < defaultBuildings.Count; i++)
		{
			inactiveBuildings.Add(defaultBuildings[i]);
			this.inactiveBuildings[i].setParentProvince(this);
		}
	}

	public void UpdateValues()
	{
		for (int i = 0; i < values.Length; i++)
		{
			values[i] += modifiers[i];
		}
	}

	/*Takes a building to activate, and whether to place it in the active/inactive list.*/
	public void activateBuilding(Building building, bool isActive)
	{
		if (isActive)
		{
			if (building.onBuildingActivated())	//If the building has been purchased.
			{
				inactiveBuildings.Remove(building);	//Remove referenced building from previous building list.
				activeBuildings.Add(building);	//Add referenced building to the new building list.
			}
		}
		else
		{
			building.onBuildingDeactivated();	//Remove modifier effects.
			activeBuildings.Remove(building);
			inactiveBuildings.Add(building);
		}	
	}
}
