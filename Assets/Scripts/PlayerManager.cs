using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour 
{
	public DataManager dataManager;
	private ProvinceManagement provinceManagement;
	private UnitControl unitControl;
	

	void Start() 
	{
		Debug.Log("Player with ID: " + GetComponent<NetworkIdentity>().netId + " has connected");

		if (!isLocalPlayer)
		{
			return;
		}

		dataManager = GetComponent<DataManager>();

		
		/*Stores a local reference to this object so that other scripts can access this player's functions via the GameManager
		singleton.*/
		GameManager.singleton.setLocalClientReference(this.gameObject, dataManager);

		/*Temporary function that delays the ProvinceManagement Start() functionality until
		the PlayerManager has been started via the NetworkManager. This can be removed once
		the starting of a game is seperated from the actual game scene itself.*/
		
		provinceManagement.NetworkStart();	
		CmdSetAccessMatrixOfAllUnits();	//Set the access matrix for all units when new person joins.
		unitControl.CmdSpawnUnit("infantryUnitV1", provinceManagement.provinces[1].trans.position, dataManager.getThisNation());
		ServerStorage.singleton.CmdAddManagerToList(this.gameObject);
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
				units[i].RpcInitializeAccessMatrix();
			}
		}
	}
}
