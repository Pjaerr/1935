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
	0 = Happiness, 1 = Economy, 2 = Food, 3 = Iron, 4 = Coal 
	as assigned in the inspector.*/
	[SerializeField] private Text[] persistentUI;
	public Text nationText;	//The text that shows the name of the currently played nation.
	[SerializeField] Image nationFlag;	//The UI image that shows the flag of the currently played nation.

	/*Province System UI*/

	/*Array of UI Text elements pertaining to the province management UI.*/
	[Tooltip("0 = Economy, 1 = Food, 2 = Iron, 3 = Coal, 4 = Happiness, 5 = Population, 6 = Province Name")]
	[SerializeField] private Text[] provinceUI;

	[SerializeField] private GameObject provinceUIParent;	//The game object that holds the province management UI.
	[HideInInspector] public bool provinceUIActive = false;	//Whether the province UI is open.
	private Province activeProvince;	//Currently active province.

	/*Building System UI*/
	[SerializeField] GameObject buildingUI;	//The gameobject that holds the building management UI.
	public GameObject[] buildings = new GameObject[3];	//The buildings. Barracks, Refinery & Fortress.
	public GameObject activeBuildingsPanel;
	public GameObject inactiveBuildingsPanel;


	private DataManager dataManager;

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

	public void NetworkStart()
	{
		dataManager = GameManager.singleton.localClientObj.GetComponent<DataManager>();
		InitialisePersistentUI();
	}
	
	/*Sets all default values for the persistent UI.*/
	void InitialisePersistentUI()
	{
		nationText.text = dataManager.getThisNation().ToString();
		
		for (int i = 0; i < persistentUI.Length; i++)
		{
			UpdateValue(dataManager.getNationValues()[i], persistentUI[i]);
		}
		
	}

	/*Takes the index of a Building.BuildingType enum. Converts the index to the corresponding enum value.
	Will then loop through the inactiveBuildings list, and check if the enum argument matches each building,
	when it finds a match, it will call the activeProvince's activateBuilding() function, passing in the building
	that the enum that matched, belongs to.*/
	public void activateBuilding(int index)
	{
		/*Converts index to the corresponding BuildingType enum.*/
		Building.BuildingType buildingType = (Building.BuildingType)index;	

		for (int i = 0; i < activeProvince.inactiveBuildings.Count; i++)
		{
			if (activeProvince.inactiveBuildings[i].buildingType == buildingType)
			{
				activeProvince.activateBuilding(activeProvince.inactiveBuildings[i], true);
			}
		}

		PositionBuildingUI(activeProvince.inactiveBuildings, false);
		PositionBuildingUI(activeProvince.activeBuildings, true);
	}

	/*Takes a list of buildings to position, and whether they are active or inactive buildings.
	It will then set the building's parents to the relevant UI panel, and position them according
	to their position in their Building List.*/
	public void PositionBuildingUI(List<Building> buildings, bool active)
	{
		/*Grabs the relevant UI panel depending upon whether the current building list
		holds active or inactive buildings.*/
		Transform activityPanel;

		if (active)
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
			if (active)	//If building is in the active panel.
			{
				buildings[i].trans.GetChild(2).gameObject.SetActive(false);	//Deactivate button.
			}
			else if (!active)	//If building is in the inactive panel.
			{
				buildings[i].trans.GetChild(2).gameObject.SetActive(true); //Activate button.
			}
			

			buildings[i].trans.SetParent(activityPanel);
	
			float yPos;

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

			buildings[i].trans.localPosition = new Vector2(0, yPos);
		}
	}


	/*Updates the specified values on the specified UI elements with the given values.
	Will also give a sign automatically depending upon whether the values updated value
	is positive or negative. This function does not increment on a value, it only replaces it.*/
	void UpdateValue(float val, Text textObj)
	{
		string newValue;

		if (val > 0)
		{
			newValue = "+" + val.ToString();
		}
		else
		{
			newValue = val.ToString();
		}

		textObj.text = newValue;
	}

	/*This function should update the provinceManagementUI values as per the province that
	has been activated using the UpdateValues() function. */
	public void LoadProvinceValues(Province province)
	{
		activeProvince = province;

		for (int i = 0; i < provinceUI.Length - 1; i++)	//For every province UI element, not including name.
		{
			UpdateValue(activeProvince.getValues()[i], provinceUI[i]);	//Update by active province values.
		}
		
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
