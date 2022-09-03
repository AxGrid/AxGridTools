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
            Settings.Model.EventManager.AddAction<T>(statePath, OnStateUpdated);    
        }

        [OnDestroy]
        private void __UnBind()
        {
            Settings.Model.EventManager.RemoveAction<T>(statePath, OnStateUpdated);    
        }
        
        public virtual void OnStateUpdated(T field)
        {
            
        }
        
        
    }
}