using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class CubeEntity : MonoBehaviour
{
    public class Entity
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

        public void Move(Vector3 dx)
        {
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
    }
}
