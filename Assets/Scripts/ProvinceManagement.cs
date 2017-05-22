using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvinceManagement : MonoBehaviour 
{
	private Transform trans;
	bool isRaised = false;

	void Start()
	{
		trans = GetComponent<Transform>();
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

	void RaiseProvince(bool raise)
	{
		if (raise && !isRaised)
		{
			isRaised = true;
			trans.Translate(new Vector3(0, 0.2f, 0));
		}
		else if (!raise && isRaised)
		{
			isRaised = false;
			trans.Translate(new Vector3(0, -0.2f, 0));
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
			}
		}

		return isClicked;
	}
}
