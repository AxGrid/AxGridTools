using AxGrid.Model;

namespace AxGrid.State
{
    public class SettingsEventManager : IEventManagerInvoke
    {
        public void Invoke(string eventName, params object[] args)
        {
            Settings.Invoke(eventName, args);
        }
    }
}