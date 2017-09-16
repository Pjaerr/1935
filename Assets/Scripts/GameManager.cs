using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour 
{
	public List<Unit> units;	//List of all units in the game, only used locally by the server.

	public static bool networkIsConnected = false;

	/*Default Attrbutes*/
	public float defaultUnitMovementSpeed = 20.0f; //Default value to be set elsewhere as per time scaling.

	/*World Map*/
	[SerializeField] private Transform WorldMap;
	private Transform[] nations;

	/*Nation Enumerator[s]*/
	public enum Nation{Austria, Belgium, Denmark, Finland, France, Germany, 
	Netherlands, Norway, Poland, Portugal, Spain, Sweden, Switzerland, UK, SIreland};

	[HideInInspector] public GameObject thisClient;
	[HideInInspector] public DataManager thisDataManager;

	public static GameManager singleton = null;	//Singleton instance.
	

	void InitializeSingleton()
	{
		if (singleton == null)	//Check if an instance of GameManager already exists.
		{
			singleton = this; 	//If not, make this that instance.
		}
		else if (singleton != this)	//If an instance already exists and it isn't this.
		{
			Destroy(gameObject);	//Destroy this.
		}

		DontDestroyOnLoad(gameObject);
	}

	void Awake()
	{
		InitializeSingleton();
		SetWorldMap();
	}

	public void setLocalClientReference(GameObject playerManager, DataManager dataManager)
	{
		thisClient = playerManager;
		thisDataManager = dataManager;
		networkIsConnected = true;
	}

	/*Grabs all children Transforms of the referenced WorldMap object 
	and assigns them to an array of Transforms called nations[].*/
	void SetWorldMap()
	{
		nations = new Transform[WorldMap.childCount];	//nations[] is the same size as the number of children on WorldMap.

		/*For every nation under worldmap, add each one incrementally to nations[]*/
		for (int i = 0; i < WorldMap.childCount; i++)
		{	
			nations[i] = WorldMap.GetChild(i);
		}
	}

	public Transform findTransformOf(Nation nation)
	{
		return nations[0];	//Returns the transform matching the position in the Nation enum of that passed in.
	}
}
