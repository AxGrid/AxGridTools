using System.Collections.Generic;
using AxGrid.Base;
using UnityEngine;

namespace AxGrid.Tools.Array
{
    public class GridArray : MonoBehaviourExt
    {
        public GameObject prototype;
        public Vector3 deltaX;
        public Vector3 deltaY;
        public bool onStart = true;
        public int countX = 10;
        public int countY = 10;
        public GameObject[,] objects = new GameObject[0, 0];
        
        [OnAwake]
        private void createOnStart()
        {
            if (onStart) Create();    
        }

        public void Create()
        {
            Clear();
           
            objects = new GameObject[countX, countY];
            for(var y=0;y<countY;y++)
            for(var x=0;x<countX;x++)
            {
                var obj = Instantiate(prototype, this.transform, false);
                obj.transform.position = transform.position + deltaX * x + deltaY * y;
                objects[x,y] = obj;
            }
        }

        public void Clear()
        {
            foreach (var o in objects)
                Destroy(o);
            objects = new GameObject[0, 0];
        }

    }
}