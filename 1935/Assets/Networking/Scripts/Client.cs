using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Client : NetworkBehaviour
{
    public static int parentNation = 0;

	// Use this for initialization
	void Start ()
    {
        parentNation = NationManager.nationValue;
	}
	
	// Update is called once per frame
	void Update ()
    {
        

    }
}
