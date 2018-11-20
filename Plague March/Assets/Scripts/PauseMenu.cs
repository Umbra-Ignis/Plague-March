//========================================================================================
//PauseMenu
//
//Functionality: Used to operate the pause menu while in game
//
//Author: Adrian P
//Altered by: Joel G
//========================================================================================
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused =  false;

    public GameObject pauseMenuUI;
    public bool quickTime;
    new AudioSource audio;


	// Update is called once per frame
	void Update ()
    {
        quickTime = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement_Adrian>().m_bQuicktime;

        audio = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();

        if (Input.GetKeyDown(KeyCode.Escape) && !quickTime)
        {
            //Sets Cursor to visable and not locked to the window
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (GameIsPaused)
            {
                Resume();
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
        Cursor.visible = false;

        audio.UnPause();

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        audio.Pause();
        GameIsPaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitGame()
    {
        Application.Quit();
        //Allows quit button to work while playing in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
