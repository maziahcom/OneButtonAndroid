using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using Object = UnityEngine.Object;
using Random = System.Random;

public class CubeEntity : MonoBehaviour
{
    public float disolveSpeed = 2.0f;


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
            disolveValue += disolveSpeed * Time.deltaTime;
            cube.transform.localScale *= 1 + ((1-disolveValue) * 4 * Time.deltaTime);
            cube.GetComponent<MeshRenderer>().material.SetFloat("_Disolve", disolveValue);
            if(disolveValue >= 1)
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
   
    private void PlaySound()
    {
        audioSourceFX.Play();
    }


    //--------- comment --------
    // we catch both OnTriggerEnter and OnTriggerStay
    // because there is no guarentee OnTriggerEnter will be caught
    // every time. (because we are not trying to use FixedUpdate)
    //--------------------------

    void OnTriggerStay(Collider other)
    {
        if (!_isTriggering)
            PlaySound();

        _isTriggering = true;
    }

    void OnTriggerEnter(Collider other)
    {

        if (!_isTriggering)
            PlaySound();
        _isTriggering = true;
    }
}
