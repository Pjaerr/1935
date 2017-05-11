using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour 
{
	/*UI ELEMENTS*/
	//NATION
	[SerializeField] Text nationText;
	[SerializeField] Image nationFlag;
	[SerializeField] Text happinessText;
	[SerializeField] Text economyText;
	//RESOURCES
	[SerializeField] Text foodText;
	[SerializeField] Text ironText;
	[SerializeField] Text coalText;

	private Text valueToChange;

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
		UpdateValue("happiness", GameManager.singleton.happinessVal);
		UpdateValue("economy", GameManager.singleton.economyVal);
		UpdateValue("food", GameManager.singleton.foodVal);
		UpdateValue("iron", GameManager.singleton.ironVal);
		UpdateValue("coal", GameManager.singleton.coalVal);
	}
	void UpdateValue(string type, int val)
	{
		string sign;

		if (val > 0)
		{
			sign = "+";
		}
		else
		{
			sign = "-";
		}

		switch(type)
		{
			case "happiness":
				valueToChange = happinessText;
				break;
			case "economy":
				valueToChange = economyText;
				break;
			case "food":
				valueToChange = foodText;
				break;
			case "iron":
				valueToChange = ironText;
				break;
			case "coal":
				valueToChange = coalText;
				break;
		}

		valueToChange.text = sign + val.ToString();
	}
}
