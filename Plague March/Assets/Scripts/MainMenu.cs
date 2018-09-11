using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 


public class MainMenu : MonoBehaviour
{
    public GameObject returnMenu = null;
    public GameObject Player = null;

    void Start()
    {
        if (returnMenu != null)
            returnMenu.SetActive (false);

        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log ("QUIT");
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (returnMenu != null)
            returnMenu.SetActive(true);

        Cursor.visible = true;
        Time.timeScale = 0f;

    }
}

