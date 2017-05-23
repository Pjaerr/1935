using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour 
{
	private Transform trans;
	public Transform[] provinces;
	public Transform[] provincePoints;

	// Use this for initialization
	void Start ()
	{
		trans = GetComponent<Transform>();
		
	}

	void SetChildren()
	{
		for (int i = 0; i < trans.childCount; i++)
		{
			provinces[i] = trans.GetChild(i);
		}
	}

}
