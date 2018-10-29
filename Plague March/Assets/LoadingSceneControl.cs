using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneControl : MonoBehaviour {

    
    public GameObject LoadingScreenPanel;
    AsyncOperation async;

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
            //PLAY LOADING ANIM
            if (async.progress == 0.9f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
