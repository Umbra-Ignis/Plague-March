//========================================================================================
//Credits
//
//Functionality: Called when the credits UI is active to allow players to close it.
//
//Author: Adrian P
//Altered by: Joel G
//========================================================================================
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    //Takes reference to the image 
    public Image m_imCreditsScreen;

    private void Update()
    {
        //Checks if the player has pressed space while the credit screen is active
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //If space has been pressed, the credits image is disabled
            m_imCreditsScreen.enabled = false;
        }
    }

    //Function to open the credit image from other scripts, as there are multiple
    //places it can be accessed from
    public void OpenCredits()
    {
        //Enables the credits image
        m_imCreditsScreen.enabled = true;
    }
}
