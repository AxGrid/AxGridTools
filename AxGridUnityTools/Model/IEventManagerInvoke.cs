namespace AxGrid.Model
{
    public interface IEventManagerInvoke
    {
        void Invoke(string eventName, params object[] args);
    }
}