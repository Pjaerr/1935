using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : NetworkBehaviour 
{
	private GameManager.Nation thisNation;
	private Transform thisNationTransform;
	private List<Province> provinces;

	private ValueContainer nationValues;

	public void initialiseData()
	{
		thisNation = (GameManager.Nation)Random.Range(0, 14);
		nationValues = new ValueContainer(new string[] {"economy", "food", "iron", "coal"}, new float[] {1, 1000, 500, 500, 500});

		//Should grab the nation via order of enums and transforms**
		thisNationTransform = GameManager.singleton.findTransformOf(thisNation);
	}

	public void loadProvinces(List<Province> provincesArg)
	{
		provinces = provincesArg;
		Debug.Log("Loaded Provinces into DataManager @ DataManager.cs::loadProvinces()");
	}

	public float[] getNationValues(bool isNetworked = false)
	{
		if (isNetworked)
		{
			ServerStorage.singleton.CmdUpdateValues(this.gameObject);
		}

		return nationValues.values.ToArray();
	}
	public GameManager.Nation getThisNation(bool isNetworked = false)
	{
		if (isNetworked)
		{
			ServerStorage.singleton.CmdUpdateValues(this.gameObject);
		}

		return thisNation;
	}
	public Transform getThisNationTransform(bool isNetworked = false)
	{
		if (isNetworked)
		{
			ServerStorage.singleton.CmdUpdateValues(this.gameObject);
		}

		return thisNationTransform;
	}
	
	/*public Transform getThisNationTransform()
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
	}*/
}
