using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionFromTitleScreen : MonoBehaviour
{
    SceneLoader loader;
    private bool transitionStarted;
    private void Awake()
    {
        transitionStarted = false;
        loader = FindObjectOfType<SceneLoader>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!transitionStarted)
            {
                transitionStarted = true;
                SceneLoader.nextScene = SceneLoader.SceneNames.level1;
                loader.LoadNextScene();
            }
        }
    }
}
