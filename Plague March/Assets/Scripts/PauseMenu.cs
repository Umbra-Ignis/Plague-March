using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused =  false;

    public GameObject pauseMenuUI;


	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Sets Cursor to visable and not locked to the window
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (GameIsPaused)
            {
                Resume();
                //Sets Cursor to not visable and locked to the window
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
            }
            else
            {
                Pause();
            }    
        }	
	}
    public void Resume()
    {
        //Sets Cursor to not visable and locked to the window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
        //Allows quit button to work while playing in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
