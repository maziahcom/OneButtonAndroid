using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SceneNames
    {
        preTitle2,
        titleScreen,
        level1,
        level2,
        level3,
        win,
        lose
    }

    public static SceneNames nextScene = SceneNames.preTitle2;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        nextScene = SceneNames.preTitle2;
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene.ToString());

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            yield return null;
        }
        Debug.Log("LOADED SCENE: " + nextScene.ToString());
    }

    

}
