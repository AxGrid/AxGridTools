using System;
using System.Collections;
using System.Collections.Generic;

namespace AxGrid.Model
{
    public class DynamicList<T> : IList<T>, IDynamicObject
    {
        public DynamicModel ModelLink { get; set; }
        public string ModelField { get; set; }

        private List<T> baseList;
        
        public void Refresh()
        {
            ModelLink.Refresh(ModelField); 
        }


        public IEnumerator<T> GetEnumerator()
        {
            return baseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return baseList.GetEnumerator();
        }

        public void Add(T item)
        {
            baseList.Add(item);
            Refresh();
        }

        public void Clear()
        {
        }

        public bool Contains(T item)
        {
            return baseList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            baseList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (baseList.Remove(item))
            {
                Refresh();
                return true;
            }
            return false;
        }

        public int Count => baseList.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            return baseList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            baseList.Insert(index, item);
            Refresh();
        }

        public void RemoveAt(int index)
        {
            baseList.RemoveAt(index);
            Refresh();
        }

        public T this[int index]
        {
            get => baseList[index];
            set
            {
                baseList[index] = value;
                Refresh();
            }
        }
    }

}