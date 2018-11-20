//========================================================================================
//EndGameTrigger_Joel
//
//Functionality: Attached to a trigger box to send the player to the credits upon
//completeing the game
//
//Author: Joel G
//========================================================================================
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndGameTrigger_Joel : MonoBehaviour
{
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
