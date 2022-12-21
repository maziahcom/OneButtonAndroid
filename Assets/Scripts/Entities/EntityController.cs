using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(BlueCubeEntity))]
public class EntityController : MonoBehaviour
{
    BlueCubeEntity.Entities entities;
    void Start()
    {
        Debug.Log("Entity controller started");
        entities = new BlueCubeEntity.Entities();
        StartCoroutine(time());
    }

    void Update()
    {
        
    }
    IEnumerator time()
    {
        while (true)
        {
            Debug.Log("Timer tick");
            timeCount();
            yield return new WaitForSeconds(0.4f);
        }
    }
    void timeCount()
    {
        Debug.Log("Creating entity");

        int id = entities.CreateEntity();
        if (id >= 0)
            entities.PositionEntity(id, new Vector3(0, 0, 20));
        Debug.Log("Creating entity return value: " + id.ToString());
    }
    public void UpdateEntities()
    {
        for(int i = 0; i < entities.GetCount(); i++)
        {
            entities.UpdateEntity(i);
        }
    }
}
