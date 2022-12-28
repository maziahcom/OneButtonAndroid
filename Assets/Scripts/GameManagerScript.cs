using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(EntityController))]
[RequireComponent(typeof(EnvironmentController))]
public class GameManagerScript : MonoBehaviour
{
    PlayerController playerController;
    InputController inputController;
    EntityController entityController;
    EnvironmentController environmentController;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputController = GetComponent<InputController>(); 
        entityController = GetComponent<EntityController>();
        environmentController = GetComponent<EnvironmentController>();
    }
    void Start()
    {
        environmentController.InitMaterials();
        entityController.InitEntities();
        inputController.InitInputs();
        playerController.InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        inputController.UpdateInputs();
        if(inputController.GetSpacePressedThisFrame())
        {
            playerController.ButtonWasPressed();
        }
        if (inputController.GetSpaceReleasedThisFrame())
        {
            playerController.ButtonWasReleased();
        }
        entityController.UpdateEntities();
        playerController.UpdatePlayer();
    }
    private void FixedUpdate()
    {
        //entityController.UpdateEntities();
        //playerController.UpdatePlayer();
    }
}
