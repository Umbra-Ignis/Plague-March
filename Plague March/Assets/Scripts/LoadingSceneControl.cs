using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneControl : MonoBehaviour {

    
    public GameObject LoadingScreenPanel;
    public Image LoadingBar;
    AsyncOperation async;

    private void Start()
    {
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
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
