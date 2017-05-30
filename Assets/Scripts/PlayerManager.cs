using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour 
{
	[SerializeField] GameManager.Nation thisNation;	//This nations nation enum.

	/*This nations values in order of:
	0 = Happiness
	1 = Economy
	2 = Food
	3 = Iron
	4 = Coal */
	private float[] nationValues;

	void Start() 
	{

		Debug.Log("Player with ID: " + GetComponent<NetworkIdentity>().netId + " has connected!");

		nationValues = new float[5] {1, 1000, 500, 500, 500};	//Initialising the nation values.

		if (!isLocalPlayer)
		{
			return;
		}

		/*Storing a local copy of this player's data so that it can be accessed for
		local use such as UI or Camera Control. As mentioned in the GameManager, the
		locally stored data should never be used within anything that affects the where
		other players are concerned.*/
		GameManager.singleton.SetLocalValues(thisNation, nationValues, this.gameObject);

		/*Temporary function that delays the ProvinceManagement Start() functionality until
		the PlayerManager has been started via the NetworkManager. This can be removed once
		the starting of a game is seperated from the actual game scene itself.*/
		GetComponent<ProvinceManagement>().NetworkStart();	
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Key pressed on the server: " + isServer);
			CmdMoveUnit();
		}
		
	}


	[Command]
	void CmdMoveUnit()
	{
		
		Debug.Log("Command Function Called");
		
	}
}
