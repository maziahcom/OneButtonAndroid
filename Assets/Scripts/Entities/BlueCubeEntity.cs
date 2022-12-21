using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class BlueCubeEntity : MonoBehaviour
{
    public class Entities
    {
        private int max_entities;
        private int count_entities;
        List<Entity> entitieList = new List<Entity>();
        public Entities()
        {
            max_entities = 50;
            count_entities = 0;
            
        }

        public int CreateEntity()
        {
            if(count_entities < max_entities)
            {
                // int retval = count_entities;
                // entitieList.
                int nextID = GetNextAvailableID();
                if (nextID < 0)
                    return nextID;
                entitieList.Insert(nextID, new Entity(nextID));
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

        public void UpdateEntity(int id)
        {
            entitieList[id].Update();
            if (entitieList[id].GetPosition().z < -20)
                DeleteEntity(id);
        }

        public int GetNextAvailableID()
        {
            for (int i = 0; i < max_entities; i++)
            {
                bool set = false;
                foreach (Entity entitie in entitieList)
                {
                    if (entitie.GetID() == i)
                        set = true;
                }
                if(!set)
                    return i;
            }
            return -2;
        }

    }
    class Entity
    {
        GameObject cube;
        private int _id;
        private string _name;
        public Entity(int id) 
        {
            _id = id;
            _name = "blue_cube" + id.ToString();
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = _name;
        }

        public void Update()
        {
            cube.transform.position -= new Vector3(0,0,20 * Time.deltaTime);

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
    }
}
