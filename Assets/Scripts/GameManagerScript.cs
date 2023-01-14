using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(EntityController))]
[RequireComponent(typeof(EnvironmentController))]
public class GameManagerScript : MonoBehaviour
{
    private PlayerController playerController;
    private InputController inputController;
    private EntityController entityController;
    private EnvironmentController environmentController;
    private UIController uiController;
    private TitleScreen titleScreen;
    private MotionBluWithInstance motionBlurController;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputController = GetComponent<InputController>(); 
        entityController = GetComponent<EntityController>();
        environmentController = GetComponent<EnvironmentController>();
        uiController = GetComponent<UIController>();
        titleScreen = GetComponent<TitleScreen>();
        motionBlurController = FindObjectOfType<MotionBluWithInstance>();
    }
    void Start()
    {
        environmentController.InitMaterials();
        entityController.InitEntities();
        entityController.StartEntitiesLevel1();
        inputController.InitInputs();
        playerController.InitPlayer();
        //remove this later for final build (it can be set ealier in the scene loader)
        Application.targetFrameRate = 120;//Screen.currentResolution.refreshRate;
        motionBlurController.InitMotionBlur(Application.targetFrameRate);
    }

    // Update is called once per frame
    void Update()
    {   
        inputController.UpdateInputs();

        if (inputController.GetSpacePressedThisFrame())
        {
            playerController.ButtonWasPressed();
        }
        if (inputController.GetSpaceReleasedThisFrame())
        {
            playerController.ButtonWasReleased();
        }
        entityController.UpdateEntities();
        playerController.UpdatePlayer();
        motionBlurController.UpdateMotionBlur();
    }
}
