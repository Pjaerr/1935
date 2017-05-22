using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This UI class should be used to manage the data and referencing associated with any UI
in the game that isn't a part of an instantiatiable object.*/

public class UI : MonoBehaviour 
{
	/*Array of UI Text elements pertaining to the 'always on screen' UI.
	0 = Happiness
	1 = Economy
	2 = Food
	3 = Iron
	4 = Coal
	*/
	[SerializeField] private Text[] persistentUI;

	[SerializeField] private Text[] provinceManagementUI;
	[SerializeField] private GameObject provinceManagementUIParent;
	[HideInInspector] public bool provinceUIActive = false;

	[SerializeField] Text nationText;
	[SerializeField] Image nationFlag;

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
	
	void InitialiseValues()
	{
		//nationText.text = GameManager.singleton.thisNation.ToString();
		for (int i = 0; i < persistentUI.Length; i++)
		{
			UpdateValue(i, GameManager.singleton.nationValues[i], "persistentUI");
		}
		
	}
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
			provinceManagementUI[valueToUpdate].text = newValue;
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
		provinceManagementUIParent.SetActive(active);
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
