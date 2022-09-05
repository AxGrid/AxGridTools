using AxGrid.Base;
using UnityEngine;

namespace AxGrid.State
{
    public abstract class MonoBehaviourExtState<T> : MonoBehaviourExt
    {
        [SerializeField]
        private string statePath = "OnStateUpdated";
        
        [OnStart]
        private void __Bind()
        {
            
        }

        [OnDestroy]
        private void __UnBind()
        {
                
        }
        
        public virtual void OnStateUpdated(T field)
        {
            
        }
        
        
    }
}