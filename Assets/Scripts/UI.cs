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
	*/
	[SerializeField] private Text[] provinceUI;
	[SerializeField] private GameObject provinceUIParent;	//The game object that holds the province management UI.
	[HideInInspector] public bool provinceUIActive = false;

	[SerializeField] Text nationText;	//The text that shows the name of the currently played nation.
	[SerializeField] Image nationFlag;	//The UI image that shows the flag of the currently played nation.

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
		InitialiseValues();
	}
	
	/*Sets all default values for the persistent UI.*/
	void InitialiseValues()
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

	/*This function should update the provinceManagementUI values as per the province that
	has been activated using the UpdateValues() function. */
	void LoadProvinceValues()
	{
		
	}

	/*Either Activates or Deactivates the province management UI depending upon the
	boolean passed in. If chosen to activate, it will call the LoadProvinceValues() function.*/
	public void ActivateProvinceManagementUI(bool active)
	{
		provinceUIParent.SetActive(active);
		if (active)
		{
			provinceUIActive = true;
			LoadProvinceValues();
		}
		else
		{
			provinceUIActive = false;
		}
	}
}
