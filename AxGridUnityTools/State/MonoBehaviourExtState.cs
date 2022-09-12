using AxGrid.Base;
using UnityEngine;

namespace AxGrid.State
{
    public abstract class MonoBehaviourExtState<T> : MonoBehaviourExt
    {
        [SerializeField]
        private string smartStatePath = "OnStateUpdated";
        
        [OnStart]
        private void __Bind()
        {
            Settings.SmartState.AddAction<T>(smartStatePath, OnStateUpdated);
        }

        [OnDestroy]
        private void __UnBind()
        {
            Settings.SmartState.RemoveAction<T>(smartStatePath, OnStateUpdated);
        }

        public abstract void OnStateUpdated(T field);

    }
}