using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
	public string name;
	public Transform trans;
	Transform provincePoint;

	/*These are the data values. value[0] is the actual value and value[1] is the amount
	by which the first value will be changed every so often*/
	public int[] happiness = new int[2] {0, 0};
	public int[] economy = new int[2] {0, 0};
	public int[] food = new int[2] {0, 0};
	public int[] iron = new int[2] {0, 0};
	public int[] coal = new int[2] {0, 0};
	public int[] population = new int[2] {0, 0};

	/*The object constructor. Takes a transform which will be assigned as this provinces
	transform, it will automatically grab the provinces province point if it exists*/
	public Province(Transform provinceTransform)
	{
		trans = provinceTransform;

		if (provinceTransform.childCount > 0)
		{
			provincePoint = provinceTransform.GetChild(0);
		}

		name = trans.name;
	}

	public void UpdateValues()
	{
		happiness[0] += happiness[1];
		economy[0] += economy[1];
		food[0] += food[1];
		iron[0] += iron[1];
		coal[0] += coal[1];
		population[0] += population[1];
	}
}

public class ProvinceManagement : MonoBehaviour 
{
	public List<Province> provinces;	//List of provinces belonging to this nation. Province objects.

	private Transform trans;	//This nations transform.
	bool isRaised = false;	//Is the active province currently raised.
	private Province activeProvince;	//The province that is currently active.

	/*TEMPORARY START METHOD, THIS IS ONLY IN PLACE WHILST STARTING A NETWORK AND THE GAME SCENE ARE THE SAME 
	TO AVOID TRYING TO SET DATA WHEN IT DOESN'T EXIST.*/
	public void NetworkStart()
	{
		trans = GameManager.singleton.thisNationTransform;
		InitialiseProvinces();
	}

	void InitialiseProvinces()
	{
		provinces = new List<Province>();

		for (int i = 0; i < trans.childCount; i++)
		{
			AddProvince(new Province(trans.GetChild(i)));
		}
	}

	/*Removes and/or adds a province to this nations province list. */
	void AddProvince(Province province)
	{
		provinces.Add(province);
	}
	void RemoveProvince(Province province)
	{
		provinces.Remove(province);
	}

	void Update()
	{
		if (provinceIsClicked() && !UI.singleton.provinceUIActive)
		{
			UI.singleton.ActivateProvinceManagementUI(true);
			UI.singleton.LoadProvinceValues(activeProvince);
			RaiseProvince(true);
		}
		else if (!UI.singleton.provinceUIActive && !provinceIsClicked())
		{
			RaiseProvince(false);
		}
	}

	/*Raises the active province, should be called when a province is clicked and activated.*/
	///bool raise should be true or false for whether to raise the province or not respectively.
	void RaiseProvince(bool raise)
	{
		if (raise && !isRaised)
		{
			isRaised = true;
			activeProvince.trans.Translate(new Vector3(0, 0.3f, 0));
		}
		else if (!raise && isRaised)
		{
			isRaised = false;
			activeProvince.trans.Translate(new Vector3(0, -0.3f, 0));
		}
	}

	/*This function should be called every frame. It will check if the right mouse button is down and if so, fire
	a raycast from the mouse. If it hits, it will do some checking to see if it is a province, and if so, it will check
	which province, and return the relevant object as the active province for use elsewhere. It will also return true or false
	if a province is clicked.*/
	bool provinceIsClicked()
	{
		bool isClicked = false;

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);	//Sends raycast from mouse.
			if (hit.collider != null && hit.transform.parent != null)	//If it has hit something and what it has hit has a parent.
			{
				if (hit.transform.IsChildOf(trans))	//Is the object a child of this nation.
				{
					isClicked = true;
					for (int i = 0; i < provinces.Count; i++)
					{
						if (hit.transform.parent == provinces[i].trans)	//Checks which province the province point that was clicked is a child of.
						{
							activeProvince = provinces[i];	//Sets that province as active.
							break;
						}
					}
				}
			}
		}

		return isClicked;
	}
}
