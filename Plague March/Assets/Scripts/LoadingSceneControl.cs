using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneControl : MonoBehaviour {

    //loading screen panel
    public GameObject LoadingScreenPanel = null;
    //loading bar
    public Image LoadingBar = null;
    //loading bar border
    public Image LoadBorder = null;
    //loading bar border
    public Image ToolTip = null;
    //async for loading progress
    AsyncOperation async;

    private void Start()
    {
        if (ToolTip != false)
        {
            // disable tooltip
            ToolTip.enabled = false;
        }

        LoadingBar.fillAmount = 0;
    }
    //start loading screen
    public void LoadScreenStart(int Level)
    {
        StartCoroutine(LoadingScreen(Level));
    }

    IEnumerator LoadingScreen(int Level)
    {
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

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

}
