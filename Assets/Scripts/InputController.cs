using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool space_pressed_thisframe;
    private bool space_released_thisframe;
    private Touch touchStruct;

    public void InitInputs()
    {
        space_pressed_thisframe = false;
        space_released_thisframe = false;
        touchStruct = new Touch();
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
        space_released_thisframe = false;
        //
        //Handle mobile touch input first
        //
        for (int i = 0; i < Input.touchCount; i++)
        {
            touchStruct = Input.GetTouch(i);
            if (touchStruct.phase == TouchPhase.Began)
            {
                space_pressed_thisframe = true;
                return;
            }
            if (touchStruct.phase == TouchPhase.Ended)
            {
                space_released_thisframe = true;
                return;
            }
        }
        //
        //Handle keyboard input
        //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            space_pressed_thisframe = true;
            return;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            space_released_thisframe = true;
            return;
        }

    }
}
