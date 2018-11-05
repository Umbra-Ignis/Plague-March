using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

    public Image CreditsScreen;

    public void OpenCredits()
    {
        CreditsScreen.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreditsScreen.enabled = false;
        }
    }
}
