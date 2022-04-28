using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1
{
    public class PriorityQueue<T>
    {
        protected readonly BinaryHeap<T> _binaryHeap;

        private int Capacity => _binaryHeap.Capacity;
        public int Count => _binaryHeap.Count;
        public PriorityQueue()
        {
            _binaryHeap = new BinaryHeap<T>();
        }

        public bool Enqueue(T value, float key)
        {
            if (_binaryHeap.IsAtCapacity())
            {
                //queue is full
                return false;
            }
            _binaryHeap.Insert(value, key);
            return true;
        }

        public void InitCapacity(int capacity)
        {
            _binaryHeap.StartHeap(capacity);
        }

        public T PeekMin()
        {
            return _binaryHeap.FindMin();
        }

        public void UpdateKey(T value, float key)
        {
            _binaryHeap.ChangeKey(value, key);
        }
        
        public virtual void Remove(T value)
        {
            _binaryHeap.Delete(value);
        }
        
        public virtual T ExtractMin()
        {
            return _binaryHeap.ExtractMin();
        }

        public override string ToString()
        {
            var copy = BinaryHeap<T>.Clone(_binaryHeap);
            StringBuilder sb = new StringBuilder();
            while (copy.Count > 0)
            {
                sb.Append(copy.ExtractMin());
                sb.Append(", ");
            }
            return sb.ToString();
        }

        public BinaryHeap<T> GetInternalHeap()
        {
            return _binaryHeap;
        }
    }
}