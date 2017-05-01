using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	/*Nation Enumerator[s]*/
	public enum Nation{Austria, Belgium, Denmark, Finland, France, Germany, Netherlands, Norway, Poland, Portugal, Spain, Sweden, Switzerland, UK};
	public Nation thisNation;	//The Nation that this client is playing as.

	public static GameManager singleton = null;	//Singleton instance.

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
	
	/*Will set the thisNation to the value chosen within the function. Currently uses a dropdown menu and will select the nation chosen.*/
	public void NationSelect(int index)
	{
		switch (index)
		{
			case 0:
				thisNation = Nation.Austria;
				break;
			case 1:
				thisNation = Nation.Belgium;
				break;
			case 2:
				thisNation = Nation.Denmark;
				break;
			case 3:
				thisNation = Nation.Finland;
				break;
			case 4:
				thisNation = Nation.France;
				break;
			case 5:
				thisNation = Nation.Germany;
				break;
			case 6:
				thisNation = Nation.Netherlands;
				break;
			case 7:
				thisNation = Nation.Norway;
				break;
			case 8:
				thisNation = Nation.Poland;
				break;
			case 9:
				thisNation = Nation.Portugal;
				break;
			case 10:
				thisNation = Nation.Spain;
				break;
			case 11:
				thisNation = Nation.Sweden;
				break;
			case 12:
				thisNation = Nation.Switzerland;
				break;
			case 13:
				thisNation = Nation.UK;
				break;
			default:
				Debug.Log("Unknown Nation. Setting thisNation to Austria");
				thisNation = Nation.Austria;
				break;
		}

		Debug.Log("Nation set to " + thisNation + " @ NationManager::NationSelect()");
	}
}
