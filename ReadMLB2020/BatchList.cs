using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadMLB2020
{
    public class BatchList<T> : List<T[]> where T : class
    {
        public event BatchListEventHandler<T> OnAdd;

        public new void Add(T[] item)
        {
            base.Add(item);
            OnAdd?.Invoke(this, new BatchListAddEventArgs<T>(item));
        }
    }
    public delegate Task BatchListEventHandler<T>(BatchList<T> sender, BatchListAddEventArgs<T> e) where T : class;
    public class BatchListAddEventArgs<T> : EventArgs
    {
        public T[] AddedItem { get; private set; }

        public BatchListAddEventArgs(T[] addedItem)
        {
            AddedItem = addedItem;
        }
    } 
}
