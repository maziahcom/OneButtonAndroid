using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlur : MonoBehaviour
{
    public Material motionBlurMaterial;
    Vector3 previousPosition;
    // Start is called before the first frame update
    void Start()
    {
        motionBlurMaterial.SetVector("_V3Pos", transform.position);
        motionBlurMaterial.SetVector("_V3PrevPos", transform.position);
        motionBlurMaterial.SetVector("_V3Velocity", Vector3.zero);
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        motionBlurMaterial.SetVector("_V3PrevPos", previousPosition);
        motionBlurMaterial.SetVector("_V3Pos", transform.position);
        motionBlurMaterial.SetVector("_V3Velocity", transform.position - previousPosition);
        previousPosition = transform.position;
        
    }
}
