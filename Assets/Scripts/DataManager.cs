using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : NetworkBehaviour 
{
	[HideInInspector] public GameManager.Nation thisNation;	//This nations nation enum.
	[HideInInspector] public Transform thisNationTransform;
	[HideInInspector] public List<Province> provinces;

	/*This nations values in order of:
	0 = Happiness, 1 = Economy, 2 = Food, 3 = Iron, 4 = Coal */
	private int[] nationValues;

	void NetworkStart()
	{
		
	}

	public void initialiseData(GameManager.Nation nationArg)
	{
		thisNation = nationArg;
		nationValues = new int[5] {1, 1000, 500, 500, 500};	//Initialising the nation values.

		thisNationTransform = GameManager.singleton.thisNationTransform;
	}

	public void loadProvinces(List<Province> provincesArg)
	{
		provinces = provincesArg;
		Debug.Log("Loaded Provinces into DataManager @ DataManager.cs::loadProvinces()");
	}

	
	public Transform getThisNationTransform()
	{
		GameManager.singleton.CmdUpdateValues(this.gameObject);
		return thisNationTransform;
	}

	public GameManager.Nation getThisNation()
	{
		GameManager.singleton.CmdUpdateValues(this.gameObject);
		return thisNation;
	}
	public int[] getNationValues()
	{
		GameManager.singleton.CmdUpdateValues(this.gameObject);
		return nationValues;
	}
}
