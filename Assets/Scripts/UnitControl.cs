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
}
