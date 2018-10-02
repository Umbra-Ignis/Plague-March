using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_Joel : MonoBehaviour
{
    public Image img;

    public Image[] keys;

    public Image[] notes;

    private bool setOff = true;

	// Use this for initialization
	void Start (){}
	
	// Update is called once per frame
	void Update ()
    {
        if(setOff)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].enabled = false;
            }

            for (int i = 0; i < notes.Length; i++)
            {
                notes[i].enabled = false;
            }

            setOff = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            TurnOnKey(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            TurnOnKey(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            TurnOnKey(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            TurnOnKey(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            TurnOnNote(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            TurnOnNote(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            TurnOnNote(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            TurnOnNote(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            TurnOnNote(4);
        }
    }

    public void TurnOnKey(int number)
    {
        keys[number].enabled = true;
    }

    public void TurnOnNote(int number)
    {
        notes[number].enabled = true;
    }

    public void TurnOffKey(int number)
    {
        keys[number].enabled = false;
    }

    public void TurnOffNote(int number)
    {
        notes[number].enabled = false;
    }
}
