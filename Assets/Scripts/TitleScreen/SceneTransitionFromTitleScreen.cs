using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionFromTitleScreen : MonoBehaviour
{
    //Touch tt = new Touch();
    
    //SceneLoader loader;
    private bool transitionStarted;
    //Pub pub;
    private void Awake()
    {
        transitionStarted = false;
        //loader = FindObjectOfType<SceneLoader>();
        //pub = new Pub();
        Pub.OnChange += () => KeyPressed();
    }
    private void KeyPressed()
    {
        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!transitionStarted)
            {
                transitionStarted = true;
                SceneLoader.sceneIndex += 1;
                //SceneLoader.nextScene = SceneLoader.SceneNames.level1;
                SceneLoader.LoadNextScene();
                //loader.LoadNextScene();
            }
        }
    }
}
