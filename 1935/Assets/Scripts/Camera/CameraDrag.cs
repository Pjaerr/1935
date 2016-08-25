using UnityEngine;

public class CameraDrag : MonoBehaviour 
{
	private Vector3 CameraDefaultPos;
	private Vector3 Origin;
	private Vector3 Difference;
	private bool Drag = false;

	void Start () 
	{
		CameraDefaultPos = Camera.main.transform.position; //CameraDefaultPos is equal to the main camera's position.
	}
    void Update()
    {
        Camera.main.transform.position = new Vector3(
        Mathf.Clamp(Camera.main.transform.position.x, -400, 1050), Mathf.Clamp(Camera.main.transform.position.y, -780, 762), Camera.main.transform.position.z);
    }
	void LateUpdate () 
	{
        

		if (Input.GetMouseButton (0)) 
		{
			//Assigns the difference between the mouse position and the camera position to variable 'difference'
			Difference = (Camera.main.ScreenToWorldPoint (Input.mousePosition))- Camera.main.transform.position;

			if (Drag == false)
			{
				Drag = true;
				Origin = Camera.main.ScreenToWorldPoint (Input.mousePosition); //Origin is equal to the mouse position
			}
		} 
		else 
		{
			Drag = false;
		}


		if (Drag == true)
		{
			Camera.main.transform.position = Origin - Difference;
		}

		//RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
		if (Input.GetMouseButton (1)) 
		{
			Camera.main.transform.position = CameraDefaultPos;
		}
	}
}