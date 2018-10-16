using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MainMenu : MonoBehaviour
{
    public GameObject returnMenu = null;

    //Gets Infection
    public GameObject infect = null;

    void Start()
    {
        infect = GameObject.FindGameObjectWithTag("Player");
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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


    private void Update()
    {
        if (infect != null)
        {
            if (infect.GetComponent<Infection_Adrian>().m_fInfection >= 100)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if returnMenu Object is empty
        if (returnMenu != null)
        {
            returnMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

