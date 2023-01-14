using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MotionBluWithInstance : MonoBehaviour
{
    public Mesh mesh;
    public Material material;

    private int instanceCount;
    private int frameCount;
    private List<Matrix4x4> matrix = new List<Matrix4x4>();
    private Vector3[] framePositions;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Renderer m_renderer;
    private List<Material> materials = new List<Material>();
    private float yDiff;
    private Material m_material;
    private bool initialised = false;
    public void InitMotionBlur(int targetFrameRate)
    {
        //frameCount is how many frames of position data to hold for interpolation
        frameCount = targetFrameRate/6;
        
        //instanceCount is how many instances of the mesh to draw for the trail
        instanceCount = Mathf.RoundToInt(frameCount / 1.2f);
        
        //framePositions is a store for all the position data
        framePositions = new Vector3[frameCount];

        //setup a store store of matrix of transforms
        for (int i = 0; i < instanceCount; i++)
        {
            matrix.Add(new Matrix4x4());
        }

        //the material of the original mesh
        m_renderer = GetComponent<Renderer>();
        m_material = m_renderer.material;
        
        //set alpha to full opaque for main object
        Color colorA = m_material.color;
        colorA.a = 1;
        m_material.color = colorA;

        //setup clones of the material for the trail
        for (int i = 0; i < instanceCount; i++)
        {
            materials.Add(new Material(material));
        }
        //decreses the alpha of each subsequent mesh instance in the trail
        for (int i = 0; i < materials.Count; i++)
        {
            Color color = materials[i].color;
            float alpha = ((i * i) / (float)materials.Count)/(targetFrameRate/(float) 9) ;//-0.3333f;
            if(alpha > 1.0f)
                alpha = 1.0f;
          //  if(alpha < 0.0f)
           //     alpha = 0.0f;
            color.a = alpha;
           // color.r = 0.25f;
           // color.g = 0.25f;
           // color.b = 0.25f;
            materials[i].color = color;
        }
        for( int i = 0; i < framePositions.Length; i++)
        {
            framePositions[i] = transform.position;
        }
        initialised = true;
    }

    // Update is called once per frame
    public void UpdateMotionBlur()
    {
        //if (!initialised)
        //    return;
        //rotate frame positions array
        for (int i = framePositions.Length - 1; i >= 1; i--)
        {
            framePositions[i] = framePositions[i-1];
        }
        framePositions[0] = transform.position;

        //store current position and scale because they will be modified during instancing (then we restore later)
        originalPosition = transform.position;
        originalScale = transform.localScale;

        //yDiff is a vector along which we blur - lets use first and last frame from the frame array
        yDiff = framePositions[0].y - framePositions[frameCount-1].y;

        //draw the 0th mesh unaltered
        //matrix[0] = transform.localToWorldMatrix;

        //draw the mesh a number of times, increasing the transparency and shrinking its size each step
        float j = 0;
        for (int i = matrix.Count-1; i >= 0; i--)
        {
            transform.position = originalPosition - new Vector3(0, Mathf.Lerp(0,yDiff,j), 0);
            j += 1 / (float)matrix.Count;
            //transform.position = originalPosition - new Vector3(0, (yDiff/(float) (instanceCount*2)*(i+0.5f)), 0);
            //transform.localScale = originalScale - new Vector3((i / (float)instanceCount) * originalScale.x + Random.Range(-0.8f, 0.2f), 0, 0);//(originalScale.y / 1.4f), (originalScale.z / 1.4f));
            matrix[i] = transform.localToWorldMatrix;
            Graphics.DrawMesh(mesh, matrix[i], materials[i], 0);
        }
        
        //restore transform position and scale
        transform.position = originalPosition;
        transform.localScale = originalScale;
    }
}
