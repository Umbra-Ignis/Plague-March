using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneControl : MonoBehaviour {

    
    public GameObject LoadingScreenPanel = null;
    public Image LoadingBar = null;
    public Image LoadBorder = null;
    public Image ToolTip = null;
    AsyncOperation async;

    private void Start()
    {
        if (ToolTip != false)
        {
            ToolTip.enabled = false;
        }

        LoadingBar.fillAmount = 0;
    }

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
