using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
    public static int sceneIndex;
    public static List<string> sceneNames = new List<string>();

    public static SceneNames nextScene = SceneNames.preTitle2;
    private void Awake()
    {
        //Set target framerate according to refresh rate of player's screen
        //Application.targetFrameRate = 30;//Screen.currentResolution.refreshRate;
        sceneIndex = 0;
        sceneNames.Clear();
        sceneNames.Add("preTitle2");
        sceneNames.Add("titleScreen");
        sceneNames.Add("level1");
        sceneNames.Add("level2");
        sceneNames.Add("level3");
        sceneNames.Add("win");
        sceneNames.Add("lose");
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

    public static void LoadNextScene()
    {
        //StartCoroutine(LoadYourAsyncScene());
        SceneManager.LoadScene(sceneNames.ElementAt(sceneIndex));
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
