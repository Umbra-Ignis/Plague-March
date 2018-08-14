using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_Joel : MonoBehaviour
{
    public Image img;

	// Use this for initialization
	void Start ()
    {
        //Turns off the image so that it is not always visible
        img.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (img.enabled == true)
                img.enabled = false;
            else
                img.enabled = true;
        }
	}
}
