using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
	/*MOVEMENT*/
	/*Movement speed for all units. Useful to set by default depending upon how quick the game should progress
	and letting child units increase/decrease the default movementSpeed by a scaling factor as needed. */
	private float movementSpeed = 0.0f;

	/*Factor by which to set this units movement speed. 
	This should be the only value changed on inherited objects.*/
	public float movementSpeedScalingFactor = 1.0f;	

	/*The Vector2 that the unit will move towards when Move() is called*/
	Vector2 destination;

	/*COMPONENTS*/
	private Transform trans; //This object's transform.
	private SpriteRenderer spriteRenderer;	//This object's sprite renderer.

	/*UI*/
	[SerializeField] private GameObject unitUI;

	/*CHECKS*/
	public GameManager.Nation parentNation;	//This unit's parent nation.

	private bool isVisible = false;			//Check to see if visible before making it visible.
	private bool isInteractable = false;
	private bool isMoving = false;


	/*MONOBEHAVOUR TEMPLATES*/

	/*Function used to update this Unit.cs class. This allows the actual monobehaviour Update()
	to be called on the classes that inherit from Unit.cs. These functions should only be used
	for code that occurs for all units.*/
	public void unitStart()
	{
		SetAccessMatrix();	//Allocate access for this unit to the client.
		trans = GetComponent<Transform>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		movementSpeed = GameManager.singleton.defaultUnitMovementSpeed * movementSpeedScalingFactor;
	}
	public void unitUpdate()
	{
		if (isMoving)
		{
			Move();
		}
	}

	/*MONOBEHAVIOUR FUNCTIONS*/
	/*Calls a function when mouse clicks on the collider attached to this.gameObject. */
	void OnMouseDown()
	{
		DisplayUnitUI(true);
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

	/*This function should be called to initiate movement, passing in a destination and setting
	isMoving to true so that the actual movement can occur in unitUpdate().*/
	public void StartMoving(Vector2 pos)
	{
		isMoving = true;
		destination = pos;
	}

	/*Moves the unit towards the destination at the given movementSpeed*/
	private void Move()
	{
		float step = movementSpeed * Time.deltaTime; //Temporary movement value. Swap this out for predefined time value, so that the unit moves over time.
		trans.position = Vector2.MoveTowards(trans.position, destination, step);
	}

	/*Activates and/or Deactivates this unit's UI menu.*/
	public void DisplayUnitUI(bool isActive)
	{
		if (isInteractable)
		{
			unitUI.SetActive(isActive);
		}
	}
	//TODO: Have seperate movement UI that is activated here.
	public void DisplayMoveUI(bool isActive)
	{
		//moveUI.SetActive(isActive);
	}
}
