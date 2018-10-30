using UnityEngine;
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
    //Takes in the image that pops up when the note is interacted with
    public Image popupImage;
    //Takes in a text element that shows to the player what they have picked up
    public Text acquiredText;

    [Space]
    [Space]

    //Stores whether this item will turn on the activation of another
    public bool turnOnOther;
    //If picking up one item is to turn on another, this should be set to the key it turns on
    public GameObject turnOnObject;
    //Takes in a reference to a particle effect to turn on if there is a second item to be turned on
    public ParticleSystem partEffect;

    [Space]
    [Space]

    //Stores whether inspecting the note will also pickup a key
    public bool noteNKey;
    //Stores which key is picked up at this time
    public pickupType keyNumber;
    //Takes a reference to the key object to turn it off once picked up
    public GameObject keyObject;
    //Takes in a text element that shows to the player what they have picked up
    public Text acquiredKeyText;

    //================================================================================================================
    //PRIVATES
    //================================================================================================================
    //Stores the reference to the UI controller script in order to turn on and off item images
    private UIController_Joel ui;
    //Stores whether the item has been picked up yet or not
    private bool toBePickedUp;
    //Stores whether the image has been opened or not
    private bool opened;
    //Stores how long the popup text is displayed to the screen
    private float textTimer;

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
        if (turnOnOther)
        {
            turnOnObject.GetComponent<SphereCollider>().enabled = false;
            partEffect.Stop();
        }
        acquiredText.enabled = false;
        textTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && opened)
        {
            Time.timeScale = 1;
            popupImage.enabled = false;
            acquiredText.enabled = false;

            //Turns on the UI image of this selected item
            ui.TurnOnItem((int)type);

            //Turns off the tooltip text of this item as it can no longer be picked up
            //Switches the bool to ensure the item cannot be picked up again, and to 
            //ensure the false option is not displayed to the player
            opened = false;

            if(noteNKey)
            {
                ui.TurnOnItem((int)keyNumber);
                keyObject.SetActive(false);
                acquiredKeyText.enabled = true;
            }

            if(turnOnOther)
            {
                turnOnObject.GetComponent<SphereCollider>().enabled = true;
                partEffect.Play();
            }
        }

        if(acquiredKeyText.enabled == true)
        {
            textTimer += Time.deltaTime;
        }

        if(textTimer >= 5.0f)
        {
            acquiredKeyText.enabled = false;
            acquiredKeyText.gameObject.SetActive(false);
            textTimer = 0.0f;
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
            if(Input.GetKeyDown(KeyCode.E) && (int)type >= 4)
            {
                Time.timeScale = 0;

                tooltip.enabled = false;
                if(acquiredKeyText.enabled == true)
                {
                    acquiredKeyText.enabled = false;
                }
                acquiredText.enabled = true;
                popupImage.enabled = true;
                opened = true;
                if(partEffect != null)
                {
                    partEffect.Stop();
                }
                if(noteNKey)
                {
                    other.GetComponent<UserControler_Adrian>().obtainedKey((int)keyNumber + 1);
                }
            }

            else if(Input.GetKeyDown(KeyCode.E))
            {
                tooltip.enabled = false;
                acquiredText.enabled = true;
                //Turns on the UI image of this selected item
                ui.TurnOnItem((int)type);
                //Turns off the tooltip text of this item as it can no longer be picked up
                //Switches the bool to ensure the item cannot be picked up again, and to 
                //ensure the false option is not displayed to the player
                toBePickedUp = false;
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
