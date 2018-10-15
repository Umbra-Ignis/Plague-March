﻿using UnityEngine;
using UnityEngine.UI;

//Stores each different key object that can be picked up
//Allowing each item to be set to a different object
[HideInInspector]
public enum pickupType
{
    key1,
    key2,
    key3,
    key4,
    note1,
    note2,
    note3,
    note4,
    note5
}

public class Pickup_Joel : MonoBehaviour
{
    //Allows each item to be set to a different key item index
    public pickupType type;
    //Tip that comes up indicating to the player what actions to take to pick up the item
    public Text tooltip;
    //Allows access to the UI controller to turn UI images on and off
    public Canvas TooltipCanvas;
    //Stores the reference to the UI controller script in order to turn on and off item images
    private UIController_Joel ui;
    //Stores whether the item has been picked up yet or not
    private bool toBePickedUp;

    private bool opened;

    public Image popupImage;

    // Use this for initialization
    void Start()
    {
        //Ensures the text is initially turned off
        tooltip.enabled = false;
        //Obtains reference to the UI controller script
        ui = TooltipCanvas.GetComponent<UIController_Joel>();
        //Sets the item to be allowed to be picked up
        toBePickedUp = true;
        opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && opened)
        {
            Time.timeScale = 1;
            popupImage.enabled = false;

            //Turns on the UI image of this selected item
            ui.TurnOnItem((int)type);

            //Turns off the tooltip text of this item as it can no longer be picked up
            //Switches the bool to ensure the item cannot be picked up again, and to 
            //ensure the false option is not displayed to the player
            toBePickedUp = false;
            opened = false;
        }
    }

    //Detects when another object enters the trigger of this object
    private void OnTriggerStay(Collider other)
    {
        //Checks if the other object is the player to avoid unneccessary calls
        if(other.CompareTag("Player") && toBePickedUp)
        {
            //Turns on the tooltip text to direct the player which actions to take to pick up the item
            tooltip.enabled = true;
            //Checks if the player completes the above stated actions
            if(Input.GetKeyDown(KeyCode.E))
            {
                Time.timeScale = 0;

                tooltip.enabled = false;
                popupImage.enabled = true;
                opened = true;
                other.GetComponent<UserControler_Adrian>().obtainedKey((int)type + 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tooltip.enabled = false;
        }
    }
}
