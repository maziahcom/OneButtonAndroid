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
    //private TitleScreen titleScreen;
    private MotionBluWithInstance motionBlurController;
    //private MidiController midiController;
    private EntitySpawnLoader entitySpawnLoader;
    private ComboCounter comboCounter;

    public float delaySecondsBeforeFirstUpdate = 1.0f;
    private float awakeTime;
    //private List<MidiController.MidiJob> midiJobs;
    
    void Awake()
    {
        awakeTime = Time.unscaledTime;
        //remove this later for final build (it can be set ealier in the scene loader)
        Application.targetFrameRate = 120;//Screen.currentResolution.refreshRate;

        //load componentt handles
        playerController = GetComponent<PlayerController>();
        inputController = GetComponent<InputController>(); 
        entityController = GetComponent<EntityController>();
        environmentController = GetComponent<EnvironmentController>();
        uiController = GetComponent<UIController>();
        //titleScreen = GetComponent<TitleScreen>();
        motionBlurController = FindObjectOfType<MotionBluWithInstance>();
        //midiController = FindObjectOfType<MidiController>();
        entitySpawnLoader = FindObjectOfType<EntitySpawnLoader>();
        comboCounter = FindObjectOfType<ComboCounter>();

        //instantiate components
        environmentController.InitMaterials();
        entityController.InitEntities();
        entityController.StartEntitiesLevel1();
        entitySpawnLoader.Init(entitySpawnLoader.fileamePublic, 4);
        inputController.InitInputs();
        uiController.InitUI();
        //midiController.InitMidi();
        playerController.InitPlayer();
        motionBlurController.InitMotionBlur(Application.targetFrameRate);
        comboCounter.Init();

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Allow all the components a moment to load before starting the Update() loop;
        if (Time.unscaledTime - awakeTime < delaySecondsBeforeFirstUpdate)
            return;

        //midiController and entityController don't directly interface each other
        //this game manager script reads pending midi events list and
        //dispatches requests to the entity spawner accordingly

        //midiController.UpdateMidi();
        //midiJobs = midiController.GetAndClearMidiJobs();
        /*for(int i = 0; i < midiJobs.Count; i++)
        {
            entityController.RequestSpawn(midiJobs[i].on, midiJobs[i].channel, midiJobs[i].note);
        }*/
        //if(midiJobs.Count > 0)
        //    entityController.RequestSpawn(midiJobs[0].on, midiJobs[0].channel, midiJobs[0].note);

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
        byte b = entitySpawnLoader.SpawnNext();
        entityController.RequestSpawnByte(b);
        int dc = entityController.GetDeadCount();
        uiController.ScoreAdd(dc);
        bool _ascending;
        bool _descending;
        bool _static;
        playerController.UpdatePlayer(out _ascending, out _descending, out _static);
        comboCounter._Update(dc, _ascending, _descending, _static);
        motionBlurController.UpdateMotionBlur();
    }
}
