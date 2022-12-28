using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(CubeEntity))]
public class EntityController : MonoBehaviour
{
    public enum Axis { X, Y, Z };
    public Axis axis = Axis.Z;
    public float movementSpeed = 10.0f;
    public float secondsDelay = 1.0f;
    private Vector3 dx;
    const int max_entities = 50;
    private Random random = new Random();
    Entities entities;
    private const float yUp = 2.9f;
    private const float yDown = -0.75f;
    public Material cube_material;
    public string ParamNameDisolve = "change this to your material/shader reference for disolve";
    public void InitEntities()
    {
        //----------- Log ------------//
        //Debug.Log("Entity controller started");
        //-----------     ------------//
        entities = new Entities(max_entities);
        switch (axis)
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
            timeCount();
            //----------- Log ------------//
            //Debug.Log("Timer tick");
            //----------------------------//
        }
    }
    void timeCount()
    {
        int id = entities.CreateEntity(cube_material);
        if (id >= 0)
        {
            entities.PositionEntity(id, GenerateNextPosition());
        }
        //----------- Log ------------//
        //Debug.Log("Creating entity return value: " + id.ToString());
        //----------------------------//
    }
    Vector3 GenerateNextPosition()
    {
        float yRange = yUp - yDown;
        int yRangeInt = (int)(yRange * 100);
        int r = random.Next(0,yRangeInt);
        float f = (r / 100.0f) + yDown;
        return new Vector3(0, f, 20);
    }
    public void UpdateEntities()
    {
        entities.MoveEntities(dx * movementSpeed * Time.deltaTime);
    }

    private class Entities
    {
        private int max_entities;
        private int count_entities;
        List<CubeEntity> entitieList = new List<CubeEntity>();
        public Entities(int capacity)
        {
            max_entities = capacity;
            count_entities = 0;

        }

        public int CreateEntity(Material m)
        {
            if (count_entities < max_entities)
            {
                int nextID = GetNextAvailableID();
                if (nextID < 0)
                    return nextID;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.GetComponent<MeshRenderer>().material = m;
                entitieList.Insert(nextID, cube.AddComponent<CubeEntity>());
                entitieList[nextID].NewEntity(nextID);
                count_entities++;
                //----------- Log ------------//
                //Debug.Log("Entity Count = " + count_entities);
                //----------------------------//
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
            if(entitieList[id].GetIsDead())
                DeleteEntity(id);
        }
        public void MoveEntities(Vector3 dx)
        {
            for(int i = entitieList.Count - 1; i >= 0; i--)
            {
                entitieList[i].Move(dx);
                if (entitieList[i].GetPosition().z < -20)
                    DeleteEntity(i);
                else if (entitieList[i].GetIsDead())
                    DeleteEntity(i);
            }
        }

        public int GetNextAvailableID()
        {
            //--------comment---------------//
            //iterate through entities, setting flag to false, and returning when GetID returns no ID (ie, we found free slot)
            //------------------------------//
            for (int i = 0; i < max_entities; i++)
            {
                bool set = false;
                foreach (CubeEntity entitie in entitieList)
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
