using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyResourceLoaderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    SceneLoader loader;
    private void Awake()
    {
        loader = FindObjectOfType<SceneLoader>();
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(0.2f);
        SceneLoader.nextScene = SceneLoader.SceneNames.preTitle2;
        loader.LoadNextScene();
    }
}
