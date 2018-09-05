using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [HideInInspector]
    public GameObject settingsPanel;

    public void Awake()
    {
        settingsPanel = GameObject.FindGameObjectWithTag("Menu_Settings");
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }

  //  public void Exit()
  //  {
  //      Application.Quit;
  //  }
}
