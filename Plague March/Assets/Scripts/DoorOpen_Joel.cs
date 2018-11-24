//========================================================================================
//DoorOpen_Joel
//
//Functionality: Attached to doors to allow them to open, close, lock and unlock
//
//Author: Joel G
//Altered by: Adrian P
//========================================================================================
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
    public Image noEntry;
    //Takes a reference to a UI element in which tells the player they can press a button to open this door
    public Image entry;
    //Takes reference to the player to allow access to its script
    public GameObject Gerard;
    //Stores a reference to the players script
    private UserControler_Adrian user;
    //Stores whether the door is has been opened or not
    [HideInInspector]
    public bool opened;

    public AudioClip doorOpen = null;

    //Takes a reference to the right side of the gate
    public GameObject doorRight;
    //Takes a reference to the left side of the gate
    public GameObject doorLeft;
    //Takes reference to the single door if there is only one
    public GameObject m_goSingleDoor;
    //Stores whether the door is a single door
    public bool m_bSingleDoor;
    //Stores whether the door should always be unlocked
    public bool alwaysOpen;
    //Stores whether the player has the needed key to unlock the door
    private bool hasRequiredKey = false;
    //Stores whether the door has been permanently locked
    private bool permaLock = false;

    // Use this for initialization
    void Start()
    {
        //Disables all text elements to ensure they do not show
        noEntry.enabled = false;
        entry.enabled = false;
        
        //Obtains the user controller script in which to be used later in the script
        user = Gerard.GetComponent<UserControler_Adrian>();
        //Ensures the door is set to closed intially
        opened = false;

        //Checks whether THIS door should be always unlocked
        if(alwaysOpen)
        {
            //Cheats this by just stating that the player has the required key, even though there is no key required
            hasRequiredKey = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Continually checks if the player obtains the required key to open the door
        if(user.haveKey((int)keyRequired + 1))
        {
            hasRequiredKey = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !m_bSingleDoor)
        {
            if (hasRequiredKey && !permaLock)
            {
                if (!opened)
                {
                    entry.enabled = true;
                }

                if (opened)
                {
                    entry.enabled = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!opened)
                    {
                        entry.enabled = false;
                        doorLeft.transform.Rotate(new Vector3(0, 1, 0), -96.0f);
                        doorRight.transform.Rotate(new Vector3(0, 1, 0), 96.0f);
                        opened = true;

                        Debug.Log("OPEN");

                        if (doorOpen != null)
                        {
                            GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>().PlayOneShot(doorOpen);
                        }
                    }
                }
            }

            else
            {
                noEntry.enabled = true;
            }
        }

        else if (other.CompareTag("Player") && m_bSingleDoor)
        {
            if (hasRequiredKey)
            {
                if (!opened)
                {
                    entry.enabled = true;
                }

                if (opened)
                {
                    entry.enabled = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!opened)
                    {
                        opened = true;
                        entry.enabled = false;
                        m_goSingleDoor.transform.Rotate(new Vector3(0, 1, 0), -96.0f);

                        if (doorOpen != null)
                        {
                            GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>().PlayOneShot(doorOpen);
                        }
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
        }
    }

    public void CloseDoor()
    {
        opened = false;
        doorLeft.transform.Rotate(new Vector3(0, 1, 0), 96.0f);
        doorRight.transform.Rotate(new Vector3(0, 1, 0), -96.0f);

        permaLock = true;
    }
}
