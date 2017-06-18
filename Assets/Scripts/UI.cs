using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This UI class should be used to manage the data and referencing associated with any UI
in the game that isn't a part of an instantiatiable object. It shouldn't carry out any 
actions that do not directly affect any UI elements.*/

public class UI : MonoBehaviour 
{
	/*Array of UI Text elements pertaining to the 'always on screen' UI.
	0 = Happiness
	1 = Economy
	2 = Food
	3 = Iron
	4 = Coal
	as assigned in the inspector.
	*/
	[SerializeField] private Text[] persistentUI;

	/*Array of UI Text elements pertaining to the province management UI.
	0 = Happiness
	1 = Economy
	2 = Food
	3 = Iron
	4 = Coal
	5 = Population
	6 = NationName
	*/
	[SerializeField] private Text[] provinceUI;
	[SerializeField] private GameObject provinceUIParent;	//The game object that holds the province management UI.
	[HideInInspector] public bool provinceUIActive = false;

	[SerializeField] Text nationText;	//The text that shows the name of the currently played nation.
	[SerializeField] Image nationFlag;	//The UI image that shows the flag of the currently played nation.

	[SerializeField] GameObject buildingUI;	//The gameobject that holds the building management UI.
	public GameObject[] buildings = new GameObject[3];	//The buildings. Barracks, Refinery & Fortress.
	public GameObject activeBuildingsPanel;
	public GameObject inactiveBuildingsPanel;


	int buildingsActivated = 0;
	Province lastOpenedProvince;

	/*activateBuilding() is called via the inspector onClick(), it is passed in a number
	to represent which button called the function. 0 upwards in order of building heirarchy position.
	These are set in the inspector.*/
	public void activateBuilding(int buildingNum)
	{
		if (activeProvince != lastOpenedProvince)
		{
			buildingsActivated = 0;
		}
		/*Calls the currently open province's activateBuilding function, activating the
		index of the building the button that was pressed resides on. It will increase the buildingsActivated
		integer whenever this function is called to account for buildings being removed from the inactiveBuildings
		List. So then if the building being asked to be activated is in a position other than the first, activate 
		that building minus the number of already activated buildings.*/

		if (activeProvince.inactiveBuildings.Count <= 1)
		{
			buildingNum = 0;
		}
		
		if (buildingNum > 0)
		{
			activeProvince.activateBuilding(activeProvince.inactiveBuildings[buildingNum - buildingsActivated], true);
		}
		else 
		{
			activeProvince.activateBuilding(activeProvince.inactiveBuildings[buildingNum], true);
		}

		PositionBuildingUI(activeProvince.inactiveBuildings, false);
		PositionBuildingUI(activeProvince.activeBuildings, true);
		
		buildingsActivated++;

		lastOpenedProvince = activeProvince;
	}

	/*Takes a list of buildings to position, and whether they are active or inactive buildings.
	It will then set the building's parents to the relevant UI panel, and position them according
	to their position in their Building List.*/
	public void PositionBuildingUI(List<Building> buildings, bool isActive)
	{
		/*Grabs the relevant UI panel depending upon whether the current building list
		holds active or inactive buildings.*/
		Transform activityPanel;

		if (isActive)
		{
			activityPanel = UI.singleton.activeBuildingsPanel.transform;
		}
		else
		{
			activityPanel = UI.singleton.inactiveBuildingsPanel.transform;
		}

		/*For every building in the passed in building list, set its xPos to default, and its yPos to
		the relevant position depending upon what position this building is in the building list.*/
		for (int i = 0; i < buildings.Count; i++)
		{
			buildings[i].trans.parent = activityPanel;
	
			float yPos;
			float xPos = 0;

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

			buildings[i].trans.localPosition = new Vector2(xPos, yPos);
		}
	}

	public static UI singleton = null;	//Singleton instance.

	void InitializeSingleton()
	{
		//Check if an instance of NationManager already exists.
		if (singleton == null)
		{
			//If not, make this that instance.
			singleton = this;
		}
		//If an instance already exists and it isn't this.
		else if (singleton != this)
		{
			//Destroy this.
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	void Awake()
	{
		InitializeSingleton();
	}

	void Start()
	{
		/*Avoids doing something that requires the GameManager values to actually work
		as before the NetworkManager has started the game, the GameManager will not have any
		values and will throw a NullReferenceException.This can be removed once the starting 
		of a game is seperated from the actual game scene itself.*/
		if (GameManager.networkIsConnected)
		{
			InitialisePersistentUI();
		}
	}

	void Update()
	{
		nationText.text = GameManager.singleton.thisNation.ToString();
	}
	
	/*Sets all default values for the persistent UI.*/
	void InitialisePersistentUI()
	{
		nationText.text = GameManager.singleton.thisNation.ToString();
		for (int i = 0; i < persistentUI.Length; i++)
		{
			UpdateValue(i, GameManager.singleton.nationValues[i], "persistentUI");
		}
		
	}

	/*Updates the specified values on the specified UI elements with the given values.
	Will also give a sign automatically depending upon whether the values updated value
	is positive or negative. This function does not increment on a value, it only replaces it.*/
	void UpdateValue(int valueToUpdate, float val, string type)
	{
		string newValue;

		if (val > 0)
		{
			newValue = "+" + val.ToString();
		}
		else
		{
			newValue = "-" + val.ToString();
		}

		if (type == "persistentUI")
		{
			persistentUI[valueToUpdate].text = newValue;
		}
		else if (type == "provinceManagementUI")
		{
			provinceUI[valueToUpdate].text = newValue;
		}
	}

	private Province activeProvince;
	/*This function should update the provinceManagementUI values as per the province that
	has been activated using the UpdateValues() function. */
	public void LoadProvinceValues(Province province)
	{
		activeProvince = province;
		UpdateValue(0, activeProvince.values[4], "provinceManagementUI");
		UpdateValue(1, activeProvince.values[0], "provinceManagementUI");
		UpdateValue(2, activeProvince.values[1], "provinceManagementUI");
		UpdateValue(3, activeProvince.values[2], "provinceManagementUI");
		UpdateValue(4, activeProvince.values[3], "provinceManagementUI");
		UpdateValue(5, activeProvince.values[5], "provinceManagementUI");
		provinceUI[6].text = activeProvince.name;
	}

	/*Either Activates or Deactivates the province management UI depending upon the
	boolean passed in. If chosen to activate, it will call the LoadProvinceValues() function.*/
	public void ActivateProvinceManagementUI(bool active)
	{
		provinceUIParent.SetActive(active);
		if (active)
		{
			provinceUIActive = true;
		}
		else
		{
			provinceUIActive = false;
		}
	}

	public void ActivateBuildingManagementUI(bool active)
	{
		buildingUI.SetActive(active);
	}
}
