using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Province
{
	public string name;
	Transform trans;
	Transform provincePoint;

	/*These are the data values. value[0] is the actual value and value[1] is the amount
	by which the first value will be changed every so often*/
	int[] happiness = new int[2];
	int[] economy = new int[2];
	int[] food = new int[2];
	int[] iron = new int[2];
	int[] coal = new int[2];
	int[] population = new int[2];

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
	private List<Province> provinces;

	private Transform trans;
	bool isRaised = false;
	private Transform activeProvince;

	void Start()
	{
		trans = GetComponent<Transform>();
		InitProvinces();
	}

	void InitProvinces()
	{
		provinces = new List<Province>();

		for (int i = 0; i < trans.childCount; i++)
		{
			AddProvince(new Province(trans.GetChild(i)));
		}
	}
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
		if (provinceIsClicked())
		{
			UI.singleton.ActivateProvinceManagementUI(true);
			RaiseProvince(true);
		}
		else if (!UI.singleton.provinceUIActive)
		{
			RaiseProvince(false);
		}
	}
	/*ACTIVE PROVINCE IS THE PROVINCE THAT HAS BEEN CLICKED. ASSOCIATE ANY DATA THAT
	NEEDS TO BE ADDED TO THE UI WITH THAT TRANSFORM.*/
	void RaiseProvince(bool raise)
	{
		if (raise && !isRaised)
		{
			isRaised = true;
			activeProvince.Translate(new Vector3(0, 0.3f, 0));
		}
		else if (!raise && isRaised)
		{
			isRaised = false;
			activeProvince.Translate(new Vector3(0, -0.3f, 0));
		}
	}


	/*TO FIX THE PROVINCE UI BEING ACTIVATED WHEN THE UNIT IS CLICKED, 
	MAKE SURE TO CHECK IF THE RAYCAST IS HITTING A COLLIDER THAT IS A CHILD OF THE
	PARENT THIS SCRIPT IS ATTACHED TO, OR ATLEAST SOME WAY LINKED VIA HEIRARCHY. */
	bool provinceIsClicked()
	{
		bool isClicked = false;

		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Ray Fired");
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null && hit.transform.parent.IsChildOf(trans))
			{
				Debug.Log("Province Clicked");
				isClicked = true;
				activeProvince = hit.transform.parent;
			}
		}

		return isClicked;
	}
}
