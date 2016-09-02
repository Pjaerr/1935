using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NationManager : MonoBehaviour
{
    //Nation Attributes
    public static int nationValue = 1; //The number value of the nation the player is playing as. 0 = France, 1 = Germany etc.


    //UI
    public GameObject UI; //The temporary nation selection UI.
    

    //Method used to assign the nation value to the player based on which nation they chose.
    public void nationAssignation(int passedNationValue)
    {
        nationValue = passedNationValue;
        //UI.SetActive(false);
    }





}
