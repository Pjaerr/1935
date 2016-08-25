using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class infantryUnit : MonoBehaviour
{
    public int parentNation; //When a unit is created, change it's parentNation number to the number of the nation that created it.
    public GameObject actionMenu; //The Panel containing any and all actions that can be taken surrounding this unit.
    public GameObject movementPin; //The default game object attached to a unit, to be placed. Is also cloned when placed instead of having a separate game object.
    private Vector2 movementPinPos;
    private bool placePinActive = false; //Controls whether or not the pin placement should be active.
    private bool moveUnitActive = false;
    public float movementSpeed = 2; //The speed the unit moves at. (Measured in units per Time.deltatime).

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
    }

    void OnMouseDown()
    {
        actionMenu.SetActive(true); // Open action menu.

        Debug.Log("Unit clicked!");
    }

    public void closeActionMenu()
    {
        actionMenu.SetActive(false); // Close action menu.
    }

    public void moveUnitClicked() //Method is called when the 'Move Unit' button is pressed.
    {
        placePinActive = true;
        actionMenu.SetActive(false);
    }
    
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
            moveUnitActive = true;
        }

    }
    void moveUnit() //Method is called in the Update() function if moveUnitActive is true.
    {
        Vector2 currentPos = GetComponent<Transform>().transform.position; //Sets the units current position.
        //Moves the unit towards the movement pin that was placed down at the current global movementSpeed.
        GetComponent<Transform>().transform.position = Vector2.MoveTowards(currentPos, movementPinPos, movementSpeed * Time.deltaTime);
    }
}
