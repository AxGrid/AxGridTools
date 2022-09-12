using System.Collections.Generic;
using AxGrid.Base;
using UnityEngine;

namespace AxGrid.State
{
    public abstract class MonoBehaviourExtCollection<T> : MonoBehaviourExt
    {
        [SerializeField]
        private string smartStatePath = "OnStateUpdated";
        
                
        
        [OnStart]
        private void __Bind()
        {
            Settings.SmartState.AddAction<IEnumerable<T>>(smartStatePath, OnStateUpdated);
        }

        [OnDestroy]
        private void __UnBind()
        {
            Settings.SmartState.RemoveAction<IEnumerable<T>>(smartStatePath, OnStateUpdated);
        }

        public abstract void OnStateUpdated(IEnumerable<T> field);
    }
}