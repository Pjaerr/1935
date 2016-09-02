using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class infantryUnit : MonoBehaviour
{
    public int parentNation = 1; //When a unit is created, change it's parentNation number to the number of the nation that created it.
    public GameObject actionMenu; //The Panel containing any and all actions that can be taken surrounding this unit.
    public GameObject movementPin; //The default game object attached to a unit, to be placed. Is also cloned when placed instead of having a separate game object.
    private Vector2 movementPinPos;
    private bool placePinActive = false; //Controls whether or not the pin placement should be active.
    private bool moveUnitActive = false;
    public float movementSpeed = 2; //The speed the unit moves at. (Measured in units per Time.deltatime).
    public Transform trans; //This unit's transform.


    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        if (placePinActive)
        {
            placePin();
        }
        else if (moveUnitActive)
        {
            moveUnit();
        }

        //drawUnit();
    }

    /*void drawUnit()
    {
        //Add checks here to see if the unit's parentNation is allies with the NationManager.nationValue, if so, show the unit.
        //Add checks here to show unit no matter what the NationManager.nationValue is if this unit is within the distance of a province or unit owned by someone else.
        if (parentNation != NationManager.nationValue)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (parentNation == NationManager.nationValue)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }*/

    void placePin() //Method is called in the Update() function if placePinActive is true.
    {
        movementPin.SetActive(true); //Activates the movementPin game object so it can be shown on screen.
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Assigns the position of the mouse cursor to mouseCursorPos and makes it applicable in the world camera point.
        movementPin.transform.position = mouseCursorPos; //Moves the movementPin gameobject to the cursor position. (Follows the cursor)

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(movementPin, mouseCursorPos, Quaternion.Euler(0, 0, 0)); //Places a clone of movementPin where the mouse cursor was when left mouse was clicked.
            movementPinPos = mouseCursorPos;
            placePinActive = false;
            movementPin.SetActive(false); //Deactivates the default movementPin gameobject so it cannot be seen anymore.
            moveUnitActive = true; //Makes moveUnitActive true so that the moveUnit function can be called in the Update() function.
        }

    }
    void moveUnit() //Method is called in the Update() function if moveUnitActive is true.
    {
        Vector2 currentPos = GetComponent<Transform>().transform.position; //Sets the units current position.

        //Moves the unit towards the movement pin that was placed down at the current global movementSpeed.
        GetComponent<Transform>().transform.position = Vector2.MoveTowards(currentPos, movementPinPos, movementSpeed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        if (NationManager.nationValue == parentNation)
        {
            actionMenu.SetActive(true); // Open action menu.
        }
    }

    public void closeActionMenu()
    {
        actionMenu.SetActive(false); // Close action menu.
    }

    public void moveUnitClicked() //Method is called when the 'Move Unit' button is pressed.
    {
        Debug.Log("moveUnitClicked: Move Unit button was clicked");
        placePinActive = true;
        actionMenu.SetActive(false);
    }

    /*Unit Movement Description
    Methods: OnMousedown(), closeActionMenu(), Update(), placePin(), moveUnit()
    placePinActive is set to true when the moveUnitClicked() method is called, causing the Update() function to start calling placePin() which will
    show the Movement Pin sprite and make it follow the mouse cursor, this function will also check if the left mouse is clicked and, if so, will instantiate
    a copy of the Movement Pin and place it at the position the mouse cursor was at when clicked, it will then set the placePinActive to false so that the Update() function stops calling
    the placePin() function and the Movement Pin will dissapear and stop following the cursor. It will then set the moveUnitActive bool to true so that the Update() function can start calling
    the moveUnit() function which will get the units current position and move it towards the position of the copy of the movement pin that was placed via the placePin() function.
    TODO;
    .Make the copy of the movement pin delete itself when the unit reaches its position.
    .Add several checks (when networking is in place) to see whether the pin is being placed in the sea or in someone elses territory before actually placing the pin, 
    asking the user for confirmation first.
    .Make sure the user can only place one pin per unit at a time. If they place a new pin, remove the other pin.
    */
}
