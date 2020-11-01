using AxGrid.Model;
using UnityEngine;

namespace AxGrid.Base
{
    public class Binder : MonoBehaviourExt
    {
        [Tooltip("Use Global Model")]
        public bool globalModel = false;

        /// <summary>
        /// Proxy
        /// </summary>
        protected override DynamicModel Model => globalModel ? Settings.GlobalModel : Settings.Model; 
    }
}