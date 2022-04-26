using System;

namespace Assignment1
{
    public class PriorityQueue<T>
    {
        private readonly BinaryHeap<T> _binaryHeap;

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
        
        public void Remove(T value)
        {
            _binaryHeap.Delete(value);
        }
        public T ExtractMin()
        {
            return _binaryHeap.ExtractMin();
        }

        public override string ToString()
        {
            return _binaryHeap.ToString();
        }
    }
}