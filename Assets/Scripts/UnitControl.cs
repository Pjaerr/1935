using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitControl : NetworkBehaviour
{
	[Command]
	public void CmdPinPlaced(GameObject unit, Vector2 pinPos)
	{
		Unit thisUnit = unit.GetComponent<Unit>();	//Reference to the Unit.cs script attached to the passed in GameObject.
		Debug.Log("Pin has been placed!");
		thisUnit.isMoving = true;
		thisUnit.destination = pinPos;
	}



	/*CmdSpawnUnit() takes a string, a position and a parent nation. Using that data it will instantiate a gameobject
	passing in to Resources.Load() the string given. Once instantiated, it will spawn that gameobject on the server.
	The gameobject being a unit. It will also set the units parent nation to the passed in Nation enum.*/
	[Command]
	public void CmdSpawnUnit(string type, Vector2 pos, GameManager.Nation parentNation)
	{
		Debug.Log("thisNation is " + parentNation.ToString() + " @ UnitControl.cs");
		GameObject go = (GameObject)Instantiate(Resources.Load(type, typeof(GameObject)), pos, Quaternion.identity);
		go.GetComponent<Unit>().parentNation = parentNation;
		NetworkServer.Spawn(go);
	}
}
