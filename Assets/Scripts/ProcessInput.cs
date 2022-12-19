using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessInput : MonoBehaviour
{
    private bool space_pressed_thisframe;
    private bool space_released_thisframe;

    void Start()
    {
        space_pressed_thisframe = false;
        space_released_thisframe = false;
    }
    public bool GetSpacePressedThisFrame()
    {
        return space_pressed_thisframe;
    }
    public bool GetSpaceReleasedThisFrame()
    {
        return space_released_thisframe;
    }

    void Update()
    {
        space_pressed_thisframe = false;
        space_released_thisframe= false;
        if (Input.GetKeyDown(KeyCode.Space))
            space_pressed_thisframe = true;
        else if (Input.GetKeyUp(KeyCode.Space))
            space_released_thisframe = true;
    }
}
