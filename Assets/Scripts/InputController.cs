using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool space_pressed_thisframe;
    private bool space_released_thisframe;

    public void InitInputs()
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

    public void UpdateInputs()
    {
        space_pressed_thisframe = false;
        space_released_thisframe= false;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            space_pressed_thisframe = true;
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.Joystick1Button3))
            space_released_thisframe = true;


    }
}
