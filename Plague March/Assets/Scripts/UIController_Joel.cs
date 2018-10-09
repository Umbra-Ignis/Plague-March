using UnityEngine;
using UnityEngine.UI;

public class UIController_Joel : MonoBehaviour
{
    //Takes in reference to each key and note to be rendered in the UI
    public Image[] items;
    //Takes in a reference to the rock UI image
    public Image rock;

    //Reference to Gerard to control the movement script, altering the rock UI
    public GameObject Gerard;

    //Stores whether or not the images have been initially turned off or not
    private bool setOff = true;
    //Stores a reference to the movescript obtained through Gerard
    private Movement_Adrian moveScript;

	// Use this for initialization
	void Start ()
    {
        //Obtains the movescript off Gerard
        moveScript = Gerard.GetComponent<Movement_Adrian>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Ensures all images are turned off when the scene goes through its first update
        if(setOff)
        {
            //Goes through all items to ensure they are turned off
            for (int i = 0; i < items.Length; i++)
            {
                //Turns off the selected image from the for loop
                items[i].enabled = false;
            }
            //Switches the bool to ensure this only happens once, on the first update
            setOff = false;
        }

        //Stores the rock count to check whether to display the rock image or not
        int rc = moveScript.GetRockCount();

        //Checks if the rock count is above 0 to show the rock UI image
        if (rc > 0)
        {
            //Turns on the rock UI image
            rock.enabled = true;
        }

        else
        {
            //Ensures the rock image is turned off if the player is not holding a rock
            rock.enabled = false;
        }
        //========================================================================
        //UNCOMMENT TO DEBUG UI ELEMENTS
        //========================================================================

        //if (Input.GetKeyDown(KeyCode.Keypad0))
        //{
        //    TurnOnItem(0);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad1))
        //{
        //    TurnOnItem(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad2))
        //{
        //    TurnOnItem(2);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad3))
        //{
        //    TurnOnItem(3);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad4))
        //{
        //    TurnOnItem(4);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad5))
        //{
        //    TurnOnItem(5);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad6))
        //{
        //    TurnOnItem(6);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad7))
        //{
        //    TurnOnItem(7);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad8))
        //{
        //    TurnOnItem(8);
        //}
    }

    //Allows an items image to be turned on externally to this script
    //Takes in an int in which dictates which image to display
    //Each int corresponds to the index of the image in the array of images input above
    public void TurnOnItem(int number)
    {
        //Turns on the item image with the index passed in
        items[number].enabled = true;
    }

    //Allows an items image to be turned of externally to this script
    //Takes in an int in which dictates which image to turn off
    //Each int corresponds to the index of the image in the array of images input above
    public void TurnOffItem(int number)
    {
        //Turns off the item image with the index passed in
        items[number].enabled = false;
    }
}
