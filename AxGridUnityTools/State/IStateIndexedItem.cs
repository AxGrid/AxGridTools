namespace AxGrid.State
{
    public interface IStateIndexedItem<in T>
    {
        int StateItemIndex { get; set; }
        void RefreshStateItem(T state);
    }
}