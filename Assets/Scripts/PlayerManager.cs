using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour 
{
	public GameManager.Nation thisNation;	//This nations nation enum.
	private ProvinceManagement provinceManagement;
	private UnitControl unitControl;

	/*This nations values in order of:
	0 = Happiness
	1 = Economy
	2 = Food
	3 = Iron
	4 = Coal */
	private float[] nationValues;

	void Start() 
	{
		Debug.Log("Player with ID: " + GetComponent<NetworkIdentity>().netId + " has connected");

		if (!isLocalPlayer)
		{
			return;
		}
		
		initialiseReferences();

		thisNation = (GameManager.Nation)Random.Range(0, 14);
		

		nationValues = new float[5] {1, 1000, 500, 500, 500};	//Initialising the nation values.
		
		/*Storing a local copy of this player's data so that it can be accessed for
		local use such as UI or Camera Control. As mentioned in the GameManager, the
		locally stored data should never be used within anything that affects the where
		other players are concerned.*/
		GameManager.singleton.SetLocalValues(thisNation, nationValues, this.gameObject);

		/*Temporary function that delays the ProvinceManagement Start() functionality until
		the PlayerManager has been started via the NetworkManager. This can be removed once
		the starting of a game is seperated from the actual game scene itself.*/
		
		provinceManagement.NetworkStart();	//Delayed Province Management start.
		Camera.main.GetComponent<CameraControl>().NetworkStart();	//Delayed Camera start.
		UI.singleton.NetworkStart();	//Delayed UI start.

		CmdSetAccessMatrixOfAllUnits();
		unitControl.CmdSpawnUnit("infantryUnitV1", provinceManagement.provinces[1].trans.position, thisNation);
	}

	private void initialiseReferences()
	{
		provinceManagement = GetComponent<ProvinceManagement>();
		unitControl = GetComponent<UnitControl>();
	}

	[Command]
	void CmdSetAccessMatrixOfAllUnits()
	{
		List<Unit> units = GameManager.singleton.units;

		if (units.Count > 0)
		{
			for (int i = 0; i < units.Count; i++)
			{
				Debug.Log("Setting access matrix for unit " + i);
				if (units[i] != null)
				{
					units[i].RpcInitializeAccessMatrix();
				}
				
			}
		}
	}
}
