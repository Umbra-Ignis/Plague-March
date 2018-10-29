using UnityEngine;
using UnityEngine.UI;

//Dictates the type of keys that can be used to open doors
public enum keyR
{
    k1,
    k2,
    k3,
    k4
}

public class DoorOpen_Joel : MonoBehaviour
{
    //Stores the key number in which is required to open this door
    public keyR keyRequired;
    //Takes a reference to a UI element in which tells the player they cannot enter this door
    public Text noEntry;
    //Takes a reference to a UI element in which tells the player they can press a button to open this door
    public Text entry;
    //Takes a reference to a UI element in which tells the player they can press a button to close this
    public Text closeDoor;
    //Takes reference to the player to allow access to its script
    public GameObject Gerard;
    //Stores a reference to the players script
    private UserControler_Adrian user;
    //Stores whether the door is has been opened or not
    private bool opened;

    //Takes a reference to the right side of the gate
    public GameObject doorRight;
    //Takes a reference to the left side of the gate
    public GameObject doorLeft;

    // Use this for initialization
    void Start()
    {
        //Disables all text elements to ensure they do not show
        noEntry.enabled = false;
        entry.enabled = false;
        closeDoor.enabled = false;
        
        //Obtains the user controller script in which to be used later in the script
        user = Gerard.GetComponent<UserControler_Adrian>();
        //Ensures the door is set to closed intially
        opened = false;
    }

    // Update is called once per frame
    void Update(){} //DELIBERATLY LEFT BLANK

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (user.haveKey((int)keyRequired + 1))
            {
                if (!opened)
                {
                    entry.enabled = true;
                    closeDoor.enabled = false;
                }

                if (opened)
                {
                    entry.enabled = false;
                    closeDoor.enabled = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!opened)
                    {
                        opened = true;
                        entry.enabled = false;
                        doorLeft.transform.Rotate(new Vector3(0, 1, 0), -96.0f);
                        doorRight.transform.Rotate(new Vector3(0, 1, 0), 96.0f);
                        closeDoor.enabled = true;
                    }

                    else
                    {
                        opened = false;
                        closeDoor.enabled = false;
                        doorLeft.transform.Rotate(new Vector3(0, 1, 0), 96.0f);
                        doorRight.transform.Rotate(new Vector3(0, 1, 0), -96.0f);
                        entry.enabled = true;
                    }
                }
            }

            else
            {
                noEntry.enabled = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            noEntry.enabled = false;
            entry.enabled = false;
            closeDoor.enabled = false;
        }
    }
}
