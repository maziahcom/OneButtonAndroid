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

    public enum Scenearios
    {
        intro,
        titleScreen,
        level1,
        level2,
        level3,
        win,
        lose
    }
    private Scenearios sceneario;
    private bool scenarioComplete;
    private Scenearios nextScenario;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputController = GetComponent<InputController>(); 
        entityController = GetComponent<EntityController>();
        environmentController = GetComponent<EnvironmentController>();
        uiController = GetComponent<UIController>();
        titleScreen = GetComponent<TitleScreen>();
    }
    void Start()
    {
        sceneario = Scenearios.titleScreen;

        titleScreen.InitTitleScreen();
        titleScreen.DisplayTitleScreen();
        environmentController.InitMaterials();
        entityController.InitEntities();
        entityController.StartEntitiesLevel1();
        inputController.InitInputs();
        playerController.InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.GetScenarioComplete())
        {
            playerController.GetNextScenario(out nextScenario); 
        }
        
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
    }
}
