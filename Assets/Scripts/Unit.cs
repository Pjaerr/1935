using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
	/*Movement speed for all units. Useful to set by default depending upon how quick the game should progress
	and letting child units increase/decrease the default movementSpeed by a scaling factor as needed. */
	[SerializeField] private float movementSpeed;

	private Transform trans; //This object's transform.
	private SpriteRenderer spriteRenderer;	//This object's sprite renderer.

	public GameManager.Nation parentNation;	//This unit's parent nation.

	[SerializeField] private GameObject unitUI;

	private bool isVisible = false;			//Check to see if visible before making it visible.
	private bool isInteractable = false;
	

	/*Monobehaviour Functions*/
	void Start()
	{
		SetAccessMatrix();	//Allocate access for this unit to the client.
		trans = GetComponent<Transform>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	/*Calls a function when mouse clicks on the collider attached to this.gameObject. */
	void OnMouseDown()
	{
		DisplayUI(true);
	}

	//TODO: Tidy up access matrix code. Maybe think about using arrays, lists of somesort of the conditions.
	private void UnitAccessMatrix(int level)
	{
		/*Access Levels: 0 = Max access, 1 = Limited access, 2 = No Access*/
		if (level == 0)
		{
			/*Make this unit both visible and interactable to this client.*/
			isVisible = true;
			isInteractable = true;
		}
		else if (level == 1)
		{
			/*Make this unit only visible to this client.*/
			isVisible = true;
			isInteractable = false;
		}
		else if (level == 2)
		{
			isVisible = false;
			isInteractable = false;
		}
	}

	private void SetAccessMatrix()
	{
		/*If this unit belongs to the nation this client is currently registered as.*/
		if (parentNation == GameManager.singleton.thisNation)
		{
			UnitAccessMatrix(0);	//Give this client all access to this unit.
		}
		else
		{
			UnitAccessMatrix(2);
		}
	}

	//Moves the unit towards the destination.
	private void Move(Vector2 destination)
	{
		float step = movementSpeed * Time.deltaTime; //Temporary movement value. Swap this out for predefined time value, so that the unit moves over time.
		trans.position = Vector2.MoveTowards(trans.position, destination, step);
	}

	/*Activates and/or Deactivates this unit's UI menu.*/
	public void DisplayUI(bool isActive)
	{
		if (isInteractable)
		{
			unitUI.SetActive(isActive);
		}
	}
}
