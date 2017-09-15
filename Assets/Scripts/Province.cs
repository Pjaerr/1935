using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
	public string name;
	public Transform trans;
	Transform provincePoint;

	public ValueContainer values = new ValueContainer(new string[] {"economy", "food", "iron", "coal", "happiness", "population"});
	public ValueContainer modifiers = new ValueContainer(new string[] {"economy", "food", "iron", "coal", "happiness", "population"});

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
		}
	}

	public void UpdateValues()
	{
		int len = values.getAll().Count;

		for (int i = 0; i < len; i++)
		{
			string valueToChange = values.getValueLookup()[i];

			values.set(valueToChange, modifiers.get(valueToChange));
		}
	}

	/*Takes a building to activate, and whether to place it in the active/inactive list.*/
	public void activateBuilding(Building building, bool isActive)
	{
		if (isActive)
		{
			if (building.onBuildingActivated(this))	//If the building has been purchased.
			{
				inactiveBuildings.Remove(building);	//Remove referenced building from previous building list.
				activeBuildings.Add(building);	//Add referenced building to the new building list.
			}
		}
		else
		{
			building.onBuildingDeactivated(this);	//Remove modifier effects.
			activeBuildings.Remove(building);
			inactiveBuildings.Add(building);
		}	
	}
}
