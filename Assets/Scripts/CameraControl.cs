using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
 {
	Camera thisCamera;
	void Start () 
	{
		thisCamera = GetComponent<Camera>();
	}
}
