using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(ProcessInput))]
public class GameManagerScript : MonoBehaviour
{
    PlayerController playerController;
    ProcessInput inputController;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputController = GetComponent<ProcessInput>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(inputController.GetSpacePressedThisFrame())
        {
            playerController.SetSwingUp();
        }
        if (inputController.GetSpaceReleasedThisFrame())
        {
            playerController.SetSwingDown();
        }
    }
}
