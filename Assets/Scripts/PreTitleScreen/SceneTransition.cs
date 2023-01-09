using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    SceneLoader loader;
    Camera cam;
    private void Awake()
    {
        loader = FindObjectOfType<SceneLoader>();
        cam = FindObjectOfType<Camera>(Camera.main);
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(6.5f);
        SceneLoader.nextScene = SceneLoader.SceneNames.titleScreen;
        loader.LoadNextScene();
    }

}
