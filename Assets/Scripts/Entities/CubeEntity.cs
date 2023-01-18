using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.VFX;
using Object = UnityEngine.Object;
using Random = System.Random;

public class CubeEntity : MonoBehaviour
{
    public float disolveSpeed = 2.0f;
    public string nameOfMainPlayerCollider = "PlayerCube";
    private GameObject effectsObject;
    private GameObject effectsObjectClone;
    private Random random = new Random();
    private AudioSource hitSound1;
    private GameObject cube;
    private int _id;
    private string _name;
    private BoxCollider boxCollider;
    private bool _isTriggering;
    private Rigidbody rb;
    private float disolveValue;
    private bool _isEndOfLife;
    private AudioSource audioSourceFX;
    private List<AudioClip> impactFX = new List<AudioClip>();
    private bool collisionBroadcastedAlready;

    private enum HitDir
    {
        Up,
        Down,
        None
    }

    public void NewEntity(int id) 
    {
        _id = id;
        _name = "blue_cube" + id.ToString();
        _isTriggering = false;
        cube = gameObject;
        cube.name = _name;
        boxCollider = cube.AddComponent<BoxCollider>();
        rb = cube.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        boxCollider.isTrigger = true;
        disolveValue = 0;
        cube.GetComponent<MeshRenderer>().material.SetFloat("_Disolve", disolveValue);
        _isEndOfLife = false;
        
        InitSoundFX();

        effectsObject = GameObject.FindGameObjectWithTag("sparksEffect");
        effectsObjectClone = GameObject.Instantiate(effectsObject);
        effectsObjectClone.transform.parent = transform;
        effectsObjectClone.transform.localPosition = Vector3.zero;

        collisionBroadcastedAlready = false;
        //cube.AddComponent<VisualEffect>();
        //cube.GetComponent<VisualEffect>().visualEffectAsset = visualEffectSparks.visualEffectAsset;
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
        audioSourceFX.volume = 1.0f;
        audioSourceFX.pitch = 1.0f;

        for (int i = 1; i <= 10; i++)
        {
            string displayString = i.ToString("000");
            impactFX.Add((AudioClip)Resources.Load("SoundFX/CubeImpacts/" + displayString));
        }
        
        audioSourceFX.clip = impactFX[random.Next(0, impactFX.Count)];
    }
    public void Move(Vector3 dx)
    {
        if (_isTriggering)
        {
            //dx = Vector3.zero;
            //effectsObjectClone.GetComponent<VisualEffect>().transform.position += dx;
            disolveValue += disolveSpeed * Time.deltaTime;
            cube.transform.localScale *= 1 + ((1-disolveValue) * 4 * Time.deltaTime);
            cube.GetComponent<MeshRenderer>().material.SetFloat("_Disolve", disolveValue);
            if(disolveValue >= 2)
                if(!gameObject.GetComponent<AudioSource>().isPlaying)
                    _isEndOfLife = true;
        }
        cube.transform.position += dx;
    }

    public void SetPosition(Vector3 pos)
    {
        cube.transform.position = pos;
    }
    public Vector3 GetPosition()
    {
        return cube.transform.position;
    }
    public int GetID()
    {
        return _id;
    }
    public void Destroy()
    {
        Object.Destroy(cube);
    }
    public bool GetIsDead()
    {
        return _isEndOfLife;
    }

    public bool GetTriggeredStatus()
    {
        //------------------- comment --------------
        //returns true only once then false every other time
        //this is used for player score-keeping
        //basically if the triggering is true, and broadcasted is false, then we can add to the playuer's score
        //------------------------------------------
        if(!_isTriggering)
            return false;
        if (collisionBroadcastedAlready)
            return false;
        collisionBroadcastedAlready = true;
        return true;
    }
    private void PlaySound()
    {
        audioSourceFX.Play();
        effectsObjectClone.SetActive(true);//GetComponent<VisualEffect>().Play();
        effectsObjectClone.GetComponent<VisualEffect>().enabled = true;
    }

    //--------- comment --------
    // we catch both OnTriggerEnter and OnTriggerStay
    // because there is no guarentee OnTriggerEnter will be caught
    // every time. (because we are not trying to use FixedUpdate)
    //--------------------------
    private void HandleHit(Collider other)
    {
        if (other.gameObject.name == nameOfMainPlayerCollider)
        {
            if (!_isTriggering)
                PlaySound();

            _isTriggering = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        HandleHit(other);
    }

    void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }

}
