using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerStorage : NetworkBehaviour 
{
	public static ServerStorage singleton = null;	//Singleton instance.
	
	void initializeSingleton()
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
		initializeSingleton();
	}


	/*Every Player has a DataManager that holds their local data, when a player joins, 
	their DataManager and all of the values, references and objects on that DataManager 
	are stored in a list on the server via a Server script acting as a singleton. 
	When a piece of data changes locally via direct access to the DataManager, it will 
	update that piece of data on the Servers copy of said DataManager by telling the 
	server which DataManager issued the change, and then running through the list of 
	DataManagers it holds on the network until it matches the one that made the change, 
	it will then update that networked copy with the changes.

	Then, whenever a player requests data from their datamanager (UI not withstanding as it isn't vital),
	it will ask the server for a copy of said data via the relevant DataManager using the method described above. 

	This makes sure that any data that actually affects the game will be grabbed from the server. Right now, the setting 
	of data on the server literally just takes the local copy when asked and so isn't secure, however if the method above 
	is implemented correctly, it allows for an easy way to perform checks before setting data as the getting of data is
	already secure as, when it matters, it only grabs serverside data. */

	public List<GameObject> clients = new List<GameObject>();

	[Command]
	public void CmdAddManagerToList(GameObject client)
	{
		clients.Add(client);
	}

	private GameObject findClientInList(GameObject localClient)	//Should only be called on the server.
	{
		for (int i = 0; i < clients.Count; i++)
		{
			if (localClient == clients[i])
			{
				return clients[i];
			}
		}

		return null;
	}

	
	[Command]
	public void CmdUpdateValues(GameObject localClientObj)
	{
		GameObject updatedClient = findClientInList(localClientObj);

		if (updatedClient != null)
		{
			RpcUpdateDataManager(localClientObj, updatedClient);
		}
	}

	[ClientRpc]
	private void RpcUpdateDataManager(GameObject localClient, GameObject updatedClient)
	{
		localClient = updatedClient;
	}
}
