﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
	/*MOVEMENT*/
	/*Movement speed for all units. Useful to set by default depending upon how quick the game should progress
	and letting child units increase/decrease the default movementSpeed by a scaling factor as needed. */
	private float movementSpeed = 0.0f;

	/*The Vector3 that the unit will move towards when Move() is called*/
	Vector3 destination;

	/*Factor by which to set this units movement speed. 
	This should be the only value changed on inherited objects.*/
	public float movementSpeedScalingFactor = 1.0f;	

	/*COMPONENTS*/
	private Transform trans; //This object's transform.
	private SpriteRenderer spriteRenderer;	//This object's sprite renderer.
	[SerializeField] private GameObject pinPrefab;	//The pin that will be instantiated.
	private GameObject pin;	//The instantiated pin as a game object.

	/*UI*/
	[SerializeField] private GameObject unitUI;

	/*CHECKS*/
	public GameManager.Nation parentNation;	//This unit's parent nation.

	private bool isVisible = false;			//Check to see if visible before making it visible.
	private bool isInteractable = false;
	private bool isMoving = false;
	private bool pinPlaced = false;	//Used to control pin placement.
	private bool pinActive = false;	//Used to initiate pin placement.


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
		if (pinActive)
		{
			PinPlacement();
		}
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

	/*Starts the pin placement*/
	public void placePin()
	{
		pinActive = true;
	}

	/*Controls the pin placement*/
	private void PinPlacement()
	{
		/*If a pin doesn't currently exist, instantiate one using the pin prefab attached to this unit.
		If the mouse is clicked, set pinPlaced to true. If pinPlaced is true set the unit's destination
		to the position of the pin, and set isMoving to true, setting the unit in motion. If pinPlaced is not true,
		make the pin game object follow the mouse.*/

		if (unitUI.activeSelf)
		{
			unitUI.SetActive(false);
		}
		if (!pin)
		{
			pin = Instantiate(pinPrefab, trans.position, Quaternion.identity) as GameObject;
		}
		else if (!pin.activeSelf)
		{
			pin.SetActive(true);
			pinPlaced = false;
		}

		if (Input.GetMouseButtonDown(0))
		{
			pinPlaced = true;
			pinActive = false;
		}

		if (pinPlaced)
		{
			isMoving = true;
			destination = pin.transform.position;
		}
		else
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0.0f;
			pin.transform.position = mousePos;
		}
	}

	/*Moves the unit towards the destination at the given movementSpeed*/
	private void Move()
	{
		if (isMoving && trans.position == destination)
		{
			isMoving = false;
			pin.SetActive(false);	//Rework in time to accomodate for Object Pooling.
		}

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
}
