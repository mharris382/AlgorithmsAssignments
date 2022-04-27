using System;
using System.Collections.Generic;

namespace Assignment1
{
    public class AutoPriorityQueue<T> : PriorityQueue<T> where T :  IPriorityKey
    {
        public virtual bool Enqueue(T value)
        {
            return base.Enqueue(value, value.Key);
        }
    }

    /// <summary> </summary>
    /// <typeparam name="T"> cannot be pass-by-value type </typeparam>
    public class ReactivePriorityQueue<T> : AutoPriorityQueue<T> where T : class, IDynamicKey
    {
        private Dictionary<T, System.Action> _callbacks = new Dictionary<T, Action>(); 
        public override bool Enqueue(T value)
        {
            if (!_callbacks.ContainsKey(value)) 
                _callbacks.Add(value, () => _binaryHeap.ChangeKey(value, value.Key));
            
            value.OnKeyChanged += _callbacks[value];
            
            return base.Enqueue(value);
        }

        public override void Remove(T value)
        {
            value.OnKeyChanged -= _callbacks[value];
            base.Remove(value);
        }

        public override T ExtractMin()
        {
            var value = base.ExtractMin();
            value.OnKeyChanged -= _callbacks[value];
            return value;
        }
    }
}