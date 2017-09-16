using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
 {
	 /*Components*/
	private Camera thisCamera;
	private Transform trans;
	private Transform thisNationTransform;	//Access this nations transform via this.


	/*Mouse*/
	Vector3 mouseOrigin;
	Vector3 mousePos;
	[SerializeField] float scrollSpeed = 2.0f;

	void Start () 
	{
		thisCamera = GetComponent<Camera>();
		trans = GetComponent<Transform>();

		/*Avoids doing something that requires the GameManager values to actually work
		as before the NetworkManager has started the game, the GameManager will not have any
		values and will throw a NullReferenceException.This can be removed once the starting 
		of a game is seperated from the actual game scene itself.*/
		if (GameManager.networkIsConnected)
		{
			PanToNation();
		}
	}

	void Update()
	{
		CameraManipulation();
	}

	/*Will move the camera to the current nation when called.*/
	void PanToNation()
	{
		thisNationTransform = GameManager.singleton.thisDataManager.getThisNationTransform();
		trans.position = new Vector3(thisNationTransform.position.x, thisNationTransform.position.y, -10);
	}

	void CameraManipulation()
	{
		/*Makes sure that the camera is within bounds.
		 Includes both position and orthographic size.*/
		if (trans.position.x > 80)
		{
			trans.Translate(new Vector2(-thisCamera.orthographicSize, 0));
		}
		else if (trans.position.x < -80)
		{
			trans.Translate(new Vector2(thisCamera.orthographicSize, 0));
		}
		if (trans.position.y > 50)
		{
			trans.Translate(new Vector2(0, -thisCamera.orthographicSize));
		}
		else if (trans.position.y < -50)
		{
			trans.Translate(new Vector2(0, thisCamera.orthographicSize));
		}

		if (thisCamera.orthographicSize < 2)
		{
			thisCamera.orthographicSize = 2;
		}
		else if (thisCamera.orthographicSize > 50)
		{
			thisCamera.orthographicSize = 50;
		}

		MoveCamera();
		ZoomCamera();
	}
	void MoveCamera()
	{
		/*When right mouse button is pressed down once, set the mouseOrigin vector to the current mouse position
		in world space. Set it's z value to 0 to avoid moving the camera's z.*/
		if (Input.GetMouseButtonDown(1))
		{
			mouseOrigin = thisCamera.ScreenToWorldPoint(Input.mousePosition);
			mouseOrigin.z = 0.0f;
		}

		/*When the right mouse button is continually pressed down, set the mousePos vector to the current mouse
		position in world space. Set it's z value to 0 to avoid moving the camera's z and then translate
		the camera from the mouse origin to the mouse position. This results in smooth movement as the mouse origin
		is being set everytime a new 'drag' is started.*/
		if (Input.GetMouseButton(1))
		{
			mousePos = thisCamera.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0.0f;
			trans.Translate(mouseOrigin - mousePos);
		}
	}
	void ZoomCamera()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && thisCamera.orthographicSize > 2)
		{
			thisCamera.orthographicSize -= scrollSpeed;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && thisCamera.orthographicSize < 50)
		{
			thisCamera.orthographicSize += scrollSpeed;
		}
	}
}
