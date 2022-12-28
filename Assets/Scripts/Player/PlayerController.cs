using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using static PlayerController;
using static UnityEngine.Rendering.DebugUI;


public class PlayerController : MonoBehaviour
{
    public GameObject PlayerObject;
    public float angleDown = 34.0f;
    public float angleUp = -34.0f;
    public int rotationSpeed = 50;
    private float r;
    SwingState swingState;

    public void InitPlayer()
    {
        Assert.IsNotNull(PlayerObject, "Error: Add a PlayerObject to the PlayerController script");
        swingState = new SwingState();
    }

    public void ButtonWasPressed()
    {
        swingState.SetState(SwingState.State.ASCENDING);
    }
    public void ButtonWasReleased()
    {
        swingState.SetState(SwingState.State.DESCENDING);
    }

    // Update is called once per frame
    public void UpdatePlayer()
    {
        if(swingState.GetState() != SwingState.State.STATIC)
        {
            //use r to hold a rotation value matching the format in the editor
            r = PlayerObject.transform.localEulerAngles.z;
            if (r > 180)
                r -= 360f;
            else if (r < -180)
                r += 360f;
            
            if (swingState.GetState() == SwingState.State.ASCENDING)
            {
                if (r > angleUp)
                {
                    PlayerObject.transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * swingState.GetDirectionMultiplier(), Space.Self);
                }
                else
                {
                    swingState.SetState(SwingState.State.STATIC);
                }
            }

            else if(swingState.GetState() == SwingState.State.DESCENDING)
            {
                if (r < angleDown)
                {
                    PlayerObject.transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * swingState.GetDirectionMultiplier(), Space.Self);
                }
                else
                {
                    swingState.SetState(SwingState.State.STATIC);
                }
            }
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
