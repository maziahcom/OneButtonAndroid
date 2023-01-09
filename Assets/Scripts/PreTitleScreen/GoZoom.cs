using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GoZoom : MonoBehaviour
{
    public enum SoundNumber
    {
        One,
        Two,
        Three
    }
    public SoundNumber soundNumber = SoundNumber.One;
    public float speed = 100.0f;
    public float soundDelaySeconds = 1.5f;
    public float startingDistanceZ = 230.0f;
    public float endingDistance = -120.0f;
    public int numberOfLoops = 99;
    private AudioSource audioSourceFX;
    private List<AudioClip> wooshFX = new List<AudioClip>();
    private Random random = new Random();
    private IEnumerator coroutine;
    private bool startZoom;
    private bool firstFrameZoomComplete;
    private Vector3 v3;
    private int currentLoop;
    private IEnumerator WaitAndZoom(System.Action<bool> callback)
    {
        yield return new WaitForSeconds(2.0f);
        callback(true);
    }

    private void callback(bool ok)
    {
        startZoom = ok;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentLoop = 0;
        //hide the object until it is ready to start is travel across the screen
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        startZoom = false;
        //firstFrameZoomComplete = false;
        v3 = gameObject.transform.localPosition;
        //gameObject.transform.localPosition = new Vector3(v3.x, v3.y, startingDistanceZ);

        InitSound();
        InitAndStartMainLoop();


    }
    private void InitSound()
    {
        audioSourceFX = gameObject.AddComponent<AudioSource>();
        audioSourceFX.loop = false;
        audioSourceFX.bypassEffects = true;
        audioSourceFX.bypassListenerEffects = true;
        audioSourceFX.bypassReverbZones = true;
        audioSourceFX.playOnAwake = false;
        audioSourceFX.priority = 5;
        audioSourceFX.volume = 1.0f;
        audioSourceFX.pitch = 1.0f;

        for (int i = 1; i <= 3; i++)
        {
            string displayString = i.ToString("000");
            wooshFX.Add((AudioClip)Resources.Load("SoundFX/PreTitleScreen/" + displayString));
        }

        if (soundNumber == SoundNumber.One)
            audioSourceFX.clip = wooshFX[0];
        else if (soundNumber == SoundNumber.Two)
            audioSourceFX.clip = wooshFX[1];
        else
            audioSourceFX.clip = wooshFX[2];
    }
    private void InitAndStartMainLoop()
    {
        gameObject.transform.localPosition = new Vector3(v3.x, v3.y, startingDistanceZ);
        startZoom = false;
        firstFrameZoomComplete = false;
        StartCoroutine(WaitAndZoom((someVariableBool) => { startZoom = true; }));
    }

    // Update is called once per frame
    void Update()
    {
        if (startZoom)
        {
            //first frame of startZoom
            if (!firstFrameZoomComplete)
            {
                firstFrameZoomComplete = true;
                //enable the mesh renderer on the first frame of its travel
                //because we dont want to see it before then
                //gameObject.GetComponent<MeshRenderer>().enabled = true;
                audioSourceFX.PlayDelayed(soundDelaySeconds);                             
            }
            gameObject.transform.localPosition += (Vector3.back * speed * Time.deltaTime);
            if(gameObject.transform.localPosition.z < endingDistance)
            {
                if (currentLoop < numberOfLoops)
                {
                    //reset everything
                    InitAndStartMainLoop();
                    currentLoop += 1;
                }
            }
        }
    }
}
