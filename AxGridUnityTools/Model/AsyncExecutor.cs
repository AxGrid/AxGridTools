using UnityEngine;

namespace AxGrid.Model
{
    public class AsyncExecutor : MonoBehaviour
    {
        public void Update()
        {
            Settings.Model?.EventManager.ExecuteAsync(deltaTime:Time.deltaTime);
        }
    }
}