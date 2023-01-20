using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using static PlayerController;
using static UnityEngine.Rendering.DebugUI;
using Random = System.Random;


public class PlayerController : MonoBehaviour
{
    public GameObject PlayerObject;
    public float angleDown = 34.0f;
    public float angleUp = -34.0f;
    public float posUp = 7.0f;
    public float posDown = 2.24f;
    public int rotationSpeed = 50;
    private float r;
    private float posY;
    private SwingState swingState;
    private List<AudioClip> wizzFX = new List<AudioClip>();
    private AudioSource audioSourceFX;
    private Random random = new Random();
    bool firstpress = true;
    public void InitPlayer()
    {

        Assert.IsNotNull(PlayerObject, "Error: Add a PlayerObject to the PlayerController script");
        swingState = new SwingState();
        PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, posDown, PlayerObject.transform.position.z);
        InitSoundFX();
    }
    private void InitSoundFX()
    {
        audioSourceFX = gameObject.AddComponent<AudioSource>();
        audioSourceFX.loop = false;
        audioSourceFX.bypassEffects = true;
        audioSourceFX.bypassListenerEffects = true;
        audioSourceFX.bypassReverbZones = true;
        audioSourceFX.playOnAwake = false;
        audioSourceFX.priority = 5;
        audioSourceFX.volume = 0.1f;
        audioSourceFX.pitch = 1.0f;

        for (int i = 2; i <= 16; i++)
        {
            string displayString = i.ToString("000");
            wizzFX.Add((AudioClip)Resources.Load("SoundFX/BulletWizzBy/" + displayString));
        }
    }

    private void PlaySound()
    {
        audioSourceFX.clip = wizzFX[random.Next(0, wizzFX.Count)];
        audioSourceFX.Play();
    }

    public void ButtonWasPressed()
    {
        if(firstpress)
        {
            Debug.Log("first press time: " + Time.unscaledTime.ToString());
            firstpress = false;
        }
        PlaySound();
        swingState.SetState(SwingState.State.ASCENDING);
    }
    public void ButtonWasReleased()
    {
        PlaySound();
        swingState.SetState(SwingState.State.DESCENDING);
    }





    public void UpdatePlayer()
    {
        if(swingState.GetState() != SwingState.State.STATIC)
        {
            posY = PlayerObject.transform.position.y;
            if (swingState.GetState() == SwingState.State.ASCENDING)
            {
                if (posY < posUp)
                {
                    PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y + (Time.deltaTime * rotationSpeed), PlayerObject.transform.position.z);
                }
                else
                {
                    swingState.SetState(SwingState.State.STATIC);
                }
            }
            else if (swingState.GetState() == SwingState.State.DESCENDING)
            {
                if (posY > posDown)
                {
                    PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y + (Time.deltaTime * rotationSpeed * -1), PlayerObject.transform.position.z);
                }
                else
                {
                    swingState.SetState(SwingState.State.STATIC);
                }
            }
            //keep player within bounds
            posY = PlayerObject.transform.position.y;
            if(posY > posUp)
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, posUp, PlayerObject.transform.position.z);
            else if (posY < posDown)
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, posDown, PlayerObject.transform.position.z);
        }
    }

    public class SwingState
    {
        public enum State { DESCENDING, ASCENDING, STATIC };
        private State state;
        public SwingState()
        {
            state = State.STATIC;
        }
        public void SetState(State s)
        {
            state = s;
        }
        public State GetState()
        {
            return state;
        }
        public int GetDirectionMultiplier()
        {
            if(state == State.DESCENDING)
                return 1;
            if(state == State.ASCENDING)
                return -1;
            return 0;
        }
    }
}
