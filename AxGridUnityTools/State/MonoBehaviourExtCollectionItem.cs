using AxGrid.Base;

namespace AxGrid.State
{
    public abstract class MonoBehaviourExtCollectionItem<T> : MonoBehaviourExt, IStateIndexedItem<T>
    {
        public int StateItemIndex { get; set; }
        public abstract void RefreshStateItem(T state);
    }
}