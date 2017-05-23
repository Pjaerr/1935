using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvinceManagement : MonoBehaviour 
{
	private List<Transform> provinces;
	private List<Transform> provincePoints;
	private Transform trans;
	bool isRaised = false;
	private Transform activeProvince;

	void Start()
	{
		trans = GetComponent<Transform>();
		SetProvinces();
	}

	void SetProvinces()
	{
		provinces = new List<Transform>();
		provincePoints = new List<Transform>();

		for (int i = 0; i < trans.childCount; i++)
		{
			AddProvince(trans.GetChild(i));	//Adds the initial nations provinces and province points.
		}
	}

	/*AddProvince() takes a transform, it will then add that transform to the list of provinces, and, 
	if it has a province point (which all should, just a failsafe), it will add that province point to the list
	of province points.*/
	void AddProvince(Transform province)
	{
		provinces.Add(province);

		if (province.childCount > 0)
		{
			provincePoints.Add(province.GetChild(0));
			Debug.Log("Added " + province.name + " and it's province point to this nations provinces.");
		}
	}
	/*RemoveProvince() takes a transform, it will then go through the list of provinces and if that transform
	exists within the list of provinces, it will remove that transform.*/
	void RemoveProvince(Transform province)
	{
		for (int i = 0; i < provinces.Count; i++)
		{
			if (province == provinces[i])
			{
				Debug.Log("Province: " + province + " has been removed.");
				provinces.Remove(provinces[i]);

				if (province.childCount > 0 && province.GetChild(0) == provincePoints[i])
				{
					provincePoints.Remove(provincePoints[i]);
				}
			}
		}
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

	bool provinceIsClicked()
	{
		bool isClicked = false;

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null)
			{
				isClicked = true;
				activeProvince = hit.transform.parent;
			}
		}

		return isClicked;
	}
}
