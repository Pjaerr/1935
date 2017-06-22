using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProvinceManagement : NetworkBehaviour 
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
			defaultBuildings.Add(new Building(UI.singleton.buildings[i], (Building.BuildingType)i));

			if (defaultBuildings[i].buildingType == Building.BuildingType.Refinery)
			{
				defaultBuildings[i].setModifiers(new int[4] {0, 100, 100, 100});
			}
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

	
	/*Will change the values on the local UI to match that of the currently
	active province. This should be called when the province UI is activated.*/
	private void LoadProvinceValues()
	{
		UI.singleton.LoadProvinceValues(activeProvince);	//Passes in the currently active province.

		/*Positions both the active and inactive building lists for the currently active province*/
		UI.singleton.PositionBuildingUI(activeProvince.activeBuildings, true);
		UI.singleton.PositionBuildingUI(activeProvince.inactiveBuildings, false);
	}


	void Update()
	{
		if (!isLocalPlayer)
			return;

		if (provinceIsClicked())
		{
			UI.singleton.ActivateProvinceManagementUI(true);
			LoadProvinceValues();
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

		if (!UI.singleton.provinceUIActive)
		{
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
		}
		
		return isClicked;
	}
}
