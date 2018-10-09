using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum keyR
{
    k1,
    k2,
    k3,
    k4
}

public class DoorOpen_Joel : MonoBehaviour
{
    public keyR keyRequired;
    public Text noEntry;
    public Text entry;
    public Text closeDoor;
    public GameObject Gerard;
    private UserControler_Adrian user;
    private bool opened;

    public GameObject doorRight;
    public GameObject doorLeft;

    // Use this for initialization
    void Start()
    {
        noEntry.enabled = false;
        entry.enabled = false;
        closeDoor.enabled = false;
        user = Gerard.GetComponent<UserControler_Adrian>();
        opened = false;
    }

    // Update is called once per frame
    void Update(){}

    private void OnTriggerStay(Collider other)
    {
        if(user.haveKey((int)keyRequired + 1))
        {
            if (!opened)
            {
                entry.enabled = true;
                closeDoor.enabled = false;
            }

            if(opened)
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
