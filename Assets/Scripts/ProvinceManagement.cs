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

	/*Takes a building to move, and whether to place it in the active/inactive list.*/
	public void moveBuilding(Building building, bool isActive)
	{
		if (isActive)
		{
			inactiveBuildings.Remove(building);	//Remove referenced building from previous building list.
			activeBuildings.Add(building);	//Add referenced building to the new building list.
			building.trans.SetParent(UI.singleton.activeBuildingsPanel); //Set the referenced building's parent to the relevant active/inactive object.
		}
		else
		{
			activeBuildings.Remove(building);
			inactiveBuildings.Add(building);
			building.trans.SetParent(UI.singleton.inactiveBuildingsPanel);
		}

		repositionBuildings(inactiveBuildings);
		repositionBuildings(activeBuildings);
	}

	/*Takes a List<Building> and loops through it, setting the x and y position
	of each building in the list relative to its position in the list.*/
	private void repositionBuildings(List<Building> buildings)
	{
		for (int i = 0; i < buildings.Count; i++)
		{
			float yPos;
			float xPos = 115;

			switch(i)
			{
				case 0:
					yPos = 100;
					break;
				case 1:
					yPos = 0;
					break;
				case 2:
					yPos = -100;
					break;
				default:
					yPos = 100;
					break;
			}

			buildings[i].trans.position = new Vector2(xPos, yPos);
		}
	}
}

public class ProvinceManagement : MonoBehaviour 
{
	public List<Province> provinces;	//List of provinces belonging to this nation. Province objects.
	
	private Transform trans;	//This nations transform.
	bool isRaised = false;	//Is the active province currently raised.
	private Province activeProvince;	//The province that is currently active.

	/*TEMPORARY START METHOD, THIS IS ONLY IN PLACE WHILST STARTING A NETWORK AND THE GAME SCENE ARE THE SAME 
	TO AVOID TRYING TO SET DATA WHEN IT DOESN'T EXIST.*/
	public void NetworkStart()
	{
		trans = GameManager.singleton.thisNationTransform;
		initialiseBuildings();
		InitialiseProvinces();
	}

	/*The list of Building objects, by which the province objects will initialise their active and inactive lists. */
	private List<Building> defaultBuildings;

	/*Takes the building UI GameObject's and creates a new Building object for each of them
	whilst also assigning the created object's to the defaultBuildings list.*/
	void initialiseBuildings()
	{
		defaultBuildings = new List<Building>();

		for (int i = 0; i < UI.singleton.buildings.Length; i++)
		{
			defaultBuildings.Add(new Building(UI.singleton.buildings[i]));
		}
	}

	/*Takes the provinces list and for every child of this nation, create a new province 
	assigning that child transform as the transform of the province object. Also passing
	in the defaultBuildings list to every province object that is created.*/
	void InitialiseProvinces()
	{
		provinces = new List<Province>();

		for (int i = 0; i < trans.childCount; i++)
		{
			provinces.Add(new Province(trans.GetChild(i), defaultBuildings));
		}
	}

	void Update()
	{
		if (provinceIsClicked() && !UI.singleton.provinceUIActive)
		{
			UI.singleton.ActivateProvinceManagementUI(true);
			UI.singleton.LoadProvinceValues(activeProvince);
			RaiseProvince(true);
		}
		else if (!UI.singleton.provinceUIActive && !provinceIsClicked())
		{
			RaiseProvince(false);
		}
	}

	/*Raises the active province, should be called when a province is clicked and activated.*/
	///bool raise should be true or false for whether to raise the province or not respectively.
	void RaiseProvince(bool raise)
	{
		if (raise && !isRaised)
		{
			isRaised = true;
			activeProvince.trans.Translate(new Vector3(0, 0.3f, 0));
		}
		else if (!raise && isRaised)
		{
			isRaised = false;
			activeProvince.trans.Translate(new Vector3(0, -0.3f, 0));
		}
	}

	/*This function should be called every frame. It will check if the right mouse button is down and if so, fire
	a raycast from the mouse. If it hits, it will do some checking to see if it is a province, and if so, it will check
	which province, and return the relevant object as the active province for use elsewhere. It will also return true or false
	if a province is clicked.*/
	bool provinceIsClicked()
	{
		bool isClicked = false;

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);	//Sends raycast from mouse.
			if (hit.collider != null && hit.transform.parent != null)	//If it has hit something and what it has hit has a parent.
			{
				if (hit.transform.IsChildOf(trans))	//Is the object a child of this nation.
				{
					isClicked = true;
					for (int i = 0; i < provinces.Count; i++)
					{
						if (hit.transform.parent == provinces[i].trans)	//Checks which province the province point that was clicked is a child of.
						{
							activeProvince = provinces[i];	//Sets that province as active.
							break;
						}
					}
				}
			}
		}

		return isClicked;
	}
}
