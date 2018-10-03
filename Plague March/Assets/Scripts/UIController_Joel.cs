using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_Joel : MonoBehaviour
{
    public Image img;

    public Image[] items;

    private bool setOff = true;

	// Use this for initialization
	void Start (){}
	
	// Update is called once per frame
	void Update ()
    {
        if(setOff)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].enabled = false;
            }
            setOff = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            TurnOnItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            TurnOnItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            TurnOnItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            TurnOnItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            TurnOnItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            TurnOnItem(5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            TurnOnItem(6);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            TurnOnItem(7);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            TurnOnItem(8);
        }
    }

    public void TurnOnItem(int number)
    {
        items[number].enabled = true;
    }

    public void TurnOffItem(int number)
    {
        items[number].enabled = false;
    }
}
