using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraPositionAndRotation;
    private Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        cam = FindObjectOfType<Camera>(Camera.main);
        cam.transform.SetPositionAndRotation(cameraPositionAndRotation.position, cameraPositionAndRotation.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
