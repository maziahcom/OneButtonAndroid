using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    public GameObject PlayerObject;
    private bool isActive;
    public float angleDown = 34.0f;
    public float angleUp = -34.0f;
    private float dx;
    public int rotationSpeed = 50;
    private float r;
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(PlayerObject, "Error: Add a PlayerObject to the PlayerController script");
        isActive = false;
        dx = 0;
    }

    public void SetSwingUp()
    {
        isActive = true;
        dx = -1;

    }
    public void SetSwingDown()
    {
        isActive = true;
        dx = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            //use r to hold a rotation value matching the format in the editor
            r = PlayerObject.transform.localEulerAngles.z;
            if (r > 180)
                r -= 360f;
            else if (r < -180)
                r += 360f;
            
            if (dx == -1)
            {
                if (r > angleUp)
                    PlayerObject.transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * dx, Space.Self);
                else
                    isActive = false;               
            }

            else if(dx == 1)
            {
                if (r < angleDown)
                    PlayerObject.transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * dx, Space.Self);
                else
                    isActive = false;
            }
        }
    }
}
