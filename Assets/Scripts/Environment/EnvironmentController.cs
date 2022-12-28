using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Renderer))]
public class EnvironmentController : MonoBehaviour
{
    public Material floor_mat;
    public GameObject floor_obj;
    public GameObject ceiling_obj;
    public Material skybox_mat;

    public void InitMaterials()
    {
        Assert.IsNotNull(floor_mat);
        Assert.IsNotNull(floor_obj);
        Assert.IsNotNull(ceiling_obj);

        floor_obj.GetComponent<Renderer>().material = floor_mat;
        ceiling_obj.GetComponent<Renderer>().material = floor_mat;
        RenderSettings.skybox = skybox_mat;
        //RenderSettings.skybox.SetFloat("_Rotation", 90);
    }
}
