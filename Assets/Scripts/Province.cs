using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
	public string name;
	public Transform trans;
	Transform provincePoint;

	/*These are the data values. value[0] is the actual value and value[1] is the amount
	by which the first value will be changed every so often*/
	public int[] happiness = new int[2] {0, 0};
	public int[] economy = new int[2] {0, 0};
	public int[] food = new int[2] {0, 0};
	public int[] iron = new int[2] {0, 0};
	public int[] coal = new int[2] {0, 0};
	public int[] population = new int[2] {0, 0};

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

		/*Temporary random building activation for testing*/
		for (int i = inactiveBuildings.Count - 1; i > -1; i--)
		{
			int rand = (int)Random.Range(0, 5);
			if (rand == 2)
			{
				activateBuilding(inactiveBuildings[i], true);
			}
		}
	}

	public void UpdateValues()
	{
		happiness[0] += happiness[1];
		economy[0] += economy[1];
		food[0] += food[1];
		iron[0] += iron[1];
		coal[0] += coal[1];
		population[0] += population[1];
	}

	/*Takes a building to activate, and whether to place it in the active/inactive list.*/
	public void activateBuilding(Building building, bool isActive)
	{
		Debug.Log("Activating " + building.trans.name + " on " + this.name);

		if (isActive)
		{
			inactiveBuildings.Remove(building);	//Remove referenced building from previous building list.
			activeBuildings.Add(building);	//Add referenced building to the new building list.
		}
		else
		{
			activeBuildings.Remove(building);
			inactiveBuildings.Add(building);
		}	
	}
}
