using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public List<GameObject> objectsDontDestroy = new List<GameObject>();
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
    private void Awake()
    {
        //Set target framerate according to refresh rate of player's screen
        //Application.targetFrameRate = 30;//Screen.currentResolution.refreshRate;
    }
    void Start()
    {   //Scene loader has to be always available (across scenes)
        DontDestroyOnLoad(gameObject);
        
        //Other objects can be always available (e.g., global volume, main camera, event system)
        for(int i = 0; i < objectsDontDestroy.Count; i++)
            DontDestroyOnLoad(objectsDontDestroy[i]);

        //set the first scene to load
        nextScene = SceneNames.preTitle2;

        //load the first scene
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
