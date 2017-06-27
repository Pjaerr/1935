using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour 
{
	public List<GameObject> clients = new List<GameObject>();
	public GameObject localClientObj;

	public void setLocalClientObj(GameObject clientObj)
	{
		localClientObj = clientObj;
	}

	[Command]
	public void CmdSetClient(GameObject clientObject)
	{
		Debug.Log("Setting client on server");
		clients.Add(clientObject);
	}

	private GameObject findClientInList(GameObject clientObject)
	{
		for (int i = 0; i < clients.Count; i++)
		{
			if (clientObject == clients[i])
			{
				return clients[i];
			}
		}

		return null;
	}

	[Command]
	public void CmdUpdateValues(GameObject clientObj)
	{
		GameObject clientToUpdate = findClientInList(clientObj);

		if (clientToUpdate != null)
		{
			RpcUpdateClient(clientToUpdate);
		}
	}

	[ClientRpc]
	private void RpcUpdateClient(GameObject newClientObj)
	{
		localClientObj = newClientObj;
	}



	public List<Unit> units;	//List of all units in the game, only used locally by the server.

	/*Default Attrbutes*/
	public float defaultUnitMovementSpeed = 20.0f; //Default value to be set elsewhere as per time scaling.

	/*World Map*/
	[SerializeField] private Transform WorldMap;
	private Transform[] nations;

	/*Nation Enumerator[s]*/
	public enum Nation{Austria, Belgium, Denmark, Finland, France, Germany, 
	Netherlands, Norway, Poland, Portugal, Spain, Sweden, Switzerland, UK, SIreland};


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
		SetWorldMap();
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

	/*Will set thisNationTransform to the transform in nations[] that represents the nation currently 
	being played as. This can then be referenced by GameManager.singleton.thisNationTransform*/
	public Transform findTransformOf(Nation thisNation)
	{
		switch (thisNation)
		{
			case Nation.Belgium:
				return nations[1];
			case Nation.Netherlands:
				return nations[2];
			case Nation.France:
				return nations[3];
			case Nation.Switzerland:
				return nations[4];
			case Nation.Germany:
				return nations[5];
			case Nation.Austria:
				return nations[6];
			case Nation.Denmark:
				return nations[7];
			case Nation.Finland:
				return nations[8];
			case Nation.UK:
				return nations[9];
			case Nation.SIreland:
				return nations[10];
			case Nation.Norway:
				return nations[11];
			case Nation.Poland:
				return nations[12];
			case Nation.Portugal:
				return nations[13];
			case Nation.Spain:
				return nations[14];
			case Nation.Sweden:
				return nations[15];
			default:
				Debug.Log("Unknown nation @ GameManager.cs::SetThisNation();");
				return null;
		}
	}
}
