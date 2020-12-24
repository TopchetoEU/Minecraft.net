using System;
using System.Collections;
using System.Collections.Generic;

namespace MinecraftNet
{
    public class ObservedCollection<T>: ICollection<T>, IList<T>
    {
        public event CollectionChangeEventHandler<T> ElementAdded;
        public event CollectionChangeEventHandler<T> ElementRemoved;
        public event CollectionChangeEventHandler<T> ElementChanged;
        public event EventHandler Cleared;

        private List<T> list = new List<T>();

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public T this[int index] {
            get => list[index];
            set {
                list[index] = value;

                ElementChanged?.Invoke(this, new CollectionChangeEventArgs<T>(index, value));
            }
        }

        public void Add(T item)
        {
            list.Add(item);
            ElementAdded?.Invoke(this, new CollectionChangeEventArgs<T>(Count, item));
        }
        public bool Remove(T item)
        {
            var index = list.IndexOf(item);
            var a = list.Remove(item);

            if (a) {
                ElementRemoved?.Invoke(this, new CollectionChangeEventArgs<T>(index, item));
            }

            return a;
        }

        public void Clear()
        {
            list.Clear();
            Cleared?.Invoke(this, new EventArgs());
        }

        public bool Contains(T item) => list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item)
        {
            list.Insert(index, item);

            ElementAdded?.Invoke(this, new CollectionChangeEventArgs<T>(index, item));
        }
        public void RemoveAt(int index)
        {
            var item = list[index];
            list.RemoveAt(index);

            ElementAdded?.Invoke(this, new CollectionChangeEventArgs<T>(index, item));
        }
    }
}
