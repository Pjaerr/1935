using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour 
{
	public DataManager dataManager;
	private ProvinceManagement provinceManagement;
	public UnitControl unitControl;
	
	void Start() 
	{
		Debug.Log("Player with ID: " + GetComponent<NetworkIdentity>().netId + " has connected");

		if (isLocalPlayer)
		{
			SetupClient();
		}
	}

	private void initialiseReferences()
	{
		dataManager = GetComponent<DataManager>();
		provinceManagement = GetComponent<ProvinceManagement>();
		unitControl = GetComponent<UnitControl>();
	}

	void SetupClient()
	{

		GameManager.singleton.setLocalClientObj(this.gameObject);
		GameManager.singleton.CmdSetClient(this.gameObject);

		initialiseReferences();

		GameManager.Nation thisNation = (GameManager.Nation)Random.Range(0, 14);
		dataManager.initialiseData(thisNation);


		/*Temporary function that delays the ProvinceManagement Start() functionality until
		the PlayerManager has been started via the NetworkManager. This can be removed once
		the starting of a game is seperated from the actual game scene itself.*/
		provinceManagement.NetworkStart();							//Delayed Province Management start.
		Camera.main.GetComponent<CameraControl>().NetworkStart();	//Delayed Camera start.
		UI.singleton.NetworkStart();								//Delayed UI start.

		CmdSetAccessMatrixOfAllUnits();
		unitControl.CmdSpawnUnit("infantryUnitV1", provinceManagement.provinces[1].trans.position, thisNation);
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
