using System.Collections.Generic;
using AxGrid.Base;
using UnityEngine;

namespace AxGrid.Tools.Array
{
    public class LinearArray : MonoBehaviourExt
    {
        public GameObject prototype;
        public Vector3 delta;
        public bool onStart = true;
        public int count = 10;
        public GameObject[] objects = new GameObject[0];
        
        [OnAwake]
        private void createOnStart()
        {
            if (onStart) Create();    
        }

        public void Create()
        {
            Clear();
            objects = new GameObject[count];
            for(var i=0;i<count;i++)
            {
                var obj = Instantiate(prototype, this.transform, false);
                obj.transform.position = transform.position + delta * i;
                objects[i] = obj;
            }
        }
        
        public void Clear()
        {
            foreach (var o in objects)
                Destroy(o);
            objects = new GameObject[0];
        }

    }
}