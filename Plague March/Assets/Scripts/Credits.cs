//========================================================================================
//Credits
//
//Functionality: Called when the credits UI is active to allow players to close it.
//
//Author: Adrian P
//Altered by: Joel G
//========================================================================================
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private void Update(){}//DELIBERATLY LEFT BLANK

    //Function to open the credit image from other scripts, as there are multiple
    //places it can be accessed from
    public void OpenCredits()
    {
        //Loads the third scene in the build index
        SceneManager.LoadScene(2);
    }
}
