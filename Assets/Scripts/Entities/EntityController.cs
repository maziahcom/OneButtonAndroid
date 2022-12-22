using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(CubeEntity))]
public class EntityController : MonoBehaviour
{
    public enum Axis { X, Y, Z };
    public Axis axis = Axis.Z;
    public float movementSpeed = 10.0f;
    public float secondsDelay = 1.0f;
    private Vector3 dx;
    const int max_entities = 50;

    Entities entities;
    void Start()
    {
        Debug.Log("Entity controller started");
        entities = new Entities(max_entities);
        switch(axis)
        {
            case Axis.X:
                dx = Vector3.left;
                break;
            case Axis.Y:
                dx = Vector3.down;
                break;
            case Axis.Z:
                dx = Vector3.back;
                break;
            default:
                break;
        }
        StartCoroutine(time());
    }

    IEnumerator time()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsDelay);
            Debug.Log("Timer tick");
            timeCount();
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
            entities.MoveEntity(i, dx * movementSpeed * Time.deltaTime);
        }
    }

    public class Entities
    {
        private int max_entities;
        private int count_entities;
        List<CubeEntity.Entity> entitieList = new List<CubeEntity.Entity>();
        public Entities(int capacity)
        {
            max_entities = capacity;
            count_entities = 0;

        }

        public int CreateEntity()
        {
            if (count_entities < max_entities)
            {
                int nextID = GetNextAvailableID();
                if (nextID < 0)
                    return nextID;
                entitieList.Insert(nextID, new CubeEntity.Entity(nextID));
                count_entities++;
                Debug.Log("Entity Count = " + count_entities);
                return nextID;
            }
            return -1;
        }
        public void DeleteEntity(int id)
        {
            entitieList[id].Destroy();
            entitieList.RemoveAt(id);
            count_entities--;
        }
        public int GetCount()
        {
            return entitieList.Count;
        }

        public void PositionEntity(int id, Vector3 pos)
        {
            entitieList[id].SetPosition(pos);
        }

        public void MoveEntity(int id, Vector3 dx)
        {
            entitieList[id].Move(dx);
            if (entitieList[id].GetPosition().z < -20)
                DeleteEntity(id);
        }

        public int GetNextAvailableID()
        {
            for (int i = 0; i < max_entities; i++)
            {
                bool set = false;
                foreach (CubeEntity.Entity entitie in entitieList)
                {
                    if (entitie.GetID() == i)
                        set = true;
                }
                if (!set)
                    return i;
            }
            return -2;
        }

    }

}
