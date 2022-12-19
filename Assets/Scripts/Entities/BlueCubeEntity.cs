using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCubeEntity : MonoBehaviour
{
    private GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public class Entities
    {
        int max_entities;
        int count_entities;
        List<Entity> entities = new List<Entity>();
        public Entities()
        {
            max_entities = 50;
            count_entities = 0;
            
        }

        public bool CreateEntity()
        {
            if(count_entities < max_entities)
            {
                entities.Add(new Entity());
                count_entities++;
                return true;
            }
            return false;
        }

    }
    class Entity
    {
        GameObject cube;
        public Entity() 
        {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }
}
