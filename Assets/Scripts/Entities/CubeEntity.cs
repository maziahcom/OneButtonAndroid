using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class CubeEntity : MonoBehaviour
{
    GameObject cube;
    private int _id;
    private string _name;
    private BoxCollider boxCollider;
    private bool _isTriggering;
    Rigidbody rb;
    private float disolveValue;
    private bool _isEndOfLife;

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
        //rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //rb.constraints = RigidbodyConstraints.FreezeRotation + RigidbodyConstraints.;
        boxCollider.isTrigger = true;
        //Debug.Log("New entity started: " + name);
        disolveValue = 0;
        cube.GetComponent<MeshRenderer>().material.SetFloat("_Disolve", disolveValue);
        _isEndOfLife = false;
    }

    public void Move(Vector3 dx)
    {
        if (_isTriggering)
        {
            dx = Vector3.zero;
            disolveValue += 4 * Time.deltaTime;
            cube.GetComponent<MeshRenderer>().material.SetFloat("_Disolve", disolveValue);
            if(disolveValue >= 1)
                _isEndOfLife = true;
        }
        cube.transform.position += dx;
        //cube.GetComponent<Rigidbody>().MovePosition(cube.transform.position + dx);
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


    void OnTriggerStay(Collider other)
    {
        _isTriggering = true;
        //Debug.Log("got OnTriggerStay: " + name);
    }

    void OnTriggerEnter(Collider other)
    {
        _isTriggering = true;
        //Debug.Log("got OnTriggerEnter: " + name);
    }
}
