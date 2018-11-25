//========================================================================================
//LoadingSceneControl
//
//Functionality: Operates the loading screen between the main menu and the game,
//and loads the level whilst doing so.
//
//Author: Adrian P
//========================================================================================
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneControl : MonoBehaviour {

    //Loading screen as an object
    public GameObject LoadingScreenPanel = null;

    [Space]
    //Images for loading screen
    public Image LoadingBar = null;
    public Image LoadBorder = null;
    public Image ToolTip = null;
    
    //Async for loading Progress
    AsyncOperation async;

    private void Start()
    {
        if (ToolTip != false)
        {
            //Turn tool tip off
            ToolTip.enabled = false;
        }

        //Bar starts at zero
        LoadingBar.fillAmount = 0;
    }

    public void LoadScreenStart(int Level)
    {
        //Starts loading level
        StartCoroutine(LoadingScreen(Level));
    }

    IEnumerator LoadingScreen(int Level)
    {
        //Panels turn on and loading begins
        LoadingScreenPanel.SetActive(true);
        async = SceneManager.LoadSceneAsync(Level);
        async.allowSceneActivation = false;

        while(async.isDone == false)
        {
            LoadingBar.fillAmount = async.progress;
            //PLAY LOADING ANIM
            if (async.progress == 0.9f)
            {

                LoadingBar.fillAmount = 1;
                if (LoadBorder != null)
                {
                    LoadBorder.enabled = false;
                }

                if (LoadingBar != null)
                {
                    LoadingBar.enabled = false;
                }

                if (ToolTip != null)
                {
                    ToolTip.enabled = true;
                }
                //If loaded and space is pressed change scenes
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

}
