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
    public GameObject Gerard;
    private UserControler_Adrian user;
    private bool opened;

    private bool open;

    // Use this for initialization
    void Start()
    {
        noEntry.enabled = false;
        entry.enabled = false;
        open = false;
        user = Gerard.GetComponent<UserControler_Adrian>();
        opened = false;
    }

    // Update is called once per frame
    void Update(){}

    private void OnTriggerStay(Collider other)
    {
        if(user.haveKey((int)keyRequired + 1))
        {
            if(!opened)
                entry.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                opened = true;
                entry.enabled = false;
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
        }
    }
}
