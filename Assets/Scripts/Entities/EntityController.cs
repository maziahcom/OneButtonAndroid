using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;
using Random = System.Random;

[RequireComponent(typeof(CubeEntity))]
//[RequireComponent(typeof(LevelReader))]
public class EntityController : MonoBehaviour
{
    public enum Axis { X, Y, Z };
    public Axis axis = Axis.Z;
    public float movementSpeed = 10.0f;
    public float secondsDelay = 1.0f;
    public Material cube_material;
    public string ParamNameDisolve = "change this to your material/shader reference for disolve";
    
    //public AudioSource hitSound1;

    private Vector3 dx;
    private const int max_entities = 50;
    private Random random = new Random();
    private Entities entities;
    private const float yUp = 2.9f;
    private const float yDown = -0.75f;

    LevelReader levels;

    private float delta;

    int sequenceCounter;
    bool sequenceDataLoaded;

    private List<float> timestamps;
    private List<LevelReader.Reader.NextEvent> nextevents;

    public void InitEntities()
    {
        //reader = new LevelReader.Reader();
        //LevelReader.Lo
        sequenceCounter = 0;
        sequenceDataLoaded = false;
        levels = new LevelReader();
        timestamps = new List<float>();
        nextevents = new List<LevelReader.KeyEvents.NextEvent>();

        bool success = levels.LoadLevel1();
        if(success)
        {
            //we now have access the structure class - note: it's buried deep TODO.. fix this class mis-structure
            timestamps = levels.reader.structure.GetTimestamps();
            nextevents = levels.reader.structure.GetNextEvents();
            delta = -1;
            sequenceDataLoaded=true;
        }
        else
        {
            Debug.LogError("Level data not loaded error: 202-5");
        }


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
    }

    public void StartEntitiesLevel1()
    {

        //StartCoroutine(time());
    }

    /*IEnumerator time()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsDelay - (float)random.NextDouble());
            timeCount();
            //----------- Log ------------//
            //Debug.Log("Timer tick");
            //----------------------------//
        }
    }*/
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
    Vector3 GenerateNextPosition(bool rndY = true)
    {
        float yRange = yUp - yDown;
        int yRangeInt = (int)(yRange * 100);
        int r = random.Next(0,yRangeInt);
        float f;

        if (rndY)
            f = (r / 100.0f) + yDown;
        else
            f = yDown + (yRange / (float)2);

        return new Vector3(0, f, 40);
    }
    public void UpdateEntities()
    {   /*
        if (sequenceDataLoaded)
        {
            if (delta == -1)
            {
                //this is the first frame
                delta = 0;
            }
            else
                delta += Time.deltaTime; 

            if (timestamps[sequenceCounter] <= delta)
            {
                //generate an entity and increment counter
                int id = entities.CreateEntity(cube_material);
                if (id >= 0)
                {
                    entities.PositionEntity(id, GenerateNextPosition());
                }

                sequenceCounter++;

                //reset delta
                delta = 0;

                if (sequenceCounter >= timestamps.Count)
                    sequenceDataLoaded = false;
            }           
        }*/

        entities.MoveEntities(movementSpeed * Time.deltaTime * dx);
    }

    public void RequestSpawn(bool on, byte channel, byte note)
    {
        if (on)
        {
            int id = entities.CreateEntity(cube_material);
            if (id >= 0)
            {
                entities.PositionEntity(id, GenerateNextPosition(false));
            }
        }
    }

    public int GetDeadCount()
    {
        //get and reset the deadcount (the getter will add the dead to the scoreboard so the reset can be performed immediately)
        int d = entities.GetDeadCount();
        entities.ResetDeadCount();
        return d;
    }
    private class Entities
    {
        private int max_entities;
        List<CubeEntity> entitieList = new List<CubeEntity>();
        
        //keep count of entities hit by the player. This is for score-keeping purposes
        private int deadCount;

        public Entities(int capacity)
        {
            max_entities = capacity;
            deadCount = 0;
        }

        public int CreateEntity(Material m)
        {
            if (entitieList.Count < max_entities)
            {
                int nextID = GetNextAvailableID();
                if (nextID < 0)
                    return nextID;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);           
                cube.GetComponent<MeshRenderer>().material = m;
                //cube.AddComponent<AudioSource>();
                //cube.GetComponent<AudioSource>().clip = a.clip;
                entitieList.Insert(nextID, cube.AddComponent<CubeEntity>());
                entitieList[nextID].NewEntity(nextID);
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
        }
        public int GetCount()
        {
            return entitieList.Count;
        }
        public int GetDeadCount()
        {
            return deadCount;
        }
        public void ResetDeadCount()
        {
            deadCount = 0;
        }

        public void PositionEntity(int id, Vector3 pos)
        {
            entitieList[id].SetPosition(pos);
        }

        public void MoveEntities(Vector3 dx)
        {
            for(int i = entitieList.Count - 1; i >= 0; i--)
            {
                entitieList[i].Move(dx);

                if (entitieList[i].GetTriggeredStatus() == true)
                    deadCount++;

                if (entitieList[i].GetPosition().z < -20)
                    DeleteEntity(i);
                else if (entitieList[i].GetIsDead())
                    DeleteEntity(i);
                //-----------comment------------
                //Note: dont put anything here after DeleteEntity
                //because it will produce index out of bounds errors (the entity has been deleted)
                //------------------------------
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
