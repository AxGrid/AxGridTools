using AxGrid.Base;
using UnityEngine;

namespace AxGrid.Tools.Array
{
    public class CircleArray : MonoBehaviourExt
    {
        public GameObject prototype;
        public float radius = 1f;
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
                var t = i * 360f / count;
                var obj = Instantiate(prototype, this.transform, false);
                obj.transform.position = Utils.Polar.FromPolar(radius, t);
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