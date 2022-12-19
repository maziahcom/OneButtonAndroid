using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(ProcessInput))]
[RequireComponent(typeof(EntityController))]
public class GameManagerScript : MonoBehaviour
{
    PlayerController playerController;
    ProcessInput inputController;
    EntityController entityController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputController = GetComponent<ProcessInput>(); 
        entityController = GetComponent<EntityController>();
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
        entityController.UpdateEntities();
    }
}
