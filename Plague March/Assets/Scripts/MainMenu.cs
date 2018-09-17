using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 


public class MainMenu : MonoBehaviour
{
    public GameObject returnMenu = null;

    //Gets Infection
    public Infection_Adrian infect = null;

    void Start()
    {
        //Check if returnMenu Object is empty
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


    private void Update()
    {
        if (infect.m_fInfection >= 100)
        {
            if (returnMenu != null)
            {
                returnMenu.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if returnMenu Object is empty
        if (returnMenu != null)
            returnMenu.SetActive(true);

    }
}

