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
        img.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("UPDATE");
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Enabled");
            if (img.enabled == true)
                img.enabled = false;
            else
                img.enabled = true;
        }
	}
}
