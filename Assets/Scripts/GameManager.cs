using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	/*World Map*/
	[SerializeField] private Transform WorldMap;
	private Transform[] nations;
	[HideInInspector] public Transform thisNationTransform;

	/*Nation Enumerator[s]*/
	public enum Nation{Austria, Belgium, Denmark, Finland, France, Germany, Netherlands, Norway, Poland, Portugal, Spain, Sweden, Switzerland, UK, SIreland};
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

	void Start()
	{
		SetWorldMap();
		SetThisNationTransform();
	}

	void Update()
	{
		/*KEEP FOR TESTING.*/
		SetThisNationTransform();
	}

	/*Grabs all children Transforms of the referenced WorldMap object 
	and assigns them to an array of Transforms called nations[].*/
	void SetWorldMap()
	{
		nations = new Transform[WorldMap.childCount];	//nations[] is the same size as the number of children on WorldMap.

		for (int i = 0; i < WorldMap.childCount; i++)
		{	
			nations[i] = WorldMap.GetChild(i);
		}
	}

	/*Will set thisNationTransform to the transform in nations[] that represents the nation currently 
	being played as. This can then be referenced by GameManager.singleton.thisNationTransform*/
	void SetThisNationTransform()
	{
		switch (thisNation)
		{
			case Nation.Belgium:
				thisNationTransform = nations[1];
				break;
			case Nation.Netherlands:
				thisNationTransform = nations[2];
				break;
			case Nation.France:
				thisNationTransform = nations[3];
				break;
			case Nation.Switzerland:
				thisNationTransform = nations[4];
				break;
			case Nation.Germany:
				thisNationTransform = nations[5];
				break;
			case Nation.Austria:
				thisNationTransform = nations[6];
				break;
			case Nation.Denmark:
				thisNationTransform = nations[7];
				break;
			case Nation.Finland:
				thisNationTransform = nations[8];
				break;
			case Nation.UK:
				thisNationTransform = nations[9];
				break;
			case Nation.SIreland:
				thisNationTransform = nations[10];
				break;
			case Nation.Norway:
				thisNationTransform = nations[11];
				break;
			case Nation.Poland:
				thisNationTransform = nations[12];
				break;
			case Nation.Portugal:
				thisNationTransform = nations[13];
				break;
			case Nation.Spain:
				thisNationTransform = nations[14];
				break;
			case Nation.Sweden:
				thisNationTransform = nations[15];
				break;
			default:
				Debug.Log("Unknown nation @ GameManager.cs::SetThisNation();");
				break;
		}
	}
}
