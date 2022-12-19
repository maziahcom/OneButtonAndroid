using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(BlueCubeEntity))]
public class EntityController : MonoBehaviour
{
    BlueCubeEntity blueCubeEntity;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    IEnumerator time()
    {
        while (true)
        {
            timeCount();
            yield return new WaitForSeconds(2);
        }
    }
    void timeCount()
    {
        //blueCubeEntity.cr
    }
    public void UpdateEntities()
    {

    }
}
